// (C) 2026 by Thomas Stoll
// MIT License - See LICENSE file for details

using System.Globalization;
using FIRE;

// Simple command-line argument parsing
if (args.Length == 0)
{
    ShowHelp();
    return 1;
}

var command = args[0].ToLowerInvariant();
string? configPath = null;
string? culture = null;
bool clearDatabase = false;

// Parse arguments
for (int i = 1; i < args.Length; i++)
{
    if ((args[i] == "--config" || args[i] == "-c") && i + 1 < args.Length)
    {
        configPath = args[++i];
    }
    else if ((args[i] == "--culture" || args[i] == "-l") && i + 1 < args.Length)
    {
        culture = args[++i];
    }
    else if (args[i] == "--clear-database" || args[i] == "--clear")
    {
        clearDatabase = true;
    }
}

// Validate required arguments
if (string.IsNullOrWhiteSpace(configPath))
{
    Console.WriteLine("ERROR: --config parameter is required");
    ShowHelp();
    return 1;
}

if (string.IsNullOrWhiteSpace(culture))
{
    Console.WriteLine("ERROR: --culture parameter is required");
    ShowHelp();
    return 1;
}

// Execute command
return command switch
{
    "collect" => ExecuteCollect(configPath, culture, clearDatabase),
    "generate" => ExecuteGenerate(configPath, culture),
    "execute" => ExecuteOperations(configPath, culture),
    "inspect" => ExecuteInspect(args, configPath, culture),
    _ => ShowHelp()
};

static int ShowHelp()
{
    Console.WriteLine("FIRE - File Information Reorganizer and Extractor - Test Console");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  FIRE.Console <command> --config <path> --culture <culture> [options]");
    Console.WriteLine();
    Console.WriteLine("Commands:");
    Console.WriteLine("  collect   - Collect files from configured source directories");
    Console.WriteLine("  generate  - Generate target paths and names for collected files");
    Console.WriteLine("  execute   - Execute file operations (copy/move/link)");
    Console.WriteLine("  inspect   - Inspect a file and extract all available metadata to Markdown");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --config, -c        Path to the configuration YAML file (required)");
    Console.WriteLine("  --culture, -l       Culture code, e.g., de-DE, en-EN (required)");
    Console.WriteLine("  --clear-database    Clear the database before collecting (only for collect command)");
    Console.WriteLine("  --clear             Short form of --clear-database");
    Console.WriteLine("  --file, -f          Path to the file to inspect (required for inspect command)");
    Console.WriteLine("  --output, -o        Output path for the Markdown report (optional for inspect)");
    Console.WriteLine();
    Console.WriteLine("Examples:");
    Console.WriteLine("  FIRE.Console collect --config \"Configuration.yaml\" --culture \"de-DE\"");
    Console.WriteLine("  FIRE.Console collect --config \"Configuration.yaml\" --culture \"de-DE\" --clear-database");
    Console.WriteLine("  FIRE.Console generate --config \"Configuration.yaml\" --culture \"en-EN\"");
    Console.WriteLine("  FIRE.Console execute --config \"Configuration.yaml\" --culture \"de-DE\"");
    Console.WriteLine("  FIRE.Console inspect --config \"Configuration.yaml\" --culture \"de-DE\" --file \"image.jpg\"");
    Console.WriteLine("  FIRE.Console inspect --config \"Configuration.yaml\" --culture \"de-DE\" --file \"image.jpg\" --output \"report.md\"");
    return 1;
}

// Implementation methods
static int ExecuteCollect(string configPath, string culture, bool clearDatabase)
{
    try
    {
        Console.WriteLine($"=== FIRE Collect Files ===");
        Console.WriteLine($"Config: {configPath}");
        Console.WriteLine($"Culture: {culture}");
        if (clearDatabase)
            Console.WriteLine("Database will be cleared before collecting.");
        Console.WriteLine();

        // Set culture
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(culture);

        // Load configuration
        Console.WriteLine("Loading configuration...");
        var config = FIREConfigration.Load(configPath);
        config.EnsureSupportedConfigurationVersion();
        Console.WriteLine($"Configuration loaded successfully (Version: {config.ConfigurationVersion})");
        Console.WriteLine();

        // Create database path
        var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
        Console.WriteLine($"Database: {dbPath}");

        // Create catalog
        Console.WriteLine("Initializing catalog...");
        using var database = new FIREDatabase(dbPath);
        using var catalog = new FIRECatalog(config, database);
        Console.WriteLine("Catalog initialized.");
        Console.WriteLine();

        // Clear database if requested
        if (clearDatabase)
        {
            Console.WriteLine("Clearing database...");
            catalog.ClearDatabase();
            Console.WriteLine("Database cleared.");
            Console.WriteLine();
        }

        // Collect files
        Console.WriteLine("Collecting files...");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int fileCount = 0;
        catalog.CollectFiles(filePath =>
        {
            fileCount++;
            Console.Write($"\r[{fileCount}] Processing: {TruncatePath(filePath, 80)}".PadRight(Console.WindowWidth - 1));
        });
        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine($"File collection completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        Console.WriteLine($"Total records: {database.Count}");
        Console.WriteLine("Warnings: missing metadata entries are reported inline with [WARN].");
        Console.WriteLine();

        Console.WriteLine("Use the database to inspect collected files.");
        return 0;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ResetColor();
        Console.WriteLine(ex.StackTrace);
        return 1;
    }
}

static int ExecuteGenerate(string configPath, string culture)
{
    try
    {
        Console.WriteLine($"=== FIRE Generate Target Paths ===");
        Console.WriteLine($"Config: {configPath}");
        Console.WriteLine($"Culture: {culture}");
        Console.WriteLine();

        // Set culture
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(culture);

        // Load configuration
        Console.WriteLine("Loading configuration...");
        var config = FIREConfigration.Load(configPath);
        config.EnsureSupportedConfigurationVersion();
        Console.WriteLine($"Configuration loaded successfully (Version: {config.ConfigurationVersion})");
        Console.WriteLine();

        // Create database path
        var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
        Console.WriteLine($"Database: {dbPath}");

        // Create catalog
        Console.WriteLine("Initializing catalog...");
        using var database = new FIREDatabase(dbPath);
        using var catalog = new FIRECatalog(config, database);
        Console.WriteLine("Catalog initialized.");
        Console.WriteLine();

        // Generate target paths
        Console.WriteLine("Generating target paths...");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int fileCount = 0;
        catalog.GenerateTargetPaths(filePath =>
        {
            fileCount++;
            Console.Write($"\r[{fileCount}] Generating: {TruncatePath(filePath, 80)}".PadRight(Console.WindowWidth - 1));
        });
        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine($"Target path generation completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        Console.WriteLine($"Total records: {database.Count}");
        Console.WriteLine("Warnings: missing metadata entries are reported inline with [WARN].");
        Console.WriteLine();

        Console.WriteLine("Use the database to inspect generated target paths.");
        return 0;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ResetColor();
        Console.WriteLine(ex.StackTrace);
        return 1;
    }
}

static int ExecuteOperations(string configPath, string culture)
{
    try
    {
        Console.WriteLine($"=== FIRE Execute File Operations ===");
        Console.WriteLine($"Config: {configPath}");
        Console.WriteLine($"Culture: {culture}");
        Console.WriteLine();

        // Set culture
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(culture);

        // Load configuration
        Console.WriteLine("Loading configuration...");
        var config = FIREConfigration.Load(configPath);
        config.EnsureSupportedConfigurationVersion();
        Console.WriteLine($"Configuration loaded successfully (Version: {config.ConfigurationVersion})");
        Console.WriteLine();

        // Create database path
        var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");
        Console.WriteLine($"Database: {dbPath}");

        // Create catalog
        Console.WriteLine("Initializing catalog...");
        using var database = new FIREDatabase(dbPath);
        using var catalog = new FIRECatalog(config, database);
        Console.WriteLine("Catalog initialized.");
        Console.WriteLine();

        // Execute file operations
        Console.WriteLine("Executing file operations...");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int fileCount = 0;
        catalog.ExecuteFileOperations(filePath =>
        {
            fileCount++;
            Console.Write($"\r[{fileCount}] Executing: {TruncatePath(filePath, 80)}".PadRight(Console.WindowWidth - 1));
        });
        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine($"File operations completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        Console.WriteLine($"Total records: {database.Count}");
        Console.WriteLine("Warnings: missing metadata entries are reported inline with [WARN].");
        Console.WriteLine();

        Console.WriteLine("File operations have been executed.");
        return 0;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ResetColor();
        Console.WriteLine(ex.StackTrace);
        return 1;
    }
}

static int ExecuteInspect(string[] args, string configPath, string culture)
{
    try
    {
        // Parse additional arguments for inspect command
        string? filePath = null;
        string? outputPath = null;

        for (int i = 1; i < args.Length; i++)
        {
            if ((args[i] == "--file" || args[i] == "-f") && i + 1 < args.Length)
            {
                filePath = args[++i];
            }
            else if ((args[i] == "--output" || args[i] == "-o") && i + 1 < args.Length)
            {
                outputPath = args[++i];
            }
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: --file parameter is required for inspect command");
            Console.ResetColor();
            Console.WriteLine();
            ShowHelp();
            return 1;
        }

        if (!File.Exists(filePath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: File not found: {filePath}");
            Console.ResetColor();
            return 1;
        }

        Console.WriteLine($"=== FIRE Inspect File Metadata ===");
        Console.WriteLine($"Config: {configPath}");
        Console.WriteLine($"Culture: {culture}");
        Console.WriteLine($"File: {filePath}");
        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            Console.WriteLine($"Output: {outputPath}");
        }
        Console.WriteLine();

        // Set culture
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = new CultureInfo(culture);

        // Load configuration
        Console.WriteLine("Loading configuration...");
        var config = FIREConfigration.Load(configPath);
        config.EnsureSupportedConfigurationVersion();
        Console.WriteLine($"Configuration loaded successfully (Version: {config.ConfigurationVersion})");
        Console.WriteLine();

        // Create database path (temporary, needed for catalog initialization)
        var dbPath = Path.Combine(config.DataBasePath ?? ".", config.DataBaseFileName ?? "FIRE.db");

        // Create catalog
        Console.WriteLine("Initializing catalog...");
        using var database = new FIREDatabase(dbPath);
        using var catalog = new FIRECatalog(config, database);
        Console.WriteLine("Catalog initialized.");
        Console.WriteLine();

        // Extract and display metadata
        Console.WriteLine("Extracting metadata...");
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var metadata = catalog.GetAllAvailableMetadata(filePath);
        stopwatch.Stop();
        Console.WriteLine($"Metadata extracted in {stopwatch.Elapsed.TotalMilliseconds:F2} ms.");
        Console.WriteLine($"Total entries found: {metadata.Count}");
        Console.WriteLine();

        // Display metadata summary
        if (metadata.Count > 0)
        {
            var groupedBySource = metadata.GroupBy(m => m.Source).OrderBy(g => g.Key);
            Console.WriteLine("Metadata by Source:");
            foreach (var sourceGroup in groupedBySource)
            {
                Console.WriteLine($"  - {sourceGroup.Key}: {sourceGroup.Count()} entries");
            }
            Console.WriteLine();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: No metadata could be extracted from this file.");
            Console.ResetColor();
            Console.WriteLine();
        }

        // Write to Markdown
        Console.WriteLine("Writing Markdown report...");
        catalog.WriteMetadataToMarkdown(filePath, outputPath);

        var finalOutputPath = outputPath ?? Path.Combine(
            Path.GetDirectoryName(filePath) ?? ".",
            Path.GetFileNameWithoutExtension(filePath) + ".md");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ Markdown report created: {finalOutputPath}");
        Console.ResetColor();
        Console.WriteLine();

        return 0;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ex.Message}");
        Console.ResetColor();
        Console.WriteLine(ex.StackTrace);
        return 1;
    }
}

/// <summary>
/// Truncates a file path to fit within the specified maximum length.
/// </summary>
static string TruncatePath(string path, int maxLength)
{
    if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
        return path;

    // Try to show the end of the path which is usually more relevant
    return "..." + path.Substring(path.Length - maxLength + 3);
}
