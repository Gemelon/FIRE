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

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FIRE;

/// <summary>
/// Represents the root configuration document loaded from the FIRE configuration YAML file.
/// 
/// <c>FIREConfigration</c> is the primary configuration model for the FIRE application,
/// serialized from and deserialized to YAML via YamlDotNet. It defines all the settings
/// required to control the three-step file processing pipeline (collect, generate, execute).
/// </summary>
/// <remarks>
/// <para>
/// <strong>Configuration Version:</strong>
/// The configuration includes a <see cref="ConfigurationVersion"/> that must match
/// <see cref="SupportedConfigurationVersion"/> (1.00). This ensures forward/backward compatibility
/// and prevents accidental use of incompatible configuration files.
/// </para>
/// 
/// <para>
/// <strong>File Discovery and Storage:</strong>
/// <see cref="FilesRootPath"/> lists the directory roots to scan for source files during
/// the <c>collect</c> phase. <see cref="DataBasePath"/> and <see cref="DataBaseFileName"/>
/// specify the location of the SQLite database file where records are persisted.
/// </para>
/// 
/// <para>
/// <strong>File Operations and Path Templates:</strong>
/// <see cref="Action"/> specifies the file operation (Copy, Move, or Link).
/// <see cref="RootPath"/> is the base directory for output files.
/// <see cref="SortingPatern"/> and <see cref="FileNamePatern"/> are path templates
/// containing placeholders like <c>{Make}</c>, <c>{Model}</c>, <c>{DateTimeOriginal}</c>
/// that are resolved using collected metadata during the <c>generate</c> phase.
/// </para>
/// 
/// <para>
/// <strong>Text Substitution and Extension Overrides:</strong>
/// <see cref="StringReplacements"/> defines global string replacements applied to all resolved paths.
/// <see cref="FileExtensions"/> provides per-extension configuration overrides for specific file types,
/// allowing different sorting patterns, keyword sets, and file operations for (e.g.) images vs. videos.
/// </para>
/// 
/// <para>
/// <strong>Deserialization and Normalization:</strong>
/// The <see cref="Parse"/> and <see cref="Load"/> methods deserialize YAML using YamlDotNet.
/// The <c>Normalize</c> method fills in defaults for all properties, ensuring predictable
/// behavior even if some fields are omitted from the YAML file.
/// </para>
/// </remarks>
/// <example>
/// Loading and validating a configuration:
/// <code>
/// var config = FIREConfigration.Load("Configuration.yaml");
/// config.EnsureSupportedConfigurationVersion();
/// 
/// Console.WriteLine($"Root path: {config.RootPath}");
/// Console.WriteLine($"Sorting pattern: {config.SortingPatern}");
/// Console.WriteLine($"File extensions configured: {config.FileExtensions.Count}");
/// </code>
/// </example>
public sealed class FIREConfigration
{
    /// <summary>
    /// The semantic version of the configuration format currently supported by this build.
    /// </summary>
    /// <remarks>
    /// Configuration files must have <see cref="ConfigurationVersion"/> equal to this value.
    /// If the versions do not match, <see cref="EnsureSupportedConfigurationVersion"/> will throw.
    /// </remarks>
    public const decimal SupportedConfigurationVersion = 1.00m;

    /// <summary>
    /// Gets or sets the configuration format version.
    /// </summary>
    /// <remarks>
    /// This value is loaded from the YAML field <c>ConfigurationVersion</c>.
    /// It must match <see cref="SupportedConfigurationVersion"/> or an exception will be thrown
    /// when <see cref="EnsureSupportedConfigurationVersion"/> is called.
    /// </remarks>
    [YamlMember(Alias = "ConfigurationVersion")]
    public decimal ConfigurationVersion { get; set; } = SupportedConfigurationVersion;

    /// <summary>
    /// Gets or sets the list of root directories to scan for source files during collection.
    /// </summary>
    /// <remarks>
    /// Each path in this list is recursively scanned to discover all qualifying files
    /// that match the configured file extension rules during the <c>collect</c> phase.
    /// Paths can use environment variables and relative paths; these should be resolved
    /// before passing to the file discovery logic.
    /// </remarks>
    [YamlMember(Alias = "FilesRootPath")]
    public List<string> FilesRootPath { get; set; } = [];

    /// <summary>
    /// Gets or sets the directory path where the SQLite database file is stored.
    /// </summary>
    /// <remarks>
    /// The complete database file path is constructed as
    /// <c>Path.Combine(DataBasePath, DataBaseFileName)</c>.
    /// If <see cref="DataBasePath"/> is empty or null, the database file is created
    /// in the application's working directory.
    /// </remarks>
    [YamlMember(Alias = "DataBasePath")]
    public string DataBasePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the SQLite database file (e.g., "fire.db").
    /// </summary>
    /// <remarks>
    /// This is combined with <see cref="DataBasePath"/> to form the full database file path.
    /// If not specified in the configuration, it defaults to an empty string.
    /// </remarks>
    [YamlMember(Alias = "DataBaseFileName")]
    public string DataBaseFileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default file operation for all files (Copy, Move, or Link).
    /// </summary>
    /// <remarks>
    /// Valid values are "Copy", "Move", and "Link". This is the default action
    /// applied during the <c>execute</c> phase for files that do not have an
    /// extension-specific override in <see cref="FileExtensions"/>.
    /// Defaults to "Copy" if not specified.
    /// </remarks>
    [YamlMember(Alias = "Action")]
    public string Action { get; set; } = "Copy";

    /// <summary>
    /// Gets or sets the media root path variable used in path templates.
    /// </summary>
    /// <remarks>
    /// This is a convenience placeholder that can be referenced in path templates
    /// as <c>{MediaRootPath}</c> to avoid hardcoding the base directory.
    /// Defaults to "CC" if not specified.
    /// </remarks>
    [YamlMember(Alias = "MediaRootPath")]
    public string MediaRootPath { get; set; } = "CC";

    /// <summary>
    /// Gets or sets the base root path for output directories (may contain placeholders).
    /// </summary>
    /// <remarks>
    /// This is a template that can reference other configuration placeholders (e.g., <c>{MediaRootPath}</c>).
    /// The template is resolved during the <c>generate</c> phase before being combined with
    /// the sorting and file name patterns.
    /// Defaults to "{MediaRootPath}" if not specified.
    /// </remarks>
    [YamlMember(Alias = "RootPath")]
    public string RootPath { get; set; } = "{MediaRootPath}";

    /// <summary>
    /// Gets or sets the directory sorting path pattern (may contain metadata placeholders).
    /// </summary>
    /// <remarks>
    /// This is a template that defines the directory structure under <see cref="RootPath"/>.
    /// Placeholders like <c>{Year}</c>, <c>{Make}</c>, <c>{CreationTime}</c> are replaced
    /// with resolved metadata values during the <c>generate</c> phase.
    /// Example: <c>{DateTimeOriginal:yyyy}/{DateTimeOriginal:MM}/{Make}</c>
    /// </remarks>
    [YamlMember(Alias = "SortingPatern")]
    public string SortingPatern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name pattern (may contain metadata placeholders).
    /// </summary>
    /// <remarks>
    /// This template defines the file name (without directory path) for output files.
    /// Placeholders are resolved similarly to <see cref="SortingPatern"/>.
    /// Example: <c>{DateTimeOriginal:yyyy-MM-dd_HHmmss}_{Make}_{Model}</c>
    /// </remarks>
    [YamlMember(Alias = "FileNamePatern")]
    public string FileNamePatern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a dictionary of global string replacements applied to all resolved paths.
    /// </summary>
    /// <remarks>
    /// After all metadata placeholders are resolved and paths are constructed,
    /// each key in this dictionary is replaced with its corresponding value.
    /// This is useful for sanitizing or normalizing folder names (e.g., replacing
    /// spaces with underscores or removing invalid characters).
    /// Case-insensitive comparison is used for keys.
    /// </remarks>
    [YamlMember(Alias = "StringReplacements")]
    public Dictionary<string, string> StringReplacements { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets or sets a dictionary of file extension-specific configurations.
    /// </summary>
    /// <remarks>
    /// Keys are file extensions (e.g., ".jpg", ".mp4", ".raw") and values are
    /// <see cref="FileExtensionConfiguration"/> objects that override the root configuration
    /// for files with that extension. This allows different sorting patterns, keyword sets,
    /// and file operations for different file types.
    /// Case-insensitive comparison is used for keys.
    /// </remarks>
    [YamlMember(Alias = "FileExtensions")]
    public Dictionary<string, FileExtensionConfiguration> FileExtensions { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Loads a FIRE configuration from a YAML file on disk.
    /// </summary>
    /// <param name="filePath">The absolute or relative path to the configuration YAML file.</param>
    /// <returns>A deserialized and normalized <see cref="FIREConfigration"/> instance.</returns>
    /// <remarks>
    /// This method reads the entire file into memory, parses it, and returns a configuration instance.
    /// All properties are normalized to default values if missing from the file.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="filePath"/> is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the specified file does not exist.
    /// </exception>
    /// <exception cref="YamlException">
    /// Thrown if the YAML file is malformed and cannot be parsed.
    /// </exception>
    public static FIREConfigration Load(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        return Parse(File.ReadAllText(filePath));
    }

    /// <summary>
    /// Parses YAML content into a <see cref="FIREConfigration"/> instance.
    /// </summary>
    /// <param name="yaml">The YAML content string.</param>
    /// <returns>A deserialized and normalized <see cref="FIREConfigration"/> instance.</returns>
    /// <remarks>
    /// This method uses YamlDotNet's deserializer configured with null naming convention
    /// to map YAML properties exactly as written. Unknown properties are ignored.
    /// All properties are normalized to default values if missing.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="yaml"/> is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="YamlException">
    /// Thrown if the YAML content is malformed and cannot be parsed.
    /// </exception>
    public static FIREConfigration Parse(string yaml)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(yaml);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(NullNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var configuration = deserializer.Deserialize<FIREConfigration>(yaml) ?? new FIREConfigration();
        configuration.Normalize();
        return configuration;
    }

    /// <summary>
    /// Checks whether the loaded configuration version matches the supported version.
    /// </summary>
    /// <returns>true if the versions match; otherwise, false.</returns>
    /// <remarks>
    /// This is a non-throwing check. Use <see cref="EnsureSupportedConfigurationVersion"/> 
    /// if you want an exception to be thrown on version mismatch.
    /// </remarks>
    public bool IsConfigurationVersionSupported() => ConfigurationVersion == SupportedConfigurationVersion;

    /// <summary>
    /// Throws an exception if the configuration version is not supported.
    /// </summary>
    /// <remarks>
    /// This method should be called after loading or parsing a configuration to ensure
    /// that the version is compatible with the current application build.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <see cref="ConfigurationVersion"/> does not equal <see cref="SupportedConfigurationVersion"/>.
    /// </exception>
    public void EnsureSupportedConfigurationVersion()
    {
        if (!IsConfigurationVersionSupported())
        {
            throw new InvalidOperationException(
                $"Unsupported ConfigurationVersion '{ConfigurationVersion}'. Expected '{SupportedConfigurationVersion}'.");
        }
    }

    private void Normalize()
    {
        DataBasePath ??= string.Empty;
        DataBaseFileName ??= string.Empty;
        Action = string.IsNullOrWhiteSpace(Action) ? "Copy" : Action;
        MediaRootPath = string.IsNullOrWhiteSpace(MediaRootPath) ? "CC" : MediaRootPath;
        RootPath = string.IsNullOrWhiteSpace(RootPath) ? "{MediaRootPath}" : RootPath;
        SortingPatern ??= string.Empty;
        FileNamePatern ??= string.Empty;
        StringReplacements ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        FileExtensions ??= new Dictionary<string, FileExtensionConfiguration>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in FileExtensions.Values)
        {
            item.Normalize();
        }
    }
}

/// <summary>
/// Represents configuration for a specific file extension within the FIRE configuration.
/// 
/// <c>FileExtensionConfiguration</c> allows per-extension overrides for sorting patterns,
/// file name patterns, file operations, and available metadata keywords. This enables
/// different handling for different file types (e.g., images, videos, documents).
/// </summary>
/// <remarks>
/// <para>
/// <strong>File Classification:</strong>
/// <see cref="FileType"/> and <see cref="FileClass"/> are descriptive labels that categorize
/// the file (e.g., FileType="Photo", FileClass="Image"). These are meant for documentation
/// and are not used programmatically by FIRE.
/// </para>
/// 
/// <para>
/// <strong>Extension-Specific Overrides:</strong>
/// All properties (Action, RootPath, SortingPatern, FileNamePatern) override the root
/// configuration values if set. If a property is null or whitespace, the root configuration
/// value is used instead. This per-extension mechanism allows fine-grained control over
/// the file processing pipeline for different file types.
/// </para>
/// 
/// <para>
/// <strong>Sidecar Files:</strong>
/// <see cref="SidecarFileExtensions"/> lists additional file extensions that are considered
/// "sidecars" to the main file. For example, ".jpg.xmp" files are sidecar metadata files
/// associated with the primary ".jpg" file. During processing, sidecar files are handled
/// together with their primary files.
/// </para>
/// 
/// <para>
/// <strong>Keyword Configuration:</strong>
/// <see cref="AvailableKeyWords"/> is a dictionary mapping keyword names to their configuration
/// (data type, metadata source, value selection strategy). These keywords are available
/// for use in path templates specific to this file extension.
/// </para>
/// 
/// <para>
/// <strong>Normalization:</strong>
/// The internal <c>Normalize</c> method fills in defaults for all properties, ensuring
/// consistent behavior. All property defaults mirror those in <see cref="FIREConfigration"/>.
/// </para>
/// </remarks>
/// <example>
/// Configuring JPEG image handling with specific sorting:
/// <code>
/// var jpegConfig = new FileExtensionConfiguration
/// {
///     FileType = "Photo",
///     FileClass = "Image",
///     Action = "Copy",
///     RootPath = "{MediaRootPath}/Photos",
///     SortingPatern = "{DateTimeOriginal:yyyy}/{DateTimeOriginal:MM}/{Make}",
///     FileNamePatern = "{DateTimeOriginal:yyyy-MM-dd_HHmmss}",
///     AvailableKeyWords = new Dictionary&lt;string, AvailableKeywordConfiguration&gt;
///     {
///         ["Make"] = new() { Source = "EXIFTOOL", DataType = "STRING" }
///     }
/// };
/// </code>
/// </example>
public sealed class FileExtensionConfiguration
{
    /// <summary>
    /// Gets or sets a descriptive file type label (e.g., "Photo", "Video", "Document").
    /// </summary>
    /// <remarks>
    /// This is a human-readable classification used for documentation purposes.
    /// It does not affect the processing logic and defaults to an empty string if not specified.
    /// </remarks>
    [YamlMember(Alias = "FileType")]
    public string FileType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a descriptive file class label (e.g., "Image", "Video", "Audio").
    /// </summary>
    /// <remarks>
    /// Similar to <see cref="FileType"/>, this is primarily for documentation.
    /// It defaults to an empty string if not specified.
    /// </remarks>
    [YamlMember(Alias = "FileClass")]
    public string FileClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file operation override for this extension.
    /// </summary>
    /// <remarks>
    /// Valid values: "Copy", "Move", or "Link". If null or whitespace, the root configuration
    /// <see cref="FIREConfigration.Action"/> is used. Otherwise, this value overrides the root.
    /// Defaults to "Copy" after normalization.
    /// </remarks>
    [YamlMember(Alias = "Action")]
    public string Action { get; set; } = "Copy";

    /// <summary>
    /// Gets or sets the root path override for this extension.
    /// </summary>
    /// <remarks>
    /// This can contain placeholders like <c>{MediaRootPath}</c>. If null or whitespace,
    /// the root configuration <see cref="FIREConfigration.RootPath"/> is used.
    /// Defaults to "{MediaRootPath}" after normalization.
    /// </remarks>
    [YamlMember(Alias = "RootPath")]
    public string RootPath { get; set; } = "{MediaRootPath}";

    /// <summary>
    /// Gets or sets the sorting pattern override for this extension.
    /// </summary>
    /// <remarks>
    /// This template defines the directory structure for files with this extension.
    /// If null or whitespace, the root configuration <see cref="FIREConfigration.SortingPatern"/> is used.
    /// Defaults to an empty string after normalization.
    /// </remarks>
    [YamlMember(Alias = "SortingPatern")]
    public string SortingPatern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name pattern override for this extension.
    /// </summary>
    /// <remarks>
    /// This template defines the output file name for files with this extension.
    /// If null or whitespace, the root configuration <see cref="FIREConfigration.FileNamePatern"/> is used.
    /// Defaults to an empty string after normalization.
    /// </remarks>
    [YamlMember(Alias = "FileNamePatern")]
    public string FileNamePatern { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a list of file extensions that are considered sidecar files for this extension.
    /// </summary>
    /// <remarks>
    /// Sidecar files typically contain metadata or auxiliary information associated with a primary file.
    /// For example, ".jpg.xmp" files are XMP metadata sidecars for ".jpg" files.
    /// When a primary file is processed, its sidecars are handled together.
    /// Defaults to an empty list if not specified.
    /// </remarks>
    [YamlMember(Alias = "SidecarFileExtensions")]
    public List<string> SidecarFileExtensions { get; set; } = [];

    /// <summary>
    /// Gets or sets a dictionary of available metadata keywords for this extension.
    /// </summary>
    /// <remarks>
    /// Keys are keyword names (e.g., "Make", "Model", "DateTimeOriginal") and values are
    /// <see cref="AvailableKeywordConfiguration"/> objects that specify where and how to extract the metadata.
    /// These keywords can be used as placeholders in <see cref="SortingPatern"/> and <see cref="FileNamePatern"/>.
    /// Case-insensitive comparison is used for keys.
    /// Defaults to an empty dictionary if not specified.
    /// </remarks>
    [YamlMember(Alias = "AvailableKeyWords")]
    public Dictionary<string, AvailableKeywordConfiguration> AvailableKeyWords { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Normalizes all properties to their defaults if not specified.
    /// </summary>
    /// <remarks>
    /// This internal method is called automatically after deserialization to ensure
    /// that all properties have valid, predictable values. It also recursively normalizes
    /// all keyword configurations in <see cref="AvailableKeyWords"/>.
    /// </remarks>
    internal void Normalize()
    {
        FileType ??= string.Empty;
        FileClass ??= string.Empty;
        Action = string.IsNullOrWhiteSpace(Action) ? "Copy" : Action;
        RootPath = string.IsNullOrWhiteSpace(RootPath) ? "{MediaRootPath}" : RootPath;
        SortingPatern ??= string.Empty;
        FileNamePatern ??= string.Empty;
        SidecarFileExtensions ??= [];
        AvailableKeyWords ??= new Dictionary<string, AvailableKeywordConfiguration>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in AvailableKeyWords.Values)
        {
            item.Normalize();
        }
    }
}

/// <summary>
/// Represents keyword metadata configuration for use within a file extension context.
/// 
/// <c>AvailableKeywordConfiguration</c> defines how a specific keyword (metadata key) should
/// be extracted and selected for use in path templates. It specifies the metadata source,
/// data type, and value selection strategy (e.g., select the lowest, highest, or first value
/// when multiple values are available).
/// </summary>
/// <remarks>
/// <para>
/// <strong>Metadata Source:</strong>
/// The <see cref="Source"/> field indicates where the metadata should be extracted:
/// <list type="bullet">
/// <item><description><c>FILEINFO</c>: Extract from file system properties (size, creation time, etc.)</description></item>
/// <item><description><c>EXIFTOOL</c>: Extract from embedded EXIF/IPTC/XMP metadata using ExifTool</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Data Type and Value Selection:</strong>
/// The <see cref="DataType"/> field indicates the expected data type (e.g., "STRING", "INT", "DATETIME").
/// The <see cref="ValAttribute"/> field specifies the selection strategy when multiple values are found:
/// <list type="bullet">
/// <item><description><c>LOWEST</c>: Select the minimum value</description></item>
/// <item><description><c>HIGHEST</c>: Select the maximum value</description></item>
/// <item><description><c>FIRST</c>: Select the first value encountered</description></item>
/// </list>
/// </para>
/// 
/// <para>
/// <strong>Keyword Names:</strong>
/// The <see cref="KeyWords"/> list contains the actual metadata field names to query from the source.
/// For EXIFTOOL, these are EXIF tag names (e.g., "Make", "Model", "DateTimeOriginal").
/// For FILEINFO, these are system property names (e.g., "CreationTime", "LastWriteTime").
/// When multiple keywords are listed, the first matching value is used.
/// </para>
/// 
/// <para>
/// <strong>Normalization:</strong>
/// The <c>Normalize</c> method fills in defaults after deserialization to ensure
/// consistent behavior even if properties are omitted from the YAML file.
/// </para>
/// </remarks>
/// <example>
/// Configuring a keyword to extract the camera make from EXIF:
/// <code>
/// var makeKeyword = new AvailableKeywordConfiguration
/// {
///     DataType = "STRING",
///     Source = "EXIFTOOL",
///     ValAttribute = "FIRST",
///     KeyWords = new List&lt;string&gt; { "Make" }
/// };
/// </code>
/// 
/// Another example: extracting the earliest datetime:
/// <code>
/// var dateTimeKeyword = new AvailableKeywordConfiguration
/// {
///     DataType = "DATETIME",
///     Source = "EXIFTOOL",
///     ValAttribute = "LOWEST",
///     KeyWords = new List&lt;string&gt; { "DateTimeOriginal", "DateTimeDigitized", "DateTime" }
/// };
/// </code>
/// </example>
public sealed class AvailableKeywordConfiguration
{
    /// <summary>
    /// Gets or sets the data type of the metadata value.
    /// </summary>
    /// <remarks>
    /// Valid values include "STRING", "INT", "INTEGER", "DATETIME", "DATE", "TIME", etc.
    /// This is used to interpret and parse the metadata value correctly during path template
    /// resolution and value selection. Defaults to "STRING" if not specified.
    /// </remarks>
    [YamlMember(Alias = "DataType")]
    public string DataType { get; set; } = "STRING";

    /// <summary>
    /// Gets or sets the metadata source for this keyword.
    /// </summary>
    /// <remarks>
    /// Valid values:
    /// <list type="bullet">
    /// <item><description><c>FILEINFO</c>: File system properties (default)</description></item>
    /// <item><description><c>EXIFTOOL</c>: Embedded file metadata</description></item>
    /// </list>
    /// Defaults to "FILEINFO" if not specified.
    /// </remarks>
    [YamlMember(Alias = "Source")]
    public string Source { get; set; } = "FILEINFO";

    /// <summary>
    /// Gets or sets the value selection attribute that determines which value to use when multiple are found.
    /// </summary>
    /// <remarks>
    /// Valid values:
    /// <list type="bullet">
    /// <item><description><c>LOWEST</c>: Select the minimum value (for numeric/date types)</description></item>
    /// <item><description><c>HIGHEST</c>: Select the maximum value</description></item>
    /// <item><description><c>FIRST</c>: Select the first value encountered</description></item>
    /// </list>
    /// Defaults to "LOWEST" if not specified.
    /// </remarks>
    [YamlMember(Alias = "ValAttribute")]
    public string ValAttribute { get; set; } = "LOWEST";

    /// <summary>
    /// Gets or sets the list of metadata field names to query from the metadata source.
    /// </summary>
    /// <remarks>
    /// For EXIFTOOL, these are EXIF/IPTC/XMP tag names (e.g., "Make", "Model", "DateTimeOriginal").
    /// For FILEINFO, these are system property names (e.g., "CreationTime", "LastWriteTime").
    /// When querying, the first non-empty value from this list is used.
    /// Defaults to an empty list if not specified.
    /// </remarks>
    [YamlMember(Alias = "KeyWords")]
    public List<string> KeyWords { get; set; } = [];

    /// <summary>
    /// Normalizes all properties to their defaults if not specified.
    /// </summary>
    /// <remarks>
    /// This internal method is called automatically after deserialization to ensure
    /// that all properties have valid, predictable values.
    /// </remarks>
    internal void Normalize()
    {
        DataType = string.IsNullOrWhiteSpace(DataType) ? "STRING" : DataType;
        Source = string.IsNullOrWhiteSpace(Source) ? "FILEINFO" : Source;
        ValAttribute = string.IsNullOrWhiteSpace(ValAttribute) ? "LOWEST" : ValAttribute;
        KeyWords ??= [];
    }
}
