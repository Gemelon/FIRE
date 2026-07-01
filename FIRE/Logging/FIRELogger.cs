/********************************************************************************
 * MIT License
 *
 * Copyright (c) 2026 by Thomas Stoll
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 ********************************************************************************/

using System.Globalization;

namespace FIRE.Logging;

/// <summary>
/// Numeric severity levels for FIRE log entries.
/// </summary>
/// <remarks>
/// The numeric values allow simple greater-than comparison for level filtering.
/// <list type="bullet">
/// <item><description><see cref="Debug"/>   (0) – Fine-grained diagnostic information.</description></item>
/// <item><description><see cref="Info"/>    (1) – Normal stage-start/complete messages.</description></item>
/// <item><description><see cref="Warning"/> (2) – Non-fatal anomalies (e.g. missing metadata).</description></item>
/// <item><description><see cref="Error"/>   (3) – Recoverable errors that affected a single file.</description></item>
/// <item><description><see cref="Critical"/>(4) – Unrecoverable failures that aborted a stage.</description></item>
/// </list>
/// </remarks>
public enum FIRELogLevel
{
    Debug    = 0,
    Info     = 1,
    Warning  = 2,
    Error    = 3,
    Critical = 4,
}

/// <summary>
/// Thread-safe, per-stage Markdown log writer for the FIRE catalog.
/// </summary>
/// <remarks>
/// <para>
/// <strong>File naming:</strong>
/// Each stage gets its own daily file:
/// <c>{LogFileName}.col.{yyyyMMdd}.md</c>, <c>.gen.</c>, <c>.exe.</c>.
/// When no stage is active (e.g. pre-stage warnings), the suffix <c>.misc.</c> is used.
/// </para>
/// <para>
/// <strong>Rotation:</strong>
/// When the active file grows beyond <see cref="LoggingConfiguration.MaxFileSizeBytes"/>,
/// writing continues in a new segment: <c>{name}.col.{date}.1.md</c>, <c>.2.md</c>, …
/// The recommended size limit is 10 MB so any common text editor can open the file.
/// </para>
/// <para>
/// <strong>Retention:</strong>
/// At construction time, files older than <see cref="LoggingConfiguration.MaxAgeDays"/> days
/// are deleted from the log directory. A 30-day window covers a reasonable audit period;
/// reduce it in high-volume environments to limit disk usage.
/// </para>
/// <para>
/// <strong>Format:</strong>
/// Every entry is one Markdown table row:
/// <c>| timestamp | LEVEL | stage | message |</c>
/// Each new file starts with a Markdown table header.
/// </para>
/// </remarks>
internal sealed class FIRELogger : IDisposable
{
    // Markdown table header written once per log file segment.
    private const string TableHeader =
        "| Timestamp | Level | Stage | Message |\n" +
        "|-----------|-------|-------|---------|";

    private readonly LoggingConfiguration _config;
    private readonly FIRELogLevel _minLevel;
    private readonly object _lock = new();

    private StreamWriter? _writer;
    private string _currentFilePath = string.Empty;
    private string _currentStageTag = string.Empty;
    private DateOnly _currentDate;
    private int _segmentIndex;
    private bool _disposed;

    /// <summary>
    /// Creates a new <see cref="FIRELogger"/> and purges files older than the configured retention window.
    /// </summary>
    /// <param name="config">The logging configuration from the FIRE configuration file.</param>
    internal FIRELogger(LoggingConfiguration config)
    {
        _config = config;
        _minLevel = ParseLevel(config.LogLevel);

        Directory.CreateDirectory(config.LogFilePath);
        PurgeOldFiles();
    }

    /// <summary>
    /// Writes a log entry if its level is at or above the configured minimum.
    /// </summary>
    /// <param name="level">Severity of the message.</param>
    /// <param name="stage">Current catalog stage tag (e.g. "col", "gen", "exe"); null for miscellaneous entries.</param>
    /// <param name="message">The plain-text message to log. Pipe characters are escaped.</param>
    internal void Log(FIRELogLevel level, string? stage, string message)
    {
        if (level < _minLevel) return;

        var stageTag = stage ?? "misc";
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);
        var timestamp = now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        var safeMessage = message.Replace("|", "\\|");

        lock (_lock)
        {
            EnsureWriter(stageTag, today);
            _writer!.WriteLine($"| {timestamp} | {level,-8} | {stageTag,-4} | {safeMessage} |");
            _writer.Flush();

            RotateIfNeeded(stageTag, today);
        }
    }

    /// <summary>
    /// Writes a continuation line beneath the previous log entry at the same level.
    /// </summary>
    /// <remarks>
    /// The timestamp, level, and stage columns are left empty so the message
    /// appears visually indented in the same message column as the preceding row.
    /// The message is prefixed with <c>→ </c> to signal it is a continuation.
    /// The entry is only written when <paramref name="level"/> is at or above the
    /// configured minimum and a writer is already open (i.e. <see cref="Log"/> was
    /// called first for the same stage/date).
    /// </remarks>
    /// <param name="level">Severity — must be at or above the configured minimum.</param>
    /// <param name="message">The continuation text. Pipe characters are escaped.</param>
    internal void LogContinuation(FIRELogLevel level, string message)
    {
        if (level < _minLevel) return;

        var safeMessage = message.Replace("|", "\\|");

        lock (_lock)
        {
            if (_writer == null) return;
            _writer.WriteLine($"|                     |          |      | \u2192 {safeMessage} |");
            _writer.Flush();
        }
    }

    /// <summary>
    /// Ensures that a writer is open for the active stage and date.
    /// </summary>
    /// <param name="stageTag">Short stage tag used in the log file name.</param>
    /// <param name="today">Current local date used for daily log partitioning.</param>
    private void EnsureWriter(string stageTag, DateOnly today)
    {
        if (_writer != null && stageTag == _currentStageTag && today == _currentDate)
            return;

        CloseWriter();
        _currentStageTag = stageTag;
        _currentDate = today;
        _segmentIndex = 0;
        OpenSegment();
    }

    /// <summary>
    /// Opens the current log segment and writes the table header for new files.
    /// </summary>
    private void OpenSegment()
    {
        _currentFilePath = BuildPath(_currentStageTag, _currentDate, _segmentIndex);
        var isNew = !File.Exists(_currentFilePath);

        var stream = new FileStream(_currentFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        _writer = new StreamWriter(stream, System.Text.Encoding.UTF8, bufferSize: 4096, leaveOpen: false);

        if (isNew)
        {
            _writer.WriteLine($"# FIRE Log — {_currentStageTag.ToUpperInvariant()} — {_currentDate:yyyy-MM-dd}");
            _writer.WriteLine();
            _writer.WriteLine(TableHeader);
        }
    }

    /// <summary>
    /// Rotates to the next segment file when the current file exceeds the configured size.
    /// </summary>
    /// <param name="stageTag">Short stage tag used in the log file name.</param>
    /// <param name="today">Current local date used for daily log partitioning.</param>
    private void RotateIfNeeded(string stageTag, DateOnly today)
    {
        if (!File.Exists(_currentFilePath)) return;
        if (new FileInfo(_currentFilePath).Length < _config.MaxFileSizeBytes) return;

        CloseWriter();
        _segmentIndex++;
        _currentStageTag = stageTag;
        _currentDate = today;
        OpenSegment();
    }

    /// <summary>
    /// Builds the file path for a log segment.
    /// </summary>
    /// <param name="stageTag">Short stage tag used in the log file name.</param>
    /// <param name="date">Date part of the log partition.</param>
    /// <param name="segment">Segment index within the same date and stage.</param>
    /// <returns>Absolute log file path for the specified segment.</returns>
    private string BuildPath(string stageTag, DateOnly date, int segment)
    {
        var datePart = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var segmentPart = segment == 0 ? string.Empty : $".{segment}";
        var name = $"{_config.LogFileName}.{stageTag}.{datePart}{segmentPart}.md";
        return Path.Combine(_config.LogFilePath, name);
    }

    /// <summary>
    /// Deletes old log files based on the configured retention window.
    /// </summary>
    private void PurgeOldFiles()
    {
        if (_config.MaxAgeDays <= 0) return;

        var cutoff = DateTime.Now.AddDays(-_config.MaxAgeDays);
        try
        {
            foreach (var file in Directory.EnumerateFiles(_config.LogFilePath, "*.md"))
            {
                if (File.GetLastWriteTime(file) < cutoff)
                    File.Delete(file);
            }
        }
        catch { /* Best-effort purge; do not disrupt the run. */ }
    }

    /// <summary>
    /// Flushes and closes the current writer instance.
    /// </summary>
    private void CloseWriter()
    {
        _writer?.Flush();
        _writer?.Dispose();
        _writer = null;
    }

    /// <summary>
    /// Flushes and closes the underlying log file.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        lock (_lock)
        {
            CloseWriter();
        }
        _disposed = true;
    }

    /// <summary>
    /// Parses a textual log level into a <see cref="FIRELogLevel"/> value.
    /// </summary>
    /// <param name="levelName">Configured level token.</param>
    /// <returns>The parsed log level. Defaults to <see cref="FIRELogLevel.Error"/> when unknown.</returns>
    internal static FIRELogLevel ParseLevel(string levelName) => levelName.Trim().ToUpperInvariant() switch
    {
        "DEBUG"    => FIRELogLevel.Debug,
        "INFO"     => FIRELogLevel.Info,
        "WARNING"  => FIRELogLevel.Warning,
        "ERROR"    => FIRELogLevel.Error,
        "CRITICAL" => FIRELogLevel.Critical,
        _          => FIRELogLevel.Error,
    };

    /// <summary>
    /// Maps catalog progress severity to logger severity.
    /// </summary>
    /// <param name="level">Catalog message level.</param>
    /// <returns>Equivalent logger level.</returns>
    internal static FIRELogLevel FromMessageLevel(FIRECatalogMessageLevel level) => level switch
    {
        FIRECatalogMessageLevel.Trace   => FIRELogLevel.Debug,
        FIRECatalogMessageLevel.Info    => FIRELogLevel.Info,
        FIRECatalogMessageLevel.Warning => FIRELogLevel.Warning,
        FIRECatalogMessageLevel.Error   => FIRELogLevel.Error,
        _                               => FIRELogLevel.Info,
    };

    /// <summary>
    /// Resolves the short stage tag used in log file names.
    /// </summary>
    /// <param name="stage">Catalog stage.</param>
    /// <returns>Stage tag token used in log file naming.</returns>
    internal static string StageTag(FIRECatalogStage? stage) => stage switch
    {
        FIRECatalogStage.Collect  => "col",
        FIRECatalogStage.Generate => "gen",
        FIRECatalogStage.Execute  => "exe",
        FIRECatalogStage.Inspect  => "ins",
        _                         => "misc",
    };
}
