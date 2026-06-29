/********************************************************************************
 * MIT License
 *
 * Copyright (c) 2025 Thoms Stoll
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

using FIRE.Localization;
using FIRE.Logging;
using Microsoft.Win32.SafeHandles;
using SharpExifTool;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Tokens;

namespace FIRE;

enum FileInfoByHandleClass
{
    FileBasicInfo = 0,
    FileStandardInfo = 1,
    FileNameInfo = 2,
    FileRenameInfo = 3,
    FileDispositionInfo = 4,
    FileAllocationInfo = 5,
    FileEndOfFileInfo = 6,
    FileStreamInfo = 7,
    FileCompressionInfo = 8,
    FileAttributeTagInfo = 9,
    FileIdBothDirectoryInfo = 10,
    FileIdBothDirectoryRestartInfo = 11,
    FileIoPriorityHintInfo = 12,
    FileRemoteProtocolInfo = 13,
    FileFullDirectoryInfo = 14,
    FileFullDirectoryRestartInfo = 15,
    FileStorageInfo = 16,
    FileAlignmentInfo = 17,
    FileIdInfo = 18,
    FileIdExtdDirectoryInfo = 19,
    FileIdExtdDirectoryRestartInfo = 20,
    MaximumFileInfoByHandleClass
}

/// <summary>
/// Defines an abstraction for metadata extraction from files.
/// 
/// <c>IMetadataSource</c> establishes a contract for extracting key-value metadata from files.
/// Implementations provide different metadata origins, such as file system properties or
/// embedded EXIF/IPTC/XMP data. Multiple sources can be registered in <see cref="MetadataSourceRegistry"/>
/// and used during the collection phase.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Metadata Sources:</strong>
/// Different implementations provide different metadata origins:
/// <list type="bullet">
/// <item><description><see cref="FileInfoMetadataSource"/>: File system properties (CreationTime, ModificationTime, AccessTime)</description></item>
/// <item><description><see cref="ExifToolMetadataSource"/>: Embedded file metadata via ExifTool (EXIF, IPTC, XMP)</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Error Handling:</strong>
/// Implementations should gracefully handle missing files or extraction failures by
/// returning an empty or partial metadata dictionary. Exceptions should be caught
/// to prevent disrupting the collection pipeline.
/// </para>
/// </remarks>
public interface IMetadataSource
{
    /// <summary>
    /// Extracts all available metadata key-value pairs from the specified file.
    /// </summary>
    /// <param name="filePath">Absolute path to the file from which to extract metadata.</param>
    /// <returns>
    /// A dictionary of metadata key-value pairs. Keys are case-insensitive tag/property names,
    /// values are string representations of the metadata. Returns an empty dictionary if
    /// the file does not exist or extraction fails.
    /// </returns>
    /// <remarks>
    /// This method is called during the collection phase for each discovered file.
    /// Implementations should handle missing files and extraction errors gracefully.
    /// </remarks>
    Dictionary<string, string> ExtractMetadata(string filePath);

    /// <summary>
    /// Gets the name of the metadata source.
    /// </summary>
    /// <remarks>
    /// This name is used to register and look up the source in <see cref="MetadataSourceRegistry"/>.
    /// Standard names include "FILEINFO" and "EXIFTOOL".
    /// </remarks>
    string SourceName { get; }
}

/// <summary>
/// Metadata source that extracts file system properties (timestamps).
/// 
/// <c>FileInfoMetadataSource</c> reads file system metadata from the Windows file system,
/// including file creation time, modification time, and last access time. It is a lightweight,
/// built-in metadata source that does not require external utilities.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Extracted Metadata:</strong>
/// This source extracts the following metadata entries:
/// <list type="bullet">
/// <item><description><c>CreationTime</c>: File creation timestamp (format: yyyy-MM-dd HH:mm:ss)</description></item>
/// <item><description><c>ModificationTime</c>: File last modified timestamp</description></item>
/// <item><description><c>AccessTime</c>: File last accessed timestamp</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Error Handling:</strong>
/// If a file does not exist or if metadata extraction fails for any reason, the source
/// returns an empty metadata dictionary. Exceptions are caught to prevent disrupting
/// the collection pipeline.
/// </para>
/// 
/// <para>
/// <strong>Registration:</strong>
/// This source is automatically registered when a <see cref="MetadataSourceRegistry"/> is created.
/// It is typically referenced with the source name "FILEINFO" in configuration files.
/// </para>
/// </remarks>
public sealed class FileInfoMetadataSource : IMetadataSource
{
    /// <summary>
    /// Gets the name of this metadata source.
    /// </summary>
    /// <remarks>This always returns "FILEINFO".</remarks>
    public string SourceName => "FILEINFO";

    /// <summary>
    /// Extracts file system metadata (timestamps) from the specified file.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <returns>
    /// A dictionary containing CreationTime, ModificationTime, and AccessTime entries,
    /// or an empty dictionary if the file does not exist or extraction fails.
    /// </returns>
    public Dictionary<string, string> ExtractMetadata(string filePath)
    {
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists) return metadata;

            metadata["CreationTime"] = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            metadata["ModificationTime"] = fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            metadata["AccessTime"] = fileInfo.LastAccessTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
        catch { }
        return metadata;
    }
}

/// <summary>
/// Metadata source for extracting embedded file metadata using ExifTool.
/// 
/// <c>ExifToolMetadataSource</c> integrates SharpExifTool to provide access to all embedded metadata
/// in files (EXIF, IPTC, XMP, and other formats). It supports a wide range of file types and
/// extracts comprehensive metadata that is typically used for photo/video sorting and management.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Extracted Metadata:</strong>
/// This source can extract metadata such as:
/// <list type="bullet">
/// <item><description>Camera make, model, and lens</description></item>
/// <item><description>Capture date/time (DateTimeOriginal, DateTimeDigitized, etc.)</description></item>
/// <item><description>Image dimensions, orientation, color space</description></item>
/// <item><description>GPS coordinates and location data</description></item>
/// <item><description>XMP comments, keywords, ratings</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Async/Await Integration:</strong>
/// SharpExifTool uses async APIs, but this implementation synchronously blocks using
/// <c>GetAwaiter().GetResult()</c> to fit the synchronous interface contract.
/// </para>
/// 
/// <para>
/// <strong>Error Handling:</strong>
/// If the file does not exist, ExifTool is unavailable, or metadata extraction fails,
/// the source returns an empty or partial metadata dictionary. Exceptions are caught
/// to prevent disrupting the collection pipeline.
/// </para>
/// 
/// <para>
/// <strong>Resource Management:</strong>
/// This class implements <see cref="IDisposable"/> to clean up the SharpExifTool instance.
/// Always dispose of this source when done to release system resources.
/// </para>
/// 
/// <para>
/// <strong>Registration:</strong>
/// This source is automatically registered when a <see cref="MetadataSourceRegistry"/> is created.
/// It is referenced with the source name "EXIFTOOL" in configuration files.
/// </para>
/// </remarks>
/// <example>
/// Using ExifToolMetadataSource directly:
/// <code>
/// using (var exifSource = new ExifToolMetadataSource())
/// {
///     var metadata = exifSource.ExtractMetadata("photo.jpg");
///     foreach (var kvp in metadata)
///     {
///         Console.WriteLine($"{kvp.Key}: {kvp.Value}");
///     }
/// }
/// </code>
/// </example>
public sealed class ExifToolMetadataSource : IMetadataSource, IDisposable
{
    private readonly ExifTool _exifTool;
    private bool _disposed;

    /// <summary>
    /// Gets the name of this metadata source.
    /// </summary>
    /// <remarks>This always returns "EXIFTOOL".</remarks>
    public string SourceName => "EXIFTOOL";

    /// <summary>
    /// Initializes a new instance of <see cref="ExifToolMetadataSource"/>.
    /// </summary>
    /// <remarks>
    /// This creates a new <see cref="SharpExifTool.ExifTool"/> instance that is used
    /// for all subsequent metadata extractions. The instance is disposed when this source is disposed.
    /// </remarks>
    public ExifToolMetadataSource()
    {
        _exifTool = new ExifTool();
    }

    /// <summary>
    /// Extracts all available metadata from the specified file using ExifTool.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <returns>
    /// A dictionary containing all extracted metadata key-value pairs, or an empty dictionary
    /// if the file does not exist, ExifTool is unavailable, or extraction fails.
    /// </returns>
    /// <remarks>
    /// ExifTool is called synchronously via <c>GetAwaiter().GetResult()</c> to support
    /// the synchronous interface. This may block briefly while ExifTool processes the file.
    /// </remarks>
    public Dictionary<string, string> ExtractMetadata(string filePath)
    {
        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            if (!File.Exists(filePath))
            {
                return metadata;
            }

            // Execute ExifTool synchronously and get all metadata
            var result = _exifTool.ExtractAllMetadataAsync(filePath).GetAwaiter().GetResult();

            if (result != null)
            {
                foreach (var kvp in result)
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Key) && kvp.Value != null)
                    {
                        metadata[kvp.Key] = kvp.Value.ToString() ?? string.Empty;
                    }
                }
            }
        }
        catch
        {
            // Metadata extraction failed; return partial or empty result.
        }

        return metadata;
    }

    /// <summary>
    /// Disposes the underlying ExifTool instance.
    /// </summary>
    /// <remarks>
    /// This method is safe to call multiple times. After calling this method,
    /// further calls to <see cref="ExtractMetadata"/> will fail silently or throw an exception.
    /// </remarks>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _exifTool?.Dispose();
        _disposed = true;
    }
}

/// <summary>
/// Registry for registered metadata sources.
/// 
/// <c>MetadataSourceRegistry</c> maintains a collection of available <see cref="IMetadataSource"/> implementations
/// and provides lookup by source name. It is used during the collection phase to retrieve the appropriate
/// metadata extractor for each configured keyword.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Default Sources:</strong>
/// When created, the registry automatically registers two built-in sources:
/// <list type="bullet">
/// <item><description><see cref="FileInfoMetadataSource"/> as "FILEINFO"</description></item>
/// <item><description><see cref="ExifToolMetadataSource"/> as "EXIFTOOL"</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Custom Sources:</strong>
/// Additional sources can be registered via <see cref="Register"/>, allowing custom metadata
/// extractors for proprietary or specialized file formats.
/// </para>
/// 
/// <para>
/// <strong>Case-Insensitive Lookup:</strong>
/// Source names are stored and looked up case-insensitively, so "exiftool", "EXIFTOOL", and
/// "ExifTool" all refer to the same source.
/// </para>
/// </remarks>
/// <example>
/// Looking up a metadata source:
/// <code>
/// var registry = new MetadataSourceRegistry();
/// var source = registry.GetSource("EXIFTOOL");
/// if (source != null)
/// {
///     var metadata = source.ExtractMetadata("photo.jpg");
/// }
/// </code>
/// </example>
public sealed class MetadataSourceRegistry
{
    private readonly Dictionary<string, IMetadataSource> _sources = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of <see cref="MetadataSourceRegistry"/>.
    /// </summary>
    /// <remarks>
    /// The constructor automatically registers the <see cref="FileInfoMetadataSource"/>
    /// and <see cref="ExifToolMetadataSource"/> instances, making them immediately available
    /// for use in metadata collection.
    /// </remarks>
    public MetadataSourceRegistry()
    {
        Register(new FileInfoMetadataSource());
        Register(new ExifToolMetadataSource());
    }

    /// <summary>
    /// Registers or replaces a metadata source in the registry.
    /// </summary>
    /// <param name="source">The metadata source to register.</param>
    /// <remarks>
    /// If a source with the same name (case-insensitive) is already registered,
    /// it is replaced. The source name is obtained from <see cref="IMetadataSource.SourceName"/>.
    /// </remarks>
    public void Register(IMetadataSource source) => _sources[source.SourceName] = source;

    /// <summary>
    /// Retrieves a registered metadata source by name.
    /// </summary>
    /// <param name="sourceName">The name of the source (case-insensitive).</param>
    /// <returns>
    /// The registered <see cref="IMetadataSource"/> with the specified name,
    /// or null if no source with that name is registered.
    /// </returns>
    public IMetadataSource? GetSource(string sourceName)
    {
        _sources.TryGetValue(sourceName, out var source);
        return source;
    }
}

/// <summary>
/// Orchestrates the three-step FIRE processing pipeline: collect, generate, and execute.
/// 
/// <c>FIRECatalog</c> is the main orchestrator that drives metadata collection from files,
/// generates target directory and file names using path templates, and executes file operations
/// (Copy, Move, or Link). It integrates the configuration, database, and metadata sources
/// to implement the complete file processing workflow.
/// </summary>
/// <remarks>
/// <para>
/// <strong>Three-Step Workflow:</strong>
/// <list type="number">
/// <item>
///   <description><strong>Collect:</strong>
///   <see cref="CollectFiles"/> recursively scans all configured root directories,
///   creates file records for each file, and populates metadata from configured sources.</description>
/// </item>
/// <item>
///   <description><strong>Generate:</strong>
///   <see cref="GenerateTargetPaths"/> computes output directory and file name for each record
///   by parsing path templates with resolved metadata values.</description>
/// </item>
/// <item>
///   <description><strong>Execute:</strong>
///   <see cref="ExecuteFileOperations"/> performs the configured file operation
///   (Copy, Move, or Link) from source to target path.</description>
/// </item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Windows File Identification:</strong>
/// <c>FIRECatalog</c> uses Windows API calls (GetFileInformationByHandleEx) to retrieve
/// volume serial numbers and file IDs, which enable detection of hard links and files
/// on the same physical volume. This is done on Windows/NTFS systems only.
/// </para>
/// 
/// <para>
/// <strong>Extensibility:</strong>
/// The metadata extraction system is extensible via <see cref="MetadataSourceRegistry"/>.
/// Custom metadata sources can be registered to support additional file formats or sources.
/// </para>
/// 
/// <para>
/// <strong>Resource Management:</strong>
/// This class implements <see cref="IDisposable"/> to properly clean up the database
/// and metadata sources. Always dispose of this instance when done.
/// </para>
/// </remarks>
/// <example>
/// Using FIRECatalog for the complete pipeline:
/// <code>
/// var config = FIREConfigration.Load("Configuration.yaml");
/// config.EnsureSupportedConfigurationVersion();
/// 
/// var dbPath = Path.Combine(config.DataBasePath, config.DataBaseFileName);
/// using var database = new FIREDatabase(dbPath);
/// using (var catalog = new FIRECatalog(config, database))
/// {
///     // Step 1: Collect metadata
///     catalog.CollectFiles(path => Console.WriteLine($"Collecting: {path}"));
///     
///     // Step 2: Generate target paths
///     catalog.GenerateTargetPaths(path => Console.WriteLine($"Generating: {path}"));
///     
///     // Step 3: Execute file operations
///     catalog.ExecuteFileOperations(path => Console.WriteLine($"Processing: {path}"));
/// }
/// </code>
/// </example>
public sealed class FIRECatalog : IDisposable
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetFileInformationByHandleEx(SafeFileHandle hFile, FileInfoByHandleClass fileInfoClass, out FILE_ID_INFO fileIdInfo, uint dwBufferSize);

    private readonly FIREConfigration _configuration;
    private readonly FIREDatabase _database;
    private readonly MetadataSourceRegistry _metadataRegistry;
    private readonly List<string> _lastCollectedSourcePaths = [];
    private readonly FIRELogger? _logger;
    private bool _disposed;

    // Collect-session statistics (reset at BeginStage for Collect)
    private int _collectTotalFiles;
    private int _collectAddedFiles;
    private readonly Dictionary<string, int> _collectSkippedExtensions = new(StringComparer.OrdinalIgnoreCase);

    public event EventHandler<FIRECatalogProgressEventArgs>? ProgressChanged;

    public CultureInfo Culture { get; set; } = CultureInfo.GetCultureInfo("en-US");

    public FIRECatalogStage? CurrentStage { get; private set; }

    public string? CurrentFilePath { get; private set; }

    public int ProcessedFileCount { get; private set; }

    public int TotalFileCount { get; private set; }

    public IReadOnlyList<string> LastCollectedSourcePaths => _lastCollectedSourcePaths;

    /// <summary>
    /// Marshalled structure for retrieving file ID information from Windows NTFS.
    /// </summary>
    /// <remarks>
    /// This structure receives output from the Windows API call GetFileInformationByHandleEx
    /// with FileInfoByHandleClass.FileIdInfo, and contains the volume serial number and file ID.
    /// </remarks>
    public struct FILE_ID_INFO
    {
        /// <summary>
        /// The volume serial number where the file resides.
        /// </summary>
        public ulong VolumeSerialNumber;

        /// <summary>
        /// The 16-byte file identifier, unique per file on the NTFS volume.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] FileId;
    }

    struct BY_HANDLE_FILE_INFORMATION
    {
        public uint FileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
        public uint VolumeSerialNumber;
        public uint FileSizeHigh;
        public uint FileSizeLow;
        public uint NumberOfLinks;
        public uint FileIndexHigh;
        public uint FileIndexLow;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="FIRECatalog"/>.
    /// </summary>
    /// <param name="configuration">The FIRE configuration containing paths, patterns, and keyword definitions.</param>
    /// <param name="database">The database instance for persisting file records and metadata.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="configuration"/> or <paramref name="database"/> is null.
    /// </exception>
    public FIRECatalog(FIREConfigration configuration, FIREDatabase database)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(database);
        _configuration = configuration;
        _database = database;
        _metadataRegistry = new MetadataSourceRegistry();

        if (configuration.Logging != null)
        {
            _logger = new FIRELogger(configuration.Logging);
        }
    }

    private void BeginStage(FIRECatalogStage stage, int totalCount)
    {
        CurrentStage = stage;
        CurrentFilePath = null;
        ProcessedFileCount = 0;
        TotalFileCount = Math.Max(totalCount, 0);

        if (stage == FIRECatalogStage.Collect)
        {
            _lastCollectedSourcePaths.Clear();
            _collectTotalFiles = 0;
            _collectAddedFiles = 0;
            _collectSkippedExtensions.Clear();
        }

        EmitProgress(FIRECatalogMessageLevel.Info, "status.started");
    }

    private void CompleteCurrentStage()
    {
        LogCollectStatistics();
        EmitProgress(FIRECatalogMessageLevel.Info, "status.completed");
    }

    private void ReportFileProgress(string filePath)
    {
        CurrentFilePath = filePath;
        ProcessedFileCount++;

        if (CurrentStage == FIRECatalogStage.Collect)
        {
            _lastCollectedSourcePaths.Add(filePath);
        }

        EmitProgress(FIRECatalogMessageLevel.Trace, "status.file_processing", filePath);
    }

    private void EmitProgress(FIRECatalogMessageLevel level, string messageKey, params object[] messageArgs)
    {
        var stage = CurrentStage ?? FIRECatalogStage.Collect;
        var stageKey = stage switch
        {
            FIRECatalogStage.Collect => "stage.collect",
            FIRECatalogStage.Generate => "stage.generate",
            FIRECatalogStage.Execute => "stage.execute",
            FIRECatalogStage.Inspect => "stage.inspect",
            _ => "stage.collect"
        };

        var args = messageArgs.Length == 0 ? [ApiLocalizer.Get(stageKey, Culture)] : messageArgs;
        var message = ApiLocalizer.Format(messageKey, Culture, args);

        var eventArgs = new FIRECatalogProgressEventArgs
        {
            Stage = stage,
            Level = level,
            Message = message,
            MessageKey = messageKey,
            CurrentFilePath = CurrentFilePath,
            ProcessedCount = ProcessedFileCount,
            TotalCount = TotalFileCount,
            Culture = Culture
        };

        ProgressChanged?.Invoke(this, eventArgs);
        _logger?.Log(FIRELogger.FromMessageLevel(level), FIRELogger.StageTag(stage), message);
    }

    /// <summary>
    /// Retrieves file identifier information from the NTFS file system for the specified file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses the Windows API function GetFileInformationByHandleEx to obtain
    /// the file identifier and volume serial number from NTFS. These IDs uniquely identify
    /// a file on the volume and persist even if the file is renamed or moved.
    /// </para>
    /// 
    /// <para>
    /// The method opens the file for reading, queries the file ID info, and closes the file.
    /// This is a synchronous, blocking operation. On non-NTFS or non-Windows systems,
    /// the returned structure may contain zeros.
    /// </para>
    /// </remarks>
    /// <param name="filePath">The absolute path to the file. Must exist and be readable.</param>
    /// <returns>
    /// A <see cref="FILE_ID_INFO"/> structure containing the volume serial number and file ID.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the file does not exist or cannot be opened.
    /// </exception>
    private FILE_ID_INFO GetFileIdInfo(string filePath)
    {
        FILE_ID_INFO fileIdInfo = new FILE_ID_INFO();
        FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        // Call the Windows API function to get the file identifier information
        GetFileInformationByHandleEx(fileStream.SafeFileHandle, FileInfoByHandleClass.FileIdInfo, out fileIdInfo, (uint)Marshal.SizeOf<FILE_ID_INFO>());

        fileStream.Close();
        return fileIdInfo;
    }

    /// <summary>
    /// Step 1: Collects all files and their metadata from configured root directories.
    /// </summary>
    /// <param name="progressCallback">
    /// Optional callback invoked for each file being processed. The callback receives the file path.
    /// Use this to report progress in a UI or log file.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method:
    /// <list type="number">
    /// <item>Clears any existing records from the database.</item>
    /// <item>Recursively scans all paths in <see cref="FIREConfigration.FilesRootPath"/>.</item>
    /// <item>For each file matching a configured file extension:
    ///   <list><item>Retrieves file ID information from the OS.</item>
    ///   <item>Queries metadata from all configured sources and keywords.</item>
    ///   <item>Creates a <see cref="FIREDbRecord"/> and adds it to the database.</item></list>
    /// </item>
    /// <item>Saves all records to the database when complete.</item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// Errors during file access or metadata extraction are logged but do not stop the pipeline.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    public void CollectFiles(Action<string>? progressCallback = null)
    {
        ThrowIfDisposed();

        var totalFiles = _configuration.FilesRootPath
            .Where(Directory.Exists)
            .Sum(root => Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Count());

        BeginStage(FIRECatalogStage.Collect, totalFiles);

        foreach (var rootPath in _configuration.FilesRootPath)
        {
            if (!Directory.Exists(rootPath)) continue;
            CollectFromDirectory(rootPath, progressCallback);
        }

        CompleteCurrentStage();
    }

    /// <summary>
    /// Clears all records from the database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should be called explicitly when a fresh collection pass is required.
    /// The default <see cref="CollectFiles"/> workflow now supports incremental collection,
    /// preserving existing records and only adding new files.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    public void ClearDatabase()
    {
        ThrowIfDisposed();
        _database.Clear();
    }

    /// <summary>
    /// Step 2: Generates target file paths for all collected file records.
    /// </summary>
    /// <param name="progressCallback">
    /// Optional callback invoked for each file being processed. The callback receives the file path.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method iterates over all records in the database and computes the target directory
    /// and file name by:
    /// <list type="number">
    /// <item>Looking up the file extension configuration.</item>
    /// <item>Retrieving the sorting and file name patterns (using extension-specific overrides if available).</item>
    /// <item>Resolving metadata placeholders in the patterns using <see cref="ParseTemplate"/>.</item>
    /// <item>Applying global string replacements from the configuration.</item>
    /// <item>Constructing the final target path.</item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// The target path is set in the <see cref="FIREDbRecord.TargetFilePath"/> property.
    /// If the patterns or resolution fails, the target path remains null.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    public void GenerateTargetPaths(Action<string>? progressCallback = null)
    {
        ThrowIfDisposed();

        _database.SortRecords(_configuration.FileSorting, _configuration.FileSortingOrder);

        var pendingCount = _database.Count(record =>
            !string.IsNullOrWhiteSpace(record.SourceFilePath) &&
            record.Status is not (ProcessingStatus.PathGenerated or ProcessingStatus.Executed));

        BeginStage(FIRECatalogStage.Generate, pendingCount);

        foreach (var record in _database)
        {
            if (string.IsNullOrWhiteSpace(record.SourceFilePath)) continue;

            // Skip files that already have a generated path or have been executed.
            if (record.Status is ProcessingStatus.PathGenerated or ProcessingStatus.Executed) continue;

            progressCallback?.Invoke(record.SourceFilePath);
            ReportFileProgress(record.SourceFilePath);

            // Handle sidecar files separately
            if (record.Classification == FileClassification.SidecarFile)
            {
                GenerateSidecarTargetPath(record);
                // Update status for sidecars
                if (!string.IsNullOrWhiteSpace(record.TargetFilePath))
                    record.Status = ProcessingStatus.PathGenerated;
                continue;
            }

            var extension = Path.GetExtension(record.SourceFilePath).ToLowerInvariant();
            if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig)) continue;

            var metadataLookup = record.FileMetaDatas.ToDictionary(
                m => m.Key ?? string.Empty,
                m => m.Value ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);

            var sortingPattern = extConfig.SortingPatern ?? _configuration.SortingPatern ?? string.Empty;
            var fileNamePattern = extConfig.FileNamePatern ?? _configuration.FileNamePatern ?? string.Empty;

            var targetDirectory = ParseTemplate(sortingPattern, metadataLookup, record.SourceFilePath);
            if (string.IsNullOrWhiteSpace(targetDirectory)) continue;

            long? counter = null;
            if (ContainsCounterPlaceholder(fileNamePattern))
                counter = _database.GetNextCounterValue(targetDirectory);

            var targetFileName = ParseTemplate(fileNamePattern, metadataLookup, record.SourceFilePath, counter);

            if (!string.IsNullOrWhiteSpace(targetFileName))
            {
                record.TargetFilePath = Path.Combine(targetDirectory, targetFileName);
                // Update status after successful path generation
                record.Status = ProcessingStatus.PathGenerated;
            }
        }

        _database.Save();
        CompleteCurrentStage();
    }

    /// <summary>
    /// Generates the target file path for a sidecar file.
    /// </summary>
    /// <param name="sidecarRecord">Database record of the sidecar file.</param>
    /// <remarks>
    /// <para>
    /// Sidecar files inherit the target path from their associated primary file.
    /// This method searches for the primary file (same directory, same base name,
    /// different extension) and adopts its target path, retaining only the sidecar's
    /// original extension.
    /// </para>
    /// <para>
    /// Example:
    /// <list type="bullet">
    /// <item><description>Primary: D:\Import\IMG_1234.jpg → D:\Sorted\2026\07\Apple\IMG_1234.jpg</description></item>
    /// <item><description>Sidecar: D:\Import\IMG_1234.xmp → D:\Sorted\2026\07\Apple\IMG_1234.xmp</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    private void GenerateSidecarTargetPath(FIREDbRecord sidecarRecord)
    {
        if (string.IsNullOrWhiteSpace(sidecarRecord.SourceFilePath))
            return;

        var sidecarDirectory = Path.GetDirectoryName(sidecarRecord.SourceFilePath);
        var sidecarBaseName = Path.GetFileNameWithoutExtension(sidecarRecord.SourceFilePath);
        var sidecarExtension = Path.GetExtension(sidecarRecord.SourceFilePath);

        if (string.IsNullOrWhiteSpace(sidecarDirectory) || string.IsNullOrWhiteSpace(sidecarBaseName))
            return;

        // Find the primary file in the same directory with the same base name
        var primaryRecord = _database.FirstOrDefault(r =>
            r.Classification == FileClassification.RegularFile &&
            !string.IsNullOrWhiteSpace(r.SourceFilePath) &&
            string.Equals(Path.GetDirectoryName(r.SourceFilePath), sidecarDirectory, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(Path.GetFileNameWithoutExtension(r.SourceFilePath), sidecarBaseName, StringComparison.OrdinalIgnoreCase));

        if (primaryRecord == null || string.IsNullOrWhiteSpace(primaryRecord.TargetFilePath))
        {
            LogMetadataWarning(sidecarRecord.SourceFilePath, "Sidecar", "could not find primary file or primary file has no target path");
            return;
        }

        // Inherit target directory and base name from primary, keep sidecar extension
        var primaryTargetDirectory = Path.GetDirectoryName(primaryRecord.TargetFilePath);
        var primaryTargetBaseName = Path.GetFileNameWithoutExtension(primaryRecord.TargetFilePath);

        if (!string.IsNullOrWhiteSpace(primaryTargetDirectory) && !string.IsNullOrWhiteSpace(primaryTargetBaseName))
        {
            sidecarRecord.TargetFilePath = Path.Combine(primaryTargetDirectory, primaryTargetBaseName + sidecarExtension);
        }
    }

    /// <summary>
    /// Step 3: Executes the configured file operation (Copy, Move, or Link) for all records.
    /// </summary>
    /// <param name="progressCallback">
    /// Optional callback invoked for each file being processed. The callback receives the file path.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method iterates over all records and performs the configured file operation:
    /// <list type="bullet">
    /// <item>Skips records with null or invalid paths or files that no longer exist.</item>
    /// <item>Looks up the file extension configuration.</item>
    /// <item>Retrieves the action (Copy, Move, or Link) using extension-specific overrides if available.</item>
    /// <item>Calls <see cref="ExecuteAction"/> to perform the operation.</item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// The operation creates all necessary output directories and handles errors gracefully.
    /// All changes are persisted to the database when complete.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    public void ExecuteFileOperations(Action<string>? progressCallback = null)
    {
        ThrowIfDisposed();

        var pendingCount = _database.Count(record =>
            !string.IsNullOrWhiteSpace(record.SourceFilePath) &&
            !string.IsNullOrWhiteSpace(record.TargetFilePath) &&
            record.Status != ProcessingStatus.Executed &&
            File.Exists(record.SourceFilePath));

        BeginStage(FIRECatalogStage.Execute, pendingCount);

        foreach (var record in _database)
        {
            if (string.IsNullOrWhiteSpace(record.SourceFilePath) || string.IsNullOrWhiteSpace(record.TargetFilePath)) continue;
            if (!File.Exists(record.SourceFilePath)) continue;

            // Skip files that have already been executed (incremental workflow)
            if (record.Status == ProcessingStatus.Executed) continue;

            progressCallback?.Invoke(record.SourceFilePath);
            ReportFileProgress(record.SourceFilePath);

            string action;

            // Sidecar files inherit the action from their primary file
            if (record.Classification == FileClassification.SidecarFile)
            {
                // Use global default action for sidecars
                action = _configuration.Action ?? "Copy";
            }
            else
            {
                // Regular files use their extension-specific configuration
                var extension = Path.GetExtension(record.SourceFilePath).ToLowerInvariant();
                if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig)) continue;
                action = extConfig.Action ?? _configuration.Action ?? "Copy";
            }

            ExecuteAction(action, record.SourceFilePath, record.TargetFilePath);

            // Mark as executed after successful operation
            record.Status = ProcessingStatus.Executed;
        }

        _database.Save();
        CompleteCurrentStage();
    }

    /// <summary>
    /// Writes collect-session statistics to the log at level Debug.
    /// </summary>
    /// <remarks>
    /// Only emits entries when the current or most-recent stage is <see cref="FIRECatalogStage.Collect"/>
    /// and logging is enabled. Called both on normal completion and on user cancellation.
    /// </remarks>
    private void LogCollectStatistics()
    {
        if (_logger == null) return;
        if (CurrentStage != FIRECatalogStage.Collect) return;

        var tag = FIRELogger.StageTag(FIRECatalogStage.Collect);

        var msgTotal = ApiLocalizer.Format("stats.collect.total", Culture, _collectTotalFiles);
        _logger.Log(FIRELogLevel.Debug, tag, msgTotal);

        var msgAdded = ApiLocalizer.Format("stats.collect.added", Culture, _collectAddedFiles);
        _logger.Log(FIRELogLevel.Debug, tag, msgAdded);

        if (_collectSkippedExtensions.Count > 0)
        {
            var extList = string.Join(", ", _collectSkippedExtensions
                .OrderByDescending(kv => kv.Value)
                .Select(kv => $"{kv.Key} ({kv.Value}x)"));
            var msgSkipped = ApiLocalizer.Format("stats.collect.skipped_extensions", Culture, extList);
            _logger.Log(FIRELogLevel.Debug, tag, msgSkipped);
        }
    }

    /// <summary>
    /// Writes a cancellation notice to the log at level Info.
    /// </summary>
    /// <remarks>
    /// Call this method before disposing the catalog when the operation was cancelled
    /// by the user (e.g. via Ctrl+C). The entry is written directly to the logger without
    /// raising <see cref="ProgressChanged"/>; if logging is disabled the call is a no-op.
    /// </remarks>
    public void LogCancelled()
    {
        LogCollectStatistics();
        var message = ApiLocalizer.Get("status.cancelled", Culture);
        _logger?.Log(FIRELogLevel.Info, FIRELogger.StageTag(CurrentStage), message);
    }

    /// <summary>
    /// Disposes the database and metadata sources held by this instance.
    /// </summary>
    /// <remarks>
    /// This method is safe to call multiple times. After calling this method, all other
    /// methods will throw <see cref="ObjectDisposedException"/>.
    /// </remarks>
    public void Dispose()
    {
        if (_disposed) return;
        _logger?.Dispose();
        _database.Dispose();
        _disposed = true;
    }

    /// <summary>
    /// Scans a directory recursively and processes every discovered file.
    /// </summary>
    /// <param name="directoryPath">Root directory to scan recursively.</param>
    /// <param name="progressCallback">Optional callback receiving each discovered file path.</param>
    private void CollectFromDirectory(string directoryPath, Action<string>? progressCallback = null)
    {
        try
        {
            foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                _collectTotalFiles++;
                progressCallback?.Invoke(filePath);
                ReportFileProgress(filePath);
                ProcessFile(filePath);
            }
        }
        catch { }
    }

    /// <summary>
    /// Processes a single source file and stores extracted metadata in the database.
    /// </summary>
    /// <param name="filePath">Absolute path of the file to process.</param>
    private void ProcessFile(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig))
        {
            if (CurrentStage == FIRECatalogStage.Collect)
            {
                var key = string.IsNullOrEmpty(extension) ? "(no extension)" : extension;
                _collectSkippedExtensions.TryGetValue(key, out var count);
                _collectSkippedExtensions[key] = count + 1;
            }
            return;
        }

        var fileIdInfo = GetFileIdInfo(filePath);

        // Skip files that are already in the database (incremental workflow)
        if (_database.FileExists(fileIdInfo.VolumeSerialNumber, fileIdInfo.FileId))
            return;

        var record = new FIREDbRecord
        {
            SourceFilePath = filePath,
            VolumeSerialNumber = fileIdInfo.VolumeSerialNumber,
            FileId = fileIdInfo.FileId
        };

        foreach (var keywordEntry in extConfig.AvailableKeyWords)
        {
            var keywordName = keywordEntry.Key;
            var keywordConfig = keywordEntry.Value;
            var sourceName = keywordConfig.Source ?? "FILEINFO";
            var valAttribute = keywordConfig.ValAttribute ?? "LOWEST";

            var source = _metadataRegistry.GetSource(sourceName);
            if (source == null)
            {
                LogMetadataWarning(filePath, keywordName, $"metadata source '{sourceName}' was not found");
                var fallbackValue = ResolveMissingKeywordValue(keywordConfig, filePath, keywordName);
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, fallbackValue, keywordConfig, sourceName));
                continue;
            }

            if (keywordConfig.KeyWords.Count == 0)
            {
                LogMetadataWarning(filePath, keywordName, "no keywords configured");
                var fallbackValue = ResolveMissingKeywordValue(keywordConfig, filePath, keywordName);
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, fallbackValue, keywordConfig, sourceName));
                continue;
            }

            var extractedMetadata = source.ExtractMetadata(filePath);
            var matchingValues = new List<string>();

            foreach (var keyword in keywordConfig.KeyWords)
            {
                if (extractedMetadata.TryGetValue(keyword, out var value) && !string.IsNullOrWhiteSpace(value))
                    matchingValues.Add(value);
            }

            if (matchingValues.Count == 0)
            {
                LogMetadataWarning(filePath, keywordName, $"none of the configured keywords were found in source '{sourceName}'");
                var fallbackValue = ResolveMissingKeywordValue(keywordConfig, filePath, keywordName);
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, fallbackValue, keywordConfig, sourceName));
                continue;
            }

            var selectedValue = SelectValue(matchingValues, valAttribute, keywordConfig.DataType, filePath, keywordName);
            record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, selectedValue, keywordConfig, sourceName));
        }

        _database.Add(record);
        if (CurrentStage == FIRECatalogStage.Collect)
            _collectAddedFiles++;
        ProcessSidecarFiles(filePath, extConfig, record);
    }

    /// <summary>
    /// Searches for and processes sidecar files associated with a primary file.
    /// </summary>
    /// <param name="primaryFilePath">Absolute path to the primary file.</param>
    /// <param name="extConfig">File extension configuration for the primary file.</param>
    /// <param name="primaryRecord">Database record of the primary file.</param>
    /// <remarks>
    /// <para>
    /// Sidecar files are auxiliary files (e.g., .xmp, .pp3) that contain metadata
    /// or processing instructions for a primary file. This method:
    /// <list type="number">
    /// <item><description>Checks if sidecar extensions are configured</description></item>
    /// <item><description>Searches for each configured sidecar extension in the same directory</description></item>
    /// <item><description>Creates a database record for each found sidecar</description></item>
    /// <item><description>Copies metadata from the primary file to enable path generation</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Sidecar records are marked with <see cref="FileClassification.SidecarFile"/> so they
    /// can be handled specially during the generate phase (they inherit the target path from
    /// their primary file).
    /// </para>
    /// </remarks>
    private void ProcessSidecarFiles(string primaryFilePath, FileExtensionConfiguration extConfig, FIREDbRecord primaryRecord)
    {
        if (extConfig.SidecarFileExtensions == null || extConfig.SidecarFileExtensions.Count == 0)
            return;

        var directory = Path.GetDirectoryName(primaryFilePath);
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            return;

        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(primaryFilePath);

        foreach (var sidecarExtension in extConfig.SidecarFileExtensions)
        {
            var normalizedExtension = sidecarExtension.StartsWith(".", StringComparison.Ordinal)
                ? sidecarExtension
                : "." + sidecarExtension;

            var sidecarPath = Path.Combine(directory, fileNameWithoutExtension + normalizedExtension);

            if (!File.Exists(sidecarPath))
                continue;

            try
            {
                var fileIdInfo = GetFileIdInfo(sidecarPath);

                // Skip sidecar if already in database (incremental workflow)
                if (_database.FileExists(fileIdInfo.VolumeSerialNumber, fileIdInfo.FileId))
                    continue;

                var sidecarRecord = new FIREDbRecord
                {
                    SourceFilePath = sidecarPath,
                    VolumeSerialNumber = fileIdInfo.VolumeSerialNumber,
                    FileId = fileIdInfo.FileId,
                    Classification = FileClassification.SidecarFile,
                    Status = primaryRecord.Status // Inherit status from primary file
                };

                // Clone metadata from primary file so the sidecar can inherit the target path
                foreach (var metadata in primaryRecord.FileMetaDatas)
                {
                    sidecarRecord.FileMetaDatas.Add(new FIREFileMetaData
                    {
                        Key = metadata.Key,
                        Value = metadata.Value,
                        TypeName = metadata.TypeName,
                        DataSource = metadata.DataSource
                    });
                }

                _database.Add(sidecarRecord);
            }
            catch
            {
                // If sidecar processing fails, continue with next sidecar
                // The primary file is already in the database
            }
        }
    }

    /// <summary>
    /// Creates a normalized metadata entry for persistence.
    /// </summary>
    /// <param name="key">Logical metadata key used in templates.</param>
    /// <param name="value">Selected metadata value.</param>
    /// <param name="keywordConfig">Keyword configuration used for extraction.</param>
    /// <param name="sourceName">Name of the metadata source that provided the value.</param>
    /// <returns>A populated <see cref="FIREFileMetaData"/> instance.</returns>
    private static FIREFileMetaData CreateMetadataEntry(string key, string value, AvailableKeywordConfiguration keywordConfig, string sourceName)
    {
        return new FIREFileMetaData
        {
            Key = key,
            Value = value,
            TypeName = keywordConfig.DataType ?? "STRING",
            DataSource = sourceName
        };
    }

    /// <summary>
    /// Resolves the fallback value for missing keyword data.
    /// </summary>
    /// <param name="keywordConfig">Keyword configuration that may define a default value.</param>
    /// <param name="filePath">Source file path for diagnostics.</param>
    /// <param name="keywordName">Logical keyword name for diagnostics.</param>
    /// <returns>Configured default value or <c>NA</c> when no valid default exists.</returns>
    private string ResolveMissingKeywordValue(AvailableKeywordConfiguration keywordConfig, string filePath, string keywordName)
    {
        var resolved = ResolveKeywordDefaultValue(keywordConfig, DateTime.Now);
        if (!string.Equals(resolved, "NA", StringComparison.Ordinal))
        {
            return resolved;
        }

        if (!string.IsNullOrWhiteSpace(keywordConfig.Default) &&
            string.Equals((keywordConfig.DataType ?? "STRING").Trim(), "DATETIME", StringComparison.OrdinalIgnoreCase))
        {
            LogMetadataWarning(filePath, keywordName, $"configured DATETIME default '{keywordConfig.Default.Trim()}' is invalid; falling back to NA");
        }

        return "NA";
    }

    internal static string ResolveKeywordDefaultValue(AvailableKeywordConfiguration keywordConfig, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(keywordConfig.Default))
        {
            return "NA";
        }

        var configuredDefault = keywordConfig.Default.Trim();
        var dataType = (keywordConfig.DataType ?? "STRING").Trim();

        if (!dataType.Equals("DATETIME", StringComparison.OrdinalIgnoreCase))
        {
            return configuredDefault;
        }

        if (configuredDefault.Equals("NOW", StringComparison.OrdinalIgnoreCase))
        {
            return now.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        if (TryNormalizeDateTime(configuredDefault, out var normalizedDate) &&
            DateTime.TryParseExact(
                normalizedDate,
                "yyyy:MM:dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
        {
            return normalizedDate;
        }

        return "NA";
    }

    /// <summary>
    /// Selects a single metadata value from a candidate set based on configuration.
    /// </summary>
    /// <param name="values">Candidate values extracted for a keyword.</param>
    /// <param name="valAttribute">Selection mode (e.g., LOWEST, HIGHEST).</param>
    /// <param name="dataType">Configured data type used for typed comparison.</param>
    /// <param name="filePath">Source file path for diagnostics.</param>
    /// <param name="keywordName">Logical keyword name for diagnostics.</param>
    /// <returns>The selected metadata value.</returns>
    private string SelectValue(List<string> values, string valAttribute, string dataType, string filePath, string keywordName)
    {
        var selectionMode = valAttribute.Trim().ToUpperInvariant();
        var normalizedDataType = dataType.Trim().ToUpperInvariant();

        // Early exit for single value
        if (values.Count == 1)
        {
            // Normalize DateTime values if applicable
            if (normalizedDataType is "DATETIME" or "DATE" or "TIME" &&
                TryNormalizeDateTime(values[0], out var normalizedDate) &&
                DateTime.TryParseExact(normalizedDate, "yyyy:MM:dd HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
            {
                return dateTimeValue.ToString();
            }
            return values[0];
        }

        // Strongly-typed data types: use SelectExtremeValue
        if (normalizedDataType is "INT" or "INTEGER" or "DATETIME" or "DATE" or "TIME")
        {
            return selectionMode switch
            {
                "HIGHEST" => SelectExtremeValue(values, normalizedDataType, true, filePath, keywordName),
                _ => SelectExtremeValue(values, normalizedDataType, false, filePath, keywordName)
            };
        }

        // String comparison: use MaxBy/MinBy instead of OrderBy
        return selectionMode switch
        {
            "HIGHEST" => values.MaxBy(value => value, StringComparer.OrdinalIgnoreCase) ?? "NA",
            _ => values.MinBy(value => value, StringComparer.OrdinalIgnoreCase) ?? "NA"
        };
    }

    /// <summary>
    /// Selects the minimum or maximum comparable value from typed candidates.
    /// </summary>
    /// <param name="values">Candidate values to evaluate.</param>
    /// <param name="normalizedDataType">Normalized target type (INT, DATETIME, ...).</param>
    /// <param name="highest">True to select maximum; false to select minimum.</param>
    /// <param name="filePath">Source file path for diagnostics.</param>
    /// <param name="keywordName">Logical keyword name for diagnostics.</param>
    /// <returns>The selected extreme value as string.</returns>
    private string SelectExtremeValue(List<string> values, string normalizedDataType, bool highest, string filePath, string keywordName)
    {
        var candidates = new List<(string RawValue, IComparable ComparableValue)>(values.Count);

        foreach (var value in values)
        {
            if (TryConvertComparable(value, normalizedDataType, out var comparableValue))
                candidates.Add((value, comparableValue));
        }

        if (candidates.Count == 0)
        {
            LogMetadataWarning(filePath, keywordName, $"no comparable values found for data type '{normalizedDataType}'");
            return values[0];
        }

        var selected = highest
            ? candidates.MaxBy(x => x.ComparableValue)
            : candidates.MinBy(x => x.ComparableValue);

        return selected.ComparableValue.ToString();
    }
    /// <summary>
    /// Tries to parse a date/time string from various known input formats and
    /// returns it normalized as <c>yyyy:MM:dd HH:mm:ss</c>.
    /// Supported inputs: dd.MM.yyyy HH:mm:ss | yyyy:MM:dd HH:mm:ss | yyyy:MM:dd HH:mm:sszzz | yyyy:MM:dd
    /// </summary>
    private static readonly string[] KnownDateTimeFormats =
    [
        "yyyy:MM:dd HH:mm:ss",
    "yyyy:MM:dd HH:mm:sszzz",
    "yyyy:MM:dd HH:mm:ssz",
    "yyyy:MM:dd",
    "yyyy-MM-dd HH:mm:ss",
    "yyyy-MM-ddTHH:mm:ss",
    "yyyy-MM-ddTHH:mm:sszzz",
    "dd.MM.yyyy HH:mm:ss",
    "dd.MM.yyyy",
];

    /// <summary>
    /// Attempts to normalize a date/time value into EXIF-like canonical format.
    /// </summary>
    /// <param name="value">Input value to parse.</param>
    /// <param name="normalized">Normalized value in <c>yyyy:MM:dd HH:mm:ss</c> format.</param>
    /// <returns><see langword="true"/> if parsing succeeded; otherwise <see langword="false"/>.</returns>
    private static bool TryNormalizeDateTime(string value, out string normalized)
    {
        if (DateTime.TryParseExact(
                value.Trim(),
                KnownDateTimeFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                out var dt))
        {
            normalized = dt.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
            return true;
        }

        normalized = string.Empty;
        return false;
    }

    /// <summary>
    /// Converts a string value to a comparable typed representation.
    /// </summary>
    /// <param name="value">Raw value to convert.</param>
    /// <param name="normalizedDataType">Normalized target type identifier.</param>
    /// <param name="comparableValue">Converted comparable value when successful.</param>
    /// <returns><see langword="true"/> if conversion succeeded; otherwise <see langword="false"/>.</returns>
    private static bool TryConvertComparable(string value, string normalizedDataType, out IComparable comparableValue)
    {
        switch (normalizedDataType)
        {
            case "INT":
            case "INTEGER":
                if (long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var longValue))
                {
                    comparableValue = longValue;
                    return true;
                }
                break;
            case "DATETIME":
            case "DATE":
            case "TIME":
                if (TryNormalizeDateTime(value, out var normalizedDate) &&
                    DateTime.TryParseExact(normalizedDate, "yyyy:MM:dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue))
                {
                    comparableValue = dateTimeValue;
                    return true;
                }
                break;
        }

        comparableValue = string.Empty;
        return false;
    }

    /// <summary>
    /// Reports a standardized metadata warning through the catalog progress event.
    /// </summary>
    /// <param name="filePath">Source file path associated with the warning.</param>
    /// <param name="keywordName">Logical keyword name associated with the warning.</param>
    /// <param name="reason">Human-readable warning reason.</param>
    private void LogMetadataWarning(string filePath, string keywordName, string reason)
    {
        var warningText = ApiLocalizer.Format("warning.metadata", Culture, Path.GetFileName(filePath), keywordName, reason);

        ProgressChanged?.Invoke(this, new FIRECatalogProgressEventArgs
        {
            Stage = CurrentStage ?? FIRECatalogStage.Collect,
            Level = FIRECatalogMessageLevel.Warning,
            Message = warningText,
            MessageKey = "warning.metadata",
            CurrentFilePath = CurrentFilePath,
            ProcessedCount = ProcessedFileCount,
            TotalCount = TotalFileCount,
            Culture = Culture
        });
    }

    /// <summary>
    /// Checks whether a template contains a counter placeholder.
    /// </summary>
    /// <param name="template">Template text to inspect.</param>
    /// <returns><see langword="true"/> if a counter placeholder is present; otherwise <see langword="false"/>.</returns>
    private static bool ContainsCounterPlaceholder(string template)
    {
        return !string.IsNullOrWhiteSpace(template) && Regex.IsMatch(template, @"\{Counter(?::[^}]+)?\}", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    /// <summary>
    /// Resolves placeholders within a template string.
    /// </summary>
    /// <param name="template">Template containing placeholders like <c>{Key}</c>.</param>
    /// <param name="metadata">Resolved metadata values indexed by key.</param>
    /// <param name="sourceFilePath">Original source file path.</param>
    /// <param name="counter">Optional persistent per-target-scope running index used by <c>{Counter:Dx}</c>.</param>
    /// <returns>Template result with placeholders replaced.</returns>
    private string ParseTemplate(string template, Dictionary<string, string> metadata, string sourceFilePath, long? counter = null)
    {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

        var result = template;
        var placeholderPattern = new Regex(@"\{(?<key>[^}]+)\}", RegexOptions.Compiled);

        result = placeholderPattern.Replace(result, match =>
            ResolvePlaceholder(match.Groups["key"].Value, metadata, sourceFilePath, counter));

        return result;
    }

    private string ApplyConfiguredStringReplacements(string value)
    {
        if (string.IsNullOrEmpty(value) || _configuration.StringReplacements.Count == 0)
        {
            return value;
        }

        var result = value;
        foreach (var replacement in _configuration.StringReplacements)
        {
            result = ApplyStringReplacement(result, replacement.Key, replacement.Value);
        }

        return result;
    }

    internal static string ApplyStringReplacement(string input, string pattern, string replacement)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(pattern))
            return input;

        replacement ??= string.Empty;

        if (pattern.StartsWith("regex:", StringComparison.OrdinalIgnoreCase))
        {
            var regexPattern = pattern["regex:".Length..];
            if (string.IsNullOrWhiteSpace(regexPattern))
                return input;

            try
            {
                return Regex.Replace(input, regexPattern, replacement, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
            catch (ArgumentException)
            {
                return input;
            }
        }

        if (pattern.Contains('*'))
        {
            if (pattern.All(c => c == '*'))
                return replacement;

            var wildcardRegex = Regex.Escape(pattern).Replace("\\*", ".*");
            return Regex.Replace(input, wildcardRegex, replacement, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return input.Replace(pattern, replacement, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Resolves a single placeholder expression to its resulting text value.
    /// </summary>
    /// <param name="key">Placeholder key without braces.</param>
    /// <param name="metadata">Resolved metadata values indexed by key.</param>
    /// <param name="sourceFilePath">Original source file path.</param>
    /// <param name="counter">Optional persistent per-target-scope running index.</param>
    /// <returns>Resolved placeholder value.</returns>
    private string ResolvePlaceholder(string key, Dictionary<string, string> metadata, string sourceFilePath, long? counter = null)
    {
        var parts = key.Split('.');
        var baseName = parts[0];
        var property = parts.Length > 1 ? parts[1] : null;

        if (baseName.StartsWith("Counter:", StringComparison.OrdinalIgnoreCase))
        {
            property = baseName["Counter:".Length..];
            baseName = "Counter";
        }

        if (baseName.Equals("FileName", StringComparison.OrdinalIgnoreCase))
        {
            if (property != null && property.Equals("Noext", StringComparison.OrdinalIgnoreCase))
                return ApplyConfiguredStringReplacements(Path.GetFileNameWithoutExtension(sourceFilePath));
            return ApplyConfiguredStringReplacements(Path.GetFileName(sourceFilePath));
        }

        if (baseName.Equals("MediaRootPath", StringComparison.OrdinalIgnoreCase))
            return ApplyConfiguredStringReplacements(_configuration.MediaRootPath);

        if (baseName.Equals("RootPath", StringComparison.OrdinalIgnoreCase))
            return ParseTemplate(_configuration.RootPath, metadata, sourceFilePath, counter);

        if (baseName.Equals("Counter", StringComparison.OrdinalIgnoreCase))
        {
            var effectiveCounter = counter ?? 1L;
            if (!string.IsNullOrWhiteSpace(property) && property.Length > 1 && property.StartsWith("D", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(property[1..], NumberStyles.None, CultureInfo.InvariantCulture, out var digits)
                && digits > 0)
            {
                return effectiveCounter.ToString($"D{digits}", CultureInfo.InvariantCulture);
            }

            return effectiveCounter.ToString(CultureInfo.InvariantCulture);
        }

        if (!metadata.TryGetValue(baseName, out var value) || string.IsNullOrWhiteSpace(value))
            return "Unknown";

        if (property == null) return ApplyConfiguredStringReplacements(value);

        // Line 548 – previously used DateTime.TryParse, which failed for EXIF-style dates.
        if (TryNormalizeDateTime(value, out var normalizedValue) &&
            DateTime.TryParseExact(normalizedValue, "yyyy:MM:dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            return property.ToUpperInvariant() switch
            {
                "YEAR" => dateTime.Year.ToString(CultureInfo.InvariantCulture),
                "MONTH" => dateTime.Month.ToString("D2", CultureInfo.InvariantCulture),
                "DAY" => dateTime.Day.ToString("D2", CultureInfo.InvariantCulture),
                _ => normalizedValue   // always returns yyyy:MM:dd HH:mm:ss
            };
        }

        //if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        //{
        //    return property.ToUpperInvariant() switch
        //    {
        //        "YEAR" => dateTime.Year.ToString(CultureInfo.InvariantCulture),
        //        "MONTH" => dateTime.Month.ToString("D2", CultureInfo.InvariantCulture),
        //        "DAY" => dateTime.Day.ToString("D2", CultureInfo.InvariantCulture),
        //        _ => value
        //    };
        //}

        return ApplyConfiguredStringReplacements(value);
    }

    /// <summary>
    /// Executes the configured file operation for a single source-target pair.
    /// </summary>
    /// <param name="action">Operation name (COPY, MOVE, LINK).</param>
    /// <param name="sourcePath">Existing source file path.</param>
    /// <param name="targetPath">Destination file path.</param>
    private void ExecuteAction(string action, string sourcePath, string targetPath)
    {
        try
        {
            var targetDirectory = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrWhiteSpace(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            switch (action.ToUpperInvariant())
            {
                case "MOVE":
                    File.Move(sourcePath, targetPath, overwrite: false);
                    break;
                case "COPY":
                    File.Copy(sourcePath, targetPath, overwrite: false);
                    break;
                case "LINK":
                    File.CreateSymbolicLink(targetPath, sourcePath);
                    break;
                default:
                    File.Copy(sourcePath, targetPath, overwrite: false);
                    break;
            }
        }
        catch { }
    }

    /// <summary>
    /// Retrieves all available metadata for a single file from all registered metadata sources.
    /// </summary>
    /// <param name="filePath">Absolute path to the file for which metadata should be retrieved.</param>
    /// <returns>
    /// A list of tuples containing the source name, metadata key, and metadata value for each
    /// available metadata entry. This list shows all metadata that can be extracted from the file
    /// and used in the YAML configuration file.
    /// </returns>
    /// <remarks>
    /// <para>
    /// <strong>Purpose:</strong>
    /// This method is useful for discovering which metadata keys are available for a specific file
    /// type and can be used in the YAML configuration. It queries all registered metadata sources
    /// (FILEINFO, EXIFTOOL, etc.) and returns every key-value pair found.
    /// </para>
    /// 
    /// <para>
    /// <strong>Return Format:</strong>
    /// Each tuple in the returned list contains:
    /// <list type="bullet">
    /// <item><description><c>Source</c>: Name of the metadata source (e.g., "FILEINFO", "EXIFTOOL")</description></item>
    /// <item><description><c>Key</c>: Metadata key/tag name (e.g., "Make", "Model", "DateTimeOriginal")</description></item>
    /// <item><description><c>Value</c>: The extracted metadata value as a string</description></item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// <strong>Usage Example:</strong>
    /// Use this method to explore what metadata is available in your files before configuring
    /// keywords in the YAML configuration. The returned keys can be directly used in the
    /// <c>AvailableKeyWords</c> section of your configuration.
    /// </para>
    /// 
    /// <para>
    /// <strong>Error Handling:</strong>
    /// If the file does not exist or if metadata extraction fails for a particular source,
    /// that source will not contribute entries to the result list. The method continues
    /// processing other sources and does not throw exceptions.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <example>
    /// Discovering available metadata for a file:
    /// <code>
    /// var config = FIREConfigration.Load("Configuration.yaml");
    /// var dbPath = Path.Combine(config.DataBasePath, config.DataBaseFileName);
    /// using var database = new FIREDatabase(dbPath);
    /// using var catalog = new FIRECatalog(config, database);
    /// 
    /// var metadata = catalog.GetAllAvailableMetadata(@"D:\Photos\IMG_1234.jpg");
    /// foreach (var (source, key, value) in metadata)
    /// {
    ///     Console.WriteLine($"[{source}] {key}: {value}");
    /// }
    /// </code>
    /// </example>
    public List<(string Source, string Key, string Value)> GetAllAvailableMetadata(string filePath)
    {
        ThrowIfDisposed();
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var result = new List<(string Source, string Key, string Value)>();

        if (!File.Exists(filePath))
        {
            return result;
        }

        // Iterate through all registered metadata sources
        // Note: _metadataRegistry does not expose a public enumeration method,
        // so we'll try to get the known built-in sources: FILEINFO and EXIFTOOL
        var knownSources = new[] { "FILEINFO", "EXIFTOOL" };

        foreach (var sourceName in knownSources)
        {
            var source = _metadataRegistry.GetSource(sourceName);
            if (source == null)
            {
                continue;
            }

            try
            {
                var metadata = source.ExtractMetadata(filePath);
                foreach (var kvp in metadata)
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Key) && kvp.Value != null)
                    {
                        result.Add((source.SourceName, kvp.Key, kvp.Value));
                    }
                }
            }
            catch
            {
                // If metadata extraction fails for this source, continue with other sources
            }
        }

        return result;
    }

    /// <summary>
    /// Retrieves all available metadata for a single file and writes it to a Markdown file.
    /// </summary>
    /// <param name="filePath">Absolute path to the file for which metadata should be retrieved.</param>
    /// <param name="outputPath">
    /// Optional output path for the Markdown file. If not specified, the output file will be created
    /// in the same directory as the source file with the same name but a .md extension.
    /// </param>
    /// <remarks>
    /// <para>
    /// <strong>Purpose:</strong>
    /// This method extracts all available metadata from the specified file and creates a well-formatted
    /// Markdown report. This report can be used to document which metadata keys are available for
    /// configuration in the YAML file.
    /// </para>
    /// 
    /// <para>
    /// <strong>Output Format:</strong>
    /// The generated Markdown file contains:
    /// <list type="bullet">
    /// <item><description>File information (path, name, size, timestamps)</description></item>
    /// <item><description>Metadata grouped by source (FILEINFO, EXIFTOOL)</description></item>
    /// <item><description>Tables with Key and Value columns for each source</description></item>
    /// <item><description>Summary statistics</description></item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// <strong>Error Handling:</strong>
    /// If the file does not exist or metadata cannot be extracted, an appropriate message is
    /// written to the Markdown file. The method does not throw exceptions for metadata extraction
    /// failures.
    /// </para>
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if this instance has been disposed.
    /// </exception>
    /// <exception cref="IOException">
    /// Thrown if the output file cannot be written.
    /// </exception>
    /// <example>
    /// Creating a metadata report:
    /// <code>
    /// var config = FIREConfigration.Load("Configuration.yaml");
    /// var dbPath = Path.Combine(config.DataBasePath, config.DataBaseFileName);
    /// using var database = new FIREDatabase(dbPath);
    /// using var catalog = new FIRECatalog(config, database);
    /// 
    /// // Creates "IMG_1234.md" in the same directory
    /// catalog.WriteMetadataToMarkdown(@"D:\Photos\IMG_1234.jpg");
    /// 
    /// // Or specify a custom output path
    /// catalog.WriteMetadataToMarkdown(@"D:\Photos\IMG_1234.jpg", @"D:\Reports\metadata.md");
    /// </code>
    /// </example>
    public void WriteMetadataToMarkdown(string filePath, string? outputPath = null)
    {
        ThrowIfDisposed();
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        // Determine output path
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            var directory = Path.GetDirectoryName(filePath) ?? ".";
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            outputPath = Path.Combine(directory, $"{fileNameWithoutExt}.md");
        }

        // Get all metadata
        var metadata = GetAllAvailableMetadata(filePath);

        // Prepare markdown content
        using var writer = new StreamWriter(outputPath, false, System.Text.Encoding.UTF8);

        // Header
        writer.WriteLine("# FIRE Metadata Report");
        writer.WriteLine();
        writer.WriteLine($"**Generated:** {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        writer.WriteLine();

        // File Information
        writer.WriteLine("## File Information");
        writer.WriteLine();

        if (File.Exists(filePath))
        {
            var fileInfo = new FileInfo(filePath);
            writer.WriteLine($"- **Path:** `{filePath}`");
            writer.WriteLine($"- **Name:** `{fileInfo.Name}`");
            writer.WriteLine($"- **Size:** {FormatFileSize(fileInfo.Length)}");
            writer.WriteLine($"- **Created:** {fileInfo.CreationTime:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"- **Modified:** {fileInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"- **Accessed:** {fileInfo.LastAccessTime:yyyy-MM-dd HH:mm:ss}");
        }
        else
        {
            writer.WriteLine($"- **Path:** `{filePath}`");
            writer.WriteLine($"- **Status:** ⚠️ File not found");
        }

        writer.WriteLine();

        // Metadata by Source
        writer.WriteLine("## Extracted Metadata");
        writer.WriteLine();

        if (metadata.Count == 0)
        {
            writer.WriteLine("⚠️ No metadata could be extracted from this file.");
            writer.WriteLine();
        }
        else
        {
            var groupedBySource = metadata.GroupBy(m => m.Source).OrderBy(g => g.Key);

            foreach (var sourceGroup in groupedBySource)
            {
                writer.WriteLine($"### {sourceGroup.Key}");
                writer.WriteLine();
                writer.WriteLine($"**Count:** {sourceGroup.Count()} metadata entries");
                writer.WriteLine();

                // Table header
                writer.WriteLine("| Key | Value |");
                writer.WriteLine("|-----|-------|");

                // Table rows
                foreach (var (_, key, value) in sourceGroup.OrderBy(m => m.Key))
                {
                    var escapedValue = EscapeMarkdown(value);
                    writer.WriteLine($"| `{key}` | {escapedValue} |");
                }

                writer.WriteLine();
            }

            // Summary
            writer.WriteLine("## Summary");
            writer.WriteLine();
            writer.WriteLine($"- **Total Metadata Entries:** {metadata.Count}");
            writer.WriteLine($"- **Sources Used:** {groupedBySource.Count()}");

            foreach (var sourceGroup in groupedBySource)
            {
                writer.WriteLine($"  - {sourceGroup.Key}: {sourceGroup.Count()} entries");
            }
        }

        writer.WriteLine();
        writer.WriteLine("---");
        writer.WriteLine();
        writer.WriteLine("*This report was generated by FIRE (File Information Reorganizer and Extractor).*");
        writer.WriteLine();
        writer.WriteLine("**Usage:** The keys listed above can be used in your YAML configuration file under `AvailableKeyWords` to extract and use these metadata values for file organization.");
    }

    /// <summary>
    /// Formats a file size in bytes to a human-readable string.
    /// </summary>
    /// <param name="bytes">File size in bytes.</param>
    /// <returns>Formatted file size string (e.g., "1.23 MB").</returns>
    private static string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    /// <summary>
    /// Escapes special Markdown characters in a string.
    /// </summary>
    /// <param name="text">Text to escape.</param>
    /// <returns>Escaped text safe for Markdown rendering.</returns>
    private static string EscapeMarkdown(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        // Escape pipe characters for table cells
        text = text.Replace("|", "\\|");

        // Truncate very long values
        if (text.Length > 200)
        {
            text = text.Substring(0, 197) + "...";
        }

        return text;
    }

    /// <summary>
    /// Throws when the catalog instance has already been disposed.
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FIRECatalog));
    }
}