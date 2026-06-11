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
    private bool _disposed;

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
        _database.Clear();

        foreach (var rootPath in _configuration.FilesRootPath)
        {
            if (!Directory.Exists(rootPath)) continue;
            CollectFromDirectory(rootPath, progressCallback);
        }
        _database.Save();
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

        foreach (var record in _database)
        {
            if (string.IsNullOrWhiteSpace(record.SourceFilePath)) continue;

            progressCallback?.Invoke(record.SourceFilePath);

            var extension = Path.GetExtension(record.SourceFilePath);
            if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig)) continue;

            var metadataLookup = record.FileMetaDatas.ToDictionary(
                m => m.Key ?? string.Empty,
                m => m.Value ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);

            var sortingPattern = extConfig.SortingPatern ?? _configuration.SortingPatern ?? string.Empty;
            var fileNamePattern = extConfig.FileNamePatern ?? _configuration.FileNamePatern ?? string.Empty;

            var targetDirectory = ParseTemplate(sortingPattern, metadataLookup, record.SourceFilePath);
            var targetFileName = ParseTemplate(fileNamePattern, metadataLookup, record.SourceFilePath);

            if (!string.IsNullOrWhiteSpace(targetDirectory) && !string.IsNullOrWhiteSpace(targetFileName))
                record.TargetFilePath = Path.Combine(targetDirectory, targetFileName);
        }
        _database.Save();
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

        foreach (var record in _database)
        {
            if (string.IsNullOrWhiteSpace(record.SourceFilePath) || string.IsNullOrWhiteSpace(record.TargetFilePath)) continue;
            if (!File.Exists(record.SourceFilePath)) continue;

            progressCallback?.Invoke(record.SourceFilePath);

            var extension = Path.GetExtension(record.SourceFilePath);
            if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig)) continue;

            var action = extConfig.Action ?? _configuration.Action ?? "Copy";
            ExecuteAction(action, record.SourceFilePath, record.TargetFilePath);
        }
        _database.Save();
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
        _database.Dispose();
        _disposed = true;
    }

    private void CollectFromDirectory(string directoryPath, Action<string>? progressCallback = null)
    {
        try
        {
            foreach (var filePath in Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories))
            {
                progressCallback?.Invoke(filePath);
                ProcessFile(filePath);
            }
        }
        catch { }
    }

    private void ProcessFile(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        if (!_configuration.FileExtensions.TryGetValue(extension, out var extConfig)) return;

        var fileIdInfo = GetFileIdInfo(filePath);
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
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, "NA", keywordConfig, sourceName));
                continue;
            }

            if (keywordConfig.KeyWords.Count == 0)
            {
                LogMetadataWarning(filePath, keywordName, "no keywords configured");
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, "NA", keywordConfig, sourceName));
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
                record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, "NA", keywordConfig, sourceName));
                continue;
            }

            var selectedValue = SelectValue(matchingValues, valAttribute, keywordConfig.DataType, filePath, keywordName);
            record.FileMetaDatas.Add(CreateMetadataEntry(keywordName, selectedValue, keywordConfig, sourceName));
        }

        _database.Add(record);
    }

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

    private static string SelectValue(List<string> values, string valAttribute, string dataType, string filePath, string keywordName)
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

    private static string SelectExtremeValue(List<string> values, string normalizedDataType, bool highest, string filePath, string keywordName)
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

    private static void LogMetadataWarning(string filePath, string keywordName, string reason)
    {
        Console.WriteLine($"[WARN] {Path.GetFileName(filePath)} / {keywordName}: {reason}");
    }

    private string ParseTemplate(string template, Dictionary<string, string> metadata, string sourceFilePath)
    {
        if (string.IsNullOrWhiteSpace(template)) return string.Empty;

        var result = template;
        var placeholderPattern = new Regex(@"\{(?<key>[^}]+)\}", RegexOptions.Compiled);

        result = placeholderPattern.Replace(result, match =>
            ResolvePlaceholder(match.Groups["key"].Value, metadata, sourceFilePath));

        foreach (var replacement in _configuration.StringReplacements)
            result = result.Replace(replacement.Key, replacement.Value, StringComparison.OrdinalIgnoreCase);

        return result;
    }

    private string ResolvePlaceholder(string key, Dictionary<string, string> metadata, string sourceFilePath)
    {
        var parts = key.Split('.');
        var baseName = parts[0];
        var property = parts.Length > 1 ? parts[1] : null;

        if (baseName.Equals("FileName", StringComparison.OrdinalIgnoreCase))
        {
            if (property != null && property.Equals("Noext", StringComparison.OrdinalIgnoreCase))
                return Path.GetFileNameWithoutExtension(sourceFilePath);
            return Path.GetFileName(sourceFilePath);
        }

        if (baseName.Equals("MediaRootPath", StringComparison.OrdinalIgnoreCase))
            return _configuration.MediaRootPath;

        if (baseName.Equals("RootPath", StringComparison.OrdinalIgnoreCase))
            return ParseTemplate(_configuration.RootPath, metadata, sourceFilePath);

        if (!metadata.TryGetValue(baseName, out var value) || string.IsNullOrWhiteSpace(value))
            return "Unknown";

        if (property == null) return value;

        // Zeile 548 – vorher: DateTime.TryParse (schlägt bei EXIF-Dates fehl)
        if (TryNormalizeDateTime(value, out var normalizedValue) &&
            DateTime.TryParseExact(normalizedValue, "yyyy:MM:dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
        {
            return property.ToUpperInvariant() switch
            {
                "YEAR" => dateTime.Year.ToString(CultureInfo.InvariantCulture),
                "MONTH" => dateTime.Month.ToString("D2", CultureInfo.InvariantCulture),
                "DAY" => dateTime.Day.ToString("D2", CultureInfo.InvariantCulture),
                _ => normalizedValue   // gibt immer yyyy:MM:dd HH:mm:ss zurück
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

        return value;
    }

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

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(FIRECatalog));
    }
}