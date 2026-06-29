// (C) 2026 by Thomas Stoll
// MIT License - See LICENSE file for details

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using FIRE;
using Spectre.Console.Cli;

return ProgramHost.Run(args);

internal static class ProgramHost
{
    public static int Run(string[] args)
    {
        var bootstrapCulture = CultureResolver.ResolveCultureCode(CultureResolver.ReadCultureArgument(args) ?? "en-US");
        var bootstrapLanguage = CultureResolver.ResolveLanguage(bootstrapCulture);

        AppLifetime.CurrentLanguage = bootstrapLanguage;

        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            AppLifetime.IsCancellationRequested = true;
            ConsoleUi.WriteWarning(AppLifetime.CurrentLanguage, AppLifetime.NoWrap, TextCatalog.Get(AppLifetime.CurrentLanguage, "cancel_requested"));
        };

        var app = new CommandApp();
        app.Configure(configuration =>
        {
            configuration.SetApplicationName("FIRE.Console");
            configuration.ValidateExamples();

            configuration
                .AddCommand<CollectCommand>("collect")
                .WithDescription(TextCatalog.Get(bootstrapLanguage, "cmd_collect"))
                .WithExample("collect", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "en-US")
                .WithExample("collect", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "de-DE", "--clear-database")
                .WithExample("collect", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "fr-FR", "--no-wrap");

            configuration
                .AddCommand<GenerateCommand>("generate")
                .WithDescription(TextCatalog.Get(bootstrapLanguage, "cmd_generate"))
                .WithExample("generate", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "en-US");

            configuration
                .AddCommand<ExecuteCommand>("execute")
                .WithDescription(TextCatalog.Get(bootstrapLanguage, "cmd_execute"))
                .WithExample("execute", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "de-DE");

            configuration
                .AddCommand<InspectCommand>("inspect")
                .WithDescription(TextCatalog.Get(bootstrapLanguage, "cmd_inspect"))
                .WithExample("inspect", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "en-US", "--file", "image.jpg")
                .WithExample("inspect", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "fil-PH", "--file", "image.jpg", "--copy-path");

            configuration
                .AddCommand<DiagnoseCommand>("diagnose")
                .WithDescription("Diagnoses path generation for a specific source file and writes a detailed Markdown report.")
                .WithExample("diagnose", "--config", "ConfigurationFiles\\Configuration.yaml", "--source-path", "D:\\Photos\\image.jpg")
                .WithExample("diagnose", "--config", "ConfigurationFiles\\Configuration.yaml", "--culture", "de-DE", "--source-path", "D:\\Videos\\video.mp4");
        });

        try
        {
            return app.Run(args);
        }
        catch (Exception ex)
        {
            ConsoleUi.WriteError(bootstrapLanguage, false, $"{TextCatalog.Get(bootstrapLanguage, "error_prefix")} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
    }
}

internal sealed class CollectCommand : Command<CollectSettings>
{
    public override int Execute(CommandContext context, CollectSettings settings)
    {
        var runtime = RuntimeContext.Create(settings.Culture ?? "en-US", settings.NoWrap);
        return CommandExecutor.ExecuteCollect(settings, runtime);
    }
}

internal sealed class GenerateCommand : Command<CommonCommandSettings>
{
    public override int Execute(CommandContext context, CommonCommandSettings settings)
    {
        var runtime = RuntimeContext.Create(settings.Culture ?? "en-US", settings.NoWrap);
        return CommandExecutor.ExecuteGenerate(settings, runtime);
    }
}

internal sealed class ExecuteCommand : Command<CommonCommandSettings>
{
    public override int Execute(CommandContext context, CommonCommandSettings settings)
    {
        var runtime = RuntimeContext.Create(settings.Culture ?? "en-US", settings.NoWrap);
        return CommandExecutor.ExecuteFileOperations(settings, runtime);
    }
}

internal sealed class InspectCommand : Command<InspectSettings>
{
    public override int Execute(CommandContext context, InspectSettings settings)
    {
        var runtime = RuntimeContext.Create(settings.Culture ?? "en-US", settings.NoWrap);
        return CommandExecutor.ExecuteInspect(settings, runtime);
    }
}

internal sealed class DiagnoseCommand : Command<DiagnoseSettings>
{
    public override int Execute(CommandContext context, DiagnoseSettings settings)
    {
        var runtime = RuntimeContext.Create(settings.Culture ?? "en-US", settings.NoWrap);
        return CommandExecutor.ExecuteDiagnose(settings, runtime);
    }
}

internal class CommonCommandSettings : CommandSettings
{
    [Description("Path to the configuration YAML file.")]
    [CommandOption("--config|-c <CONFIG_PATH>")]
    public string? ConfigPath { get; set; }

    [Description("Culture code for output language and date formatting, e.g. en-US, de-DE, fr-FR, fil-PH.")]
    [CommandOption("--culture|-l <CULTURE_CODE>")]
    public string? Culture { get; set; }

    [Description("Disable line wrapping and clip long console lines.")]
    [CommandOption("--no-wrap")]
    [DefaultValue(false)]
    public bool NoWrap { get; set; }
}

internal sealed class CollectSettings : CommonCommandSettings
{
    [Description("Clear the database before collecting files.")]
    [CommandOption("--clear-database|--clear")]
    [DefaultValue(false)]
    public bool ClearDatabase { get; set; }
}

internal sealed class InspectSettings : CommonCommandSettings
{
    [Description("Path to the file to inspect.")]
    [CommandOption("--file|-f <FILE_PATH>")]
    public string? FilePath { get; set; }

    [Description("Output path for the generated Markdown report.")]
    [CommandOption("--output|-o <OUTPUT_PATH>")]
    public string? OutputPath { get; set; }

    [Description("Copy the generated Markdown path to the clipboard.")]
    [CommandOption("--copy-path")]
    [DefaultValue(false)]
    public bool CopyPath { get; set; }
}

internal sealed class DiagnoseSettings : CommonCommandSettings
{
    [Description("Path to the source file to diagnose.")]
    [CommandOption("--source-path|-s <SOURCE_PATH>")]
    public string? SourcePath { get; set; }
}

internal static class CommandExecutor
{
    public static int ExecuteCollect(CollectSettings settings, RuntimeContext runtime)
    {
        return ExecuteCatalogPipeline(
            settings,
            runtime,
            "title_collect",
            "progress_collecting",
            "progress_processing",
            "summary_collect",
            "summary_collect_hint",
            catalog =>
            {
                if (settings.ClearDatabase)
                {
                    ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "clear_database"));
                    catalog.ClearDatabase();
                    ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "database_cleared"));
                    ConsoleUi.WriteEmptyLine();
                }
            },
            (catalog, onFile) => catalog.CollectFiles(onFile));
    }

    public static int ExecuteGenerate(CommonCommandSettings settings, RuntimeContext runtime)
    {
        return ExecuteCatalogPipeline(
            settings,
            runtime,
            "title_generate",
            "progress_generating",
            "progress_generating_item",
            "summary_generate",
            "summary_generate_hint",
            _ => { },
            (catalog, onFile) => catalog.GenerateTargetPaths(onFile));
    }

    public static int ExecuteFileOperations(CommonCommandSettings settings, RuntimeContext runtime)
    {
        return ExecuteCatalogPipeline(
            settings,
            runtime,
            "title_execute",
            "progress_executing",
            "progress_executing_item",
            "summary_execute",
            "summary_execute_hint",
            _ => { },
            (catalog, onFile) => catalog.ExecuteFileOperations(onFile));
    }

    public static int ExecuteInspect(InspectSettings settings, RuntimeContext runtime)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(settings.ConfigPath))
            {
                ConsoleUi.WriteError(runtime, TextCatalog.Get(runtime.Language, "error_config_required"));
                return 1;
            }

            if (string.IsNullOrWhiteSpace(settings.FilePath))
            {
                ConsoleUi.WriteError(runtime, TextCatalog.Get(runtime.Language, "error_file_required"));
                return 1;
            }

            var filePath = settings.FilePath;
            if (!File.Exists(filePath))
            {
                ConsoleUi.WriteError(runtime, $"{TextCatalog.Get(runtime.Language, "error_file_not_found")} {filePath}");
                return 1;
            }

            var normalizedCulture = runtime.CultureCode;
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(normalizedCulture);

            ConsoleUi.WriteTitle(runtime, TextCatalog.Get(runtime.Language, "title_inspect"));
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_config")} {settings.ConfigPath}");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_culture")} {normalizedCulture}");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_file")} {filePath}");
            if (!string.IsNullOrWhiteSpace(settings.OutputPath))
            {
                ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_output")} {settings.OutputPath}");
            }

            ConsoleUi.WriteEmptyLine();
            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "loading_configuration"));
            var config = FIREConfigration.Load(settings.ConfigPath!);
            config.EnsureSupportedConfigurationVersion();
            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "configuration_loaded"), config.ConfigurationVersion));
            ConsoleUi.WriteEmptyLine();

            var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_database")} {dbPath}");
            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "initializing_catalog"));

            using var database = new FIREDatabase(dbPath);
            using var catalog = new FIRECatalog(config, database)
            {
                Culture = CultureInfo.CurrentUICulture
            };

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "catalog_initialized"));
            ConsoleUi.WriteEmptyLine();

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "extracting_metadata"));
            var stopwatch = Stopwatch.StartNew();
            var metadata = catalog.GetAllAvailableMetadata(filePath);
            stopwatch.Stop();

            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "metadata_extracted_ms"), stopwatch.Elapsed.TotalMilliseconds));
            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "metadata_entries_found"), metadata.Count));
            ConsoleUi.WriteEmptyLine();

            if (metadata.Count > 0)
            {
                ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "metadata_by_source"));
                foreach (var sourceGroup in metadata.GroupBy(entry => entry.Source).OrderBy(group => group.Key))
                {
                    ConsoleUi.WriteLine(runtime, $"  - {sourceGroup.Key}: {sourceGroup.Count()}");
                }
                ConsoleUi.WriteEmptyLine();
            }
            else
            {
                ConsoleUi.WriteWarning(runtime, TextCatalog.Get(runtime.Language, "warn_no_metadata"));
                ConsoleUi.WriteEmptyLine();
            }

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "writing_markdown"));
            catalog.WriteMetadataToMarkdown(filePath, settings.OutputPath);

            var finalOutputPath = settings.OutputPath ?? Path.Combine(
                Path.GetDirectoryName(filePath) ?? ".",
                Path.GetFileNameWithoutExtension(filePath) + ".md");
            finalOutputPath = Path.GetFullPath(finalOutputPath);

            ConsoleUi.WriteSuccess(runtime, $"{TextCatalog.Get(runtime.Language, "markdown_created")} {finalOutputPath}");

            if (settings.CopyPath)
            {
                if (TryCopyToClipboard(finalOutputPath))
                {
                    ConsoleUi.WriteInfo(runtime, TextCatalog.Get(runtime.Language, "markdown_copied"));
                }
                else
                {
                    ConsoleUi.WriteWarning(runtime, TextCatalog.Get(runtime.Language, "warn_copy_failed"));
                }
            }

            ConsoleUi.WriteEmptyLine();
            return 0;
        }
        catch (OperationCanceledException)
        {
            ConsoleUi.WriteWarning(runtime, TextCatalog.Get(runtime.Language, "operation_canceled"));
            return 2;
        }
        catch (Exception ex)
        {
            ConsoleUi.WriteError(runtime, $"{TextCatalog.Get(runtime.Language, "error_prefix")} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
    }

    public static int ExecuteDiagnose(DiagnoseSettings settings, RuntimeContext runtime)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(settings.ConfigPath))
            {
                ConsoleUi.WriteError(runtime, TextCatalog.Get(runtime.Language, "error_config_required"));
                return 1;
            }

            if (string.IsNullOrWhiteSpace(settings.SourcePath))
            {
                ConsoleUi.WriteError(runtime, "Source path is required. Use --source-path to specify the file.");
                return 1;
            }

            var sourcePath = settings.SourcePath;
            if (!File.Exists(sourcePath))
            {
                ConsoleUi.WriteError(runtime, $"Source file not found: {sourcePath}");
                return 1;
            }

            var normalizedCulture = runtime.CultureCode;
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(normalizedCulture);

            ConsoleUi.WriteTitle(runtime, "FIRE Path Generation Diagnosis");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_config")} {settings.ConfigPath}");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_culture")} {normalizedCulture}");
            ConsoleUi.WriteLine(runtime, $"Source File: {sourcePath}");
            ConsoleUi.WriteEmptyLine();

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "loading_configuration"));
            var config = FIREConfigration.Load(settings.ConfigPath!);
            config.EnsureSupportedConfigurationVersion();
            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "configuration_loaded"), config.ConfigurationVersion));
            ConsoleUi.WriteEmptyLine();

            var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
            ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_database")} {dbPath}");

            if (!File.Exists(dbPath))
            {
                ConsoleUi.WriteError(runtime, $"Database not found: {dbPath}");
                ConsoleUi.WriteWarning(runtime, "Please run 'collect' first to populate the database.");
                return 1;
            }

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "initializing_catalog"));

            using var database = new FIREDatabase(dbPath);
            using var catalog = new FIRECatalog(config, database)
            {
                Culture = CultureInfo.CurrentUICulture
            };

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "catalog_initialized"));
            ConsoleUi.WriteEmptyLine();

            ConsoleUi.WriteLine(runtime, "Generating diagnostic report...");
            var stopwatch = Stopwatch.StartNew();
            var reportPath = catalog.DiagnoseGeneration(sourcePath);
            stopwatch.Stop();

            if (reportPath == null)
            {
                ConsoleUi.WriteWarning(runtime, $"Source file not found in database: {sourcePath}");
                ConsoleUi.WriteWarning(runtime, "This file has not been collected yet. Run 'collect' first.");
                ConsoleUi.WriteEmptyLine();
                return 1;
            }

            ConsoleUi.WriteLine(runtime, $"Diagnosis completed in {stopwatch.Elapsed.TotalMilliseconds:F0} ms");
            ConsoleUi.WriteEmptyLine();
            ConsoleUi.WriteSuccess(runtime, $"Diagnostic report written to: {reportPath}");
            ConsoleUi.WriteEmptyLine();

            return 0;
        }
        catch (Exception ex)
        {
            ConsoleUi.WriteError(runtime, $"{TextCatalog.Get(runtime.Language, "error_prefix")} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
    }

    private static int ExecuteCatalogPipeline(
        CommonCommandSettings settings,
        RuntimeContext runtime,
        string titleKey,
        string progressLineKey,
        string progressVerbKey,
        string summaryKey,
        string summaryHintKey,
        Action<FIRECatalog> beforeAction,
        Action<FIRECatalog, Action<string>> action)
    {
        if (string.IsNullOrWhiteSpace(settings.ConfigPath))
        {
            ConsoleUi.WriteError(runtime, TextCatalog.Get(runtime.Language, "error_config_required"));
            return 1;
        }

        var normalizedCulture = runtime.CultureCode;
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(normalizedCulture);

        ConsoleUi.WriteTitle(runtime, TextCatalog.Get(runtime.Language, titleKey));
        ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_config")} {settings.ConfigPath}");
        ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_culture")} {normalizedCulture}");
        ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_wrap_mode")} {(runtime.NoWrap ? TextCatalog.Get(runtime.Language, "wrap_clipping") : TextCatalog.Get(runtime.Language, "wrap_enabled"))}");
        ConsoleUi.WriteEmptyLine();

        ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "loading_configuration"));
        FIREConfigration config;
        try
        {
            config = FIREConfigration.Load(settings.ConfigPath!);
            config.EnsureSupportedConfigurationVersion();
        }
        catch (Exception ex)
        {
            ConsoleUi.WriteError(runtime, $"{TextCatalog.Get(runtime.Language, "error_prefix")} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }

        ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "configuration_loaded"), config.ConfigurationVersion));
        ConsoleUi.WriteEmptyLine();

        var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
        ConsoleUi.WriteLine(runtime, $"{TextCatalog.Get(runtime.Language, "label_database")} {dbPath}");
        ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "initializing_catalog"));

        var database = new FIREDatabase(dbPath);
        var catalog = new FIRECatalog(config, database)
        {
            Culture = CultureInfo.CurrentUICulture
        };

        try
        {
            catalog.ProgressChanged += (_, progressEvent) =>
            {
                AppLifetime.ThrowIfCancellationRequested();

                if (progressEvent.Level == FIRECatalogMessageLevel.Warning)
                {
                    ConsoleUi.EndProgressLine();
                    ConsoleUi.WriteWarning(runtime, progressEvent.Message);
                    return;
                }

                if (progressEvent.Level == FIRECatalogMessageLevel.Trace &&
                    !string.IsNullOrWhiteSpace(progressEvent.CurrentFilePath))
                {
                    ConsoleUi.WriteProgress(
                        runtime,
                        $"[{progressEvent.ProcessedCount}] {TextCatalog.Get(runtime.Language, progressVerbKey)} {progressEvent.CurrentFilePath}");
                }
            };

            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "catalog_initialized"));
            ConsoleUi.WriteEmptyLine();

            beforeAction(catalog);
            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, progressLineKey));

            var stopwatch = Stopwatch.StartNew();
            action(catalog, _ => { });
            stopwatch.Stop();

            ConsoleUi.EndProgressLine();
            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, summaryKey), stopwatch.Elapsed.TotalSeconds));
            ConsoleUi.WriteLine(runtime, string.Format(CultureInfo.CurrentCulture, TextCatalog.Get(runtime.Language, "summary_total_records"), database.Count));
            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, "summary_warnings"));
            ConsoleUi.WriteEmptyLine();
            ConsoleUi.WriteLine(runtime, TextCatalog.Get(runtime.Language, summaryHintKey));
            return 0;
        }
        catch (OperationCanceledException)
        {
            catalog.LogCancelled();
            ConsoleUi.EndProgressLine();
            ConsoleUi.WriteWarning(runtime, TextCatalog.Get(runtime.Language, "operation_canceled"));
            return 2;
        }
        catch (Exception ex)
        {
            ConsoleUi.WriteError(runtime, $"{TextCatalog.Get(runtime.Language, "error_prefix")} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
        finally
        {
            catalog.Dispose();
            database.Dispose();
        }
    }

    private static bool TryCopyToClipboard(string text)
    {
        if (string.IsNullOrWhiteSpace(text) || !OperatingSystem.IsWindows())
        {
            return false;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "clip",
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process is null)
            {
                return false;
            }

            process.StandardInput.Write(text);
            process.StandardInput.Close();
            process.WaitForExit(2000);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }
}

internal readonly record struct RuntimeContext(string CultureCode, UiLanguage Language, bool NoWrap)
{
    public static RuntimeContext Create(string cultureCode, bool noWrap)
    {
        var normalizedCulture = CultureResolver.ResolveCultureCode(cultureCode);
        var language = CultureResolver.ResolveLanguage(normalizedCulture);

        AppLifetime.CurrentLanguage = language;
        AppLifetime.NoWrap = noWrap;

        return new RuntimeContext(normalizedCulture, language, noWrap);
    }
}

internal enum UiLanguage
{
    English,
    German,
    French,
    Filipino
}

internal static class CultureResolver
{
    private static readonly Dictionary<string, string> CultureAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = "en-US",
        ["en-en"] = "en-US",
        ["en-gb"] = "en-GB",
        ["de"] = "de-DE",
        ["fr"] = "fr-FR",
        ["fil"] = "fil-PH",
        ["tl"] = "fil-PH",
        ["ph"] = "fil-PH"
    };

    public static string? ReadCultureArgument(string[] args)
    {
        for (var index = 0; index < args.Length; index++)
        {
            if ((args[index].Equals("--culture", StringComparison.OrdinalIgnoreCase) || args[index].Equals("-l", StringComparison.OrdinalIgnoreCase))
                && index + 1 < args.Length)
            {
                return args[index + 1];
            }
        }

        return null;
    }

    public static string ResolveCultureCode(string rawCulture)
    {
        if (string.IsNullOrWhiteSpace(rawCulture))
        {
            return "en-US";
        }

        var trimmed = rawCulture.Trim();
        if (CultureAliases.TryGetValue(trimmed, out var alias))
        {
            return alias;
        }

        try
        {
            var culture = CultureInfo.GetCultureInfo(trimmed);
            if (CultureAliases.TryGetValue(culture.Name, out var mappedSpecificCulture))
            {
                return mappedSpecificCulture;
            }

            return culture.Name;
        }
        catch
        {
            return "en-US";
        }
    }

    public static UiLanguage ResolveLanguage(string cultureCode)
    {
        if (cultureCode.StartsWith("de", StringComparison.OrdinalIgnoreCase))
        {
            return UiLanguage.German;
        }

        if (cultureCode.StartsWith("fr", StringComparison.OrdinalIgnoreCase))
        {
            return UiLanguage.French;
        }

        if (cultureCode.StartsWith("fil", StringComparison.OrdinalIgnoreCase) || cultureCode.StartsWith("tl", StringComparison.OrdinalIgnoreCase))
        {
            return UiLanguage.Filipino;
        }

        return UiLanguage.English;
    }
}

internal static class ConsoleUi
{
    public static void WriteTitle(RuntimeContext runtime, string text)
    {
        WriteLine(runtime, $"=== {text} ===");
    }

    public static void WriteLine(RuntimeContext runtime, string text)
    {
        Console.WriteLine(FormatLine(text, runtime.NoWrap));
    }

    public static void WriteInfo(RuntimeContext runtime, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(FormatLine(text, runtime.NoWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteSuccess(RuntimeContext runtime, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(FormatLine(text, runtime.NoWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteWarning(RuntimeContext runtime, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(FormatLine(text, runtime.NoWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteWarning(UiLanguage language, bool noWrap, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(FormatLine(text, noWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteError(RuntimeContext runtime, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(FormatLine(text, runtime.NoWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteError(UiLanguage language, bool noWrap, string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(FormatLine(text, noWrap));
        Console.ForegroundColor = previous;
    }

    public static void WriteProgress(RuntimeContext runtime, string text)
    {
        var width = GetSafeWidth();
        var line = runtime.NoWrap ? FormatLine(text, true) : text;
        //Console.Write($"\r{line}".PadRight(width));
        Console.Write($"\r\n{line}".PadRight(width));
    }

    public static void EndProgressLine()
    {
        Console.WriteLine();
    }

    public static void WriteEmptyLine()
    {
        Console.WriteLine();
    }

    private static string FormatLine(string text, bool noWrap)
    {
        if (!noWrap)
        {
            return text;
        }

        var width = GetSafeWidth();
        if (width < 4 || text.Length <= width)
        {
            return text;
        }

        return string.Concat(text.AsSpan(0, width - 3), "...");
    }

    private static int GetSafeWidth()
    {
        try
        {
            return Math.Max(Console.WindowWidth - 1, 20);
        }
        catch
        {
            return 120;
        }
    }
}

internal static class TextCatalog
{
    private static readonly IReadOnlyDictionary<string, LocalizedText> Texts = new Dictionary<string, LocalizedText>(StringComparer.Ordinal)
    {
        ["cmd_collect"] = new("Collect files from configured source directories.", "Dateien aus den konfigurierten Quellverzeichnissen sammeln.", "Collecter les fichiers depuis les dossiers source configurés.", "Mangolekta ng mga file mula sa mga naka-configure na source directory."),
        ["cmd_generate"] = new("Generate target paths and names for collected files.", "Zielpfade und Namen für gesammelte Dateien erzeugen.", "Générer les chemins et noms cibles pour les fichiers collectés.", "Bumuo ng target na path at pangalan para sa mga nakolektang file."),
        ["cmd_execute"] = new("Execute file operations (copy/move/link).", "Dateioperationen ausführen (Kopieren/Verschieben/Verknüpfen).", "Exécuter les opérations sur fichiers (copier/déplacer/lier).", "Isagawa ang mga file operation (copy/move/link)."),
        ["cmd_inspect"] = new("Inspect one file and export metadata to Markdown.", "Eine Datei prüfen und Metadaten nach Markdown exportieren.", "Inspecter un fichier et exporter les métadonnées en Markdown.", "Siyasatin ang isang file at i-export ang metadata sa Markdown."),
        ["cancel_requested"] = new("CTRL+C detected. Stopping gracefully...", "CTRL+C erkannt. Vorgang wird sauber beendet...", "CTRL+C détecté. Arrêt propre en cours...", "Nakita ang CTRL+C. Maayos na itinitigil..."),
        ["error_prefix"] = new("ERROR:", "FEHLER:", "ERREUR :", "ERROR:"),
        ["error_config_required"] = new("ERROR: --config is required.", "FEHLER: --config ist erforderlich.", "ERREUR : --config est requis.", "ERROR: Kailangan ang --config."),
        ["error_file_required"] = new("ERROR: --file is required for inspect.", "FEHLER: --file ist für inspect erforderlich.", "ERREUR : --file est requis pour inspect.", "ERROR: Kailangan ang --file para sa inspect."),
        ["error_file_not_found"] = new("File not found:", "Datei nicht gefunden:", "Fichier introuvable :", "Hindi nahanap ang file:"),
        ["title_collect"] = new("FIRE Collect Files", "FIRE Dateien sammeln", "FIRE Collecte des fichiers", "FIRE Pangongolekta ng mga File"),
        ["title_generate"] = new("FIRE Generate Target Paths", "FIRE Zielpfade erzeugen", "FIRE Génération des chemins cibles", "FIRE Pagbuo ng Target Paths"),
        ["title_execute"] = new("FIRE Execute File Operations", "FIRE Dateioperationen ausführen", "FIRE Exécution des opérations sur fichiers", "FIRE Pagpapatupad ng File Operations"),
        ["title_inspect"] = new("FIRE Inspect File Metadata", "FIRE Datei-Metadaten prüfen", "FIRE Inspection des métadonnées", "FIRE Pagsusuri ng Metadata ng File"),
        ["label_config"] = new("Config:", "Konfiguration:", "Configuration :", "Config:"),
        ["label_culture"] = new("Culture:", "Kultur:", "Culture :", "Culture:"),
        ["label_database"] = new("Database:", "Datenbank:", "Base de données :", "Database:"),
        ["label_file"] = new("File:", "Datei:", "Fichier :", "File:"),
        ["label_output"] = new("Output:", "Ausgabe:", "Sortie :", "Output:"),
        ["label_wrap_mode"] = new("Long line mode:", "Modus für lange Zeilen:", "Mode des lignes longues :", "Mode ng mahahabang linya:"),
        ["wrap_enabled"] = new("Wrap enabled", "Zeilenumbruch aktiv", "Retour à la ligne activé", "Naka-enable ang line wrap"),
        ["wrap_clipping"] = new("No wrap, clipping enabled", "Kein Umbruch, Abschneiden aktiv", "Sans retour à la ligne, rognage activé", "Walang line wrap, naka-enable ang clipping"),
        ["loading_configuration"] = new("Loading configuration...", "Lade Konfiguration...", "Chargement de la configuration...", "Nilo-load ang configuration..."),
        ["configuration_loaded"] = new("Configuration loaded successfully (Version: {0}).", "Konfiguration erfolgreich geladen (Version: {0}).", "Configuration chargée avec succès (Version : {0}).", "Matagumpay na na-load ang configuration (Version: {0})."),
        ["initializing_catalog"] = new("Initializing catalog...", "Initialisiere Katalog...", "Initialisation du catalogue...", "Ini-initialize ang catalog..."),
        ["catalog_initialized"] = new("Catalog initialized.", "Katalog initialisiert.", "Catalogue initialisé.", "Na-initialize ang catalog."),
        ["clear_database"] = new("Clearing database...", "Lösche Datenbank...", "Nettoyage de la base de données...", "Nililinis ang database..."),
        ["database_cleared"] = new("Database cleared.", "Datenbank gelöscht.", "Base de données nettoyée.", "Nalinis ang database."),
        ["progress_collecting"] = new("Collecting files...", "Sammle Dateien...", "Collecte des fichiers...", "Nangongolekta ng mga file..."),
        ["progress_generating"] = new("Generating target paths...", "Erzeuge Zielpfade...", "Génération des chemins cibles...", "Bumubuo ng target paths..."),
        ["progress_executing"] = new("Executing file operations...", "Führe Dateioperationen aus...", "Exécution des opérations sur fichiers...", "Isinasagawa ang file operations..."),
        ["progress_processing"] = new("Processing:", "Verarbeite:", "Traitement :", "Pinoproseso:"),
        ["progress_generating_item"] = new("Generating:", "Erzeuge:", "Génération :", "Binubuo:"),
        ["progress_executing_item"] = new("Executing:", "Führe aus:", "Exécution :", "Isinasagawa:"),
        ["summary_collect"] = new("File collection completed in {0:F2} seconds.", "Dateisammlung in {0:F2} Sekunden abgeschlossen.", "Collecte terminée en {0:F2} secondes.", "Natapos ang pangongolekta sa loob ng {0:F2} segundo."),
        ["summary_generate"] = new("Target path generation completed in {0:F2} seconds.", "Zielpfad-Erzeugung in {0:F2} Sekunden abgeschlossen.", "Génération des chemins terminée en {0:F2} secondes.", "Natapos ang pagbuo ng target path sa loob ng {0:F2} segundo."),
        ["summary_execute"] = new("File operations completed in {0:F2} seconds.", "Dateioperationen in {0:F2} Sekunden abgeschlossen.", "Opérations terminées en {0:F2} secondes.", "Natapos ang file operations sa loob ng {0:F2} segundo."),
        ["summary_total_records"] = new("Total records: {0}", "Datensätze gesamt: {0}", "Total des enregistrements : {0}", "Kabuuang records: {0}"),
        ["summary_warnings"] = new("Warnings: missing metadata entries are reported inline with [WARN].", "Warnungen: fehlende Metadaten werden inline mit [WARN] ausgegeben.", "Avertissements : les métadonnées manquantes sont signalées avec [WARN].", "Babala: ang nawawalang metadata ay ipinapakita gamit ang [WARN]."),
        ["summary_collect_hint"] = new("Use the database to inspect collected files.", "Verwenden Sie die Datenbank, um gesammelte Dateien zu prüfen.", "Utilisez la base de données pour inspecter les fichiers collectés.", "Gamitin ang database para siyasatin ang mga nakolektang file."),
        ["summary_generate_hint"] = new("Use the database to inspect generated target paths.", "Verwenden Sie die Datenbank, um erzeugte Zielpfade zu prüfen.", "Utilisez la base de données pour inspecter les chemins générés.", "Gamitin ang database para siyasatin ang mga nabuong target path."),
        ["summary_execute_hint"] = new("File operations have been executed.", "Dateioperationen wurden ausgeführt.", "Les opérations sur fichiers ont été exécutées.", "Naipatupad ang file operations."),
        ["extracting_metadata"] = new("Extracting metadata...", "Extrahiere Metadaten...", "Extraction des métadonnées...", "Kinukuha ang metadata..."),
        ["metadata_extracted_ms"] = new("Metadata extracted in {0:F2} ms.", "Metadaten in {0:F2} ms extrahiert.", "Métadonnées extraites en {0:F2} ms.", "Nakuha ang metadata sa {0:F2} ms."),
        ["metadata_entries_found"] = new("Total entries found: {0}", "Gefundene Einträge: {0}", "Entrées trouvées : {0}", "Kabuuang nahanap na entry: {0}"),
        ["metadata_by_source"] = new("Metadata by source:", "Metadaten nach Quelle:", "Métadonnées par source :", "Metadata ayon sa source:"),
        ["warn_no_metadata"] = new("WARNING: No metadata could be extracted from this file.", "WARNUNG: Für diese Datei konnten keine Metadaten extrahiert werden.", "AVERTISSEMENT : aucune métadonnée n'a pu être extraite de ce fichier.", "BABALA: Walang metadata na nakuha mula sa file na ito."),
        ["writing_markdown"] = new("Writing Markdown report...", "Schreibe Markdown-Bericht...", "Écriture du rapport Markdown...", "Isinusulat ang ulat na Markdown..."),
        ["markdown_created"] = new("✓ Markdown report created:", "✓ Markdown-Bericht erstellt:", "✓ Rapport Markdown créé :", "✓ Nalikha ang ulat na Markdown:"),
        ["markdown_copied"] = new("✓ Markdown path copied to clipboard.", "✓ Markdown-Pfad in Zwischenablage kopiert.", "✓ Chemin Markdown copié dans le presse-papiers.", "✓ Nakopya sa clipboard ang landas ng Markdown."),
        ["warn_copy_failed"] = new("WARNING: Could not copy Markdown path to clipboard.", "WARNUNG: Markdown-Pfad konnte nicht in die Zwischenablage kopiert werden.", "AVERTISSEMENT : impossible de copier le chemin Markdown dans le presse-papiers.", "BABALA: Hindi makopya sa clipboard ang landas ng Markdown."),
        ["operation_canceled"] = new("Operation canceled by user (CTRL+C).", "Vorgang vom Benutzer abgebrochen (CTRL+C).", "Opération annulée par l'utilisateur (CTRL+C).", "Kinansela ng user ang operasyon (CTRL+C).")
    };

    public static string Get(UiLanguage language, string key)
    {
        if (!Texts.TryGetValue(key, out var text))
        {
            return key;
        }

        return language switch
        {
            UiLanguage.German => text.German,
            UiLanguage.French => text.French,
            UiLanguage.Filipino => text.Filipino,
            _ => text.English
        };
    }
}

internal readonly record struct LocalizedText(string English, string German, string French, string Filipino);

internal static class AppLifetime
{
    public static volatile bool IsCancellationRequested;
    public static UiLanguage CurrentLanguage = UiLanguage.English;
    public static bool NoWrap;

    public static void ThrowIfCancellationRequested()
    {
        if (IsCancellationRequested)
        {
            throw new OperationCanceledException("Operation canceled by user.");
        }
    }
}
