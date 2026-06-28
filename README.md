# FIRE — File Reorganizer

<p align="center">
  <img src="docs/FIRE Logo 3.png" alt="FIRE Logo" width="200"/>
</p>

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/10.0)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)

FIRE is a .NET 10 library and command-line application that automatically
reorganizes files based on their embedded metadata (EXIF, XMP, file system
timestamps, and more). It reads metadata via **ExifTool**, stores results in
a local **SQLite** database, evaluates configurable path templates, and
finally copies, moves, or hard-links each file to its computed target location.

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Usage](#usage)
- [Configuration Reference](#configuration-reference)
- [Template Placeholders](#template-placeholders)
- [Keyword Selection](#keyword-selection)
- [Metadata Sources](#metadata-sources)
- [Examples](#examples)
- [Building from Source](#building-from-source)
- [Documentation](#documentation)
- [Third-Party Credits](#third-party-credits)
- [License](#license)

---

## Features

- **Metadata-driven file sorting** — JPEG, RAW, video, and any other
  file type supported by ExifTool
- **Three-step pipeline** — `collect` → `generate` → `execute`
- **SQLite persistence** — inspect and audit every decision before committing
- **Flexible templates** — arbitrary directory hierarchy and filename patterns
  using `{Keyword}` placeholders
- **Multi-keyword fallback** — define ordered lists of EXIF tags; FIRE picks
  the lowest/highest value
- **Sidecar file support** — `.xmp`, `.pp3`, or any companion extension
  follows its primary file automatically
- **String replacements** — normalize camera model names or other metadata
  values globally
- **Culture-aware** — date formatting respects the `--culture` flag

---

## Project Structure

```
FIRE/
├── .github/
│   └── copilot-instructions.md
├── ConfigurationFiles/
│   └── Configuration.yaml          # Example configuration
├── docs/
│   ├── Doxyfile                    # Doxygen 1.17.0 configuration
│   └── mainpage.dox                # Doxygen main page
├── FIRE/
│   ├── FIRE.csproj
│   ├── FIRECatalog.cs              # Core orchestration engine
│   ├── FIREConfigration.cs         # YAML configuration model
│   └── FIREDatabase.cs             # SQLite database abstraction
├── FIRE.Console/
│   ├── FIRE.Console.csproj
│   └── Program.cs                  # CLI entry point
├── FIRE.slnx
├── LICENSE
└── README.md
```

---

## Requirements

| Component | Version |
|-----------|---------|
| .NET SDK | 10.0 or later |
| ExifTool | bundled via [SharpExifTool](https://www.nuget.org/packages/SharpExifTool) NuGet package |
| SQLite | bundled via `Microsoft.EntityFrameworkCore.Sqlite` |
| OS | Windows (NTFS file-ID APIs are used for duplicate detection) |

---

## Installation

### From source

```powershell
git clone https://github.com/Gemelon/FIRE.git
cd FIRE
dotnet build -c Release
```

The console binary is then located at:

```
FIRE.Console\bin\Release\net10.0\FIRE.Console.exe
```

---

## Quick Start

1. Copy `ConfigurationFiles/Configuration.yaml` and adapt the paths.
2. Run the three pipeline steps in order:

```powershell
# Step 1 — scan source directories and extract metadata
FIRE.Console collect --config Configuration.yaml --culture de-DE

# Step 2 — compute target paths from templates
FIRE.Console generate --config Configuration.yaml --culture de-DE

# Step 3 — copy/move/link files to their new locations
FIRE.Console execute --config Configuration.yaml --culture de-DE
```

Optional metadata inspection command:

```powershell
FIRE.Console inspect --config Configuration.yaml --culture en-US --file "D:\Photos\IMG_1234.jpg"
```

---

## Usage

```
FIRE.Console <command> --config <path> --culture <culture> [options]
```

| Argument | Short | Description |
|----------|-------|-------------|
| `--config` | `-c` | Path to the YAML configuration file (**required**) |
| `--culture` | `-l` | Culture code for UI language and date formatting (**required**). Recommended: `en-US`, `de-DE`, `fr-FR`, `fil-PH`. |
| `--no-wrap` | - | Disable line wrapping and clip long console lines. Default is line wrapping enabled. |

### Commands

| Command | Description |
|---------|-------------|
| `collect` | Scans source directories and writes metadata to the database |
| `generate` | Computes target paths and file names from configured templates |
| `execute` | Applies the file operations (Copy / Move / Link) |
| `inspect` | Inspects one file and writes metadata to a Markdown report |

### Inspect command options

| Argument | Short | Description |
|----------|-------|-------------|
| `--file` | `-f` | Source file to inspect (**required** for `inspect`) |
| `--output` | `-o` | Optional path for the generated Markdown report |
| `--copy-path` | - | Copy the generated report path to the clipboard |

---

## Configuration Reference

The configuration is a single YAML file. Every key is case-sensitive.

```yaml
ConfigurationVersion: 1.20       # Must be 1.20

FilesRootPath:                    # One or more source directories
  - D:\Photos\Import
  - D:\Videos\Import

DataBasePath: D:\Photos           # Directory for the SQLite database file
DataBaseFileName: FIRE.db         # Database filename

Action: Copy                      # Default action: Copy | Move | Link
MediaRootPath: D:\Photos\Sorted   # Convenience root used in templates

# Global sorting template (overridden per extension)
SortingPatern: "{MediaRootPath}\\{MetaCreationTime.Year}\\{Make}\\{Model}"
FileNamePatern: "{MetaCreationTime.Year}-{MetaCreationTime.Month}-{MetaCreationTime.Day}_{FileName}"

# Normalize metadata values (e.g. camera model names)
StringReplacements:
  SM-S938B: SM-S938B Galaxy S25 Ultra

FileExtensions:
  .jpg:
	FileType: Image
	FileClass: RegularFile
	Action: Copy
	SortingPatern: "{MediaRootPath}\\{MetaCreationTime.Year}\\{MetaCreationTime.Month}\\{Make}\\{Model}"
	FileNamePatern: "{MetaCreationTime.Year}-{MetaCreationTime.Month}-{MetaCreationTime.Day}_{FileName.Noext}.JPG"
	SidecarFileExtensions:
	  - .xmp
	AvailableKeyWords:
	  Make:
		DataType: STRING
		Source: EXIFTOOL
		KeyWords:
		  - IFD0:Make
		  - DJI:Make
	  MetaCreationTime:
		DataType: DATETIME
		Source: EXIFTOOL
		ValAttribute: LOWEST
		KeyWords:
		  - ExifIFD:DateTimeOriginal
		  - IFD0:DateTime
```

### `FileExtensions` properties

| Property | Type | Description |
|----------|------|-------------|
| `FileType` | string | Logical type label (e.g. `Image`, `Video`) |
| `FileClass` | string | `RegularFile` or `SidecarFile` |
| `Action` | string | `Copy`, `Move`, or `Link` |
| `RootPath` | string | Override root path for this extension |
| `SortingPatern` | string | Target directory template |
| `FileNamePatern` | string | Target filename template |
| `SidecarFileExtensions` | list | Extensions that follow this file type |
| `AvailableKeyWords` | map | Keyword definitions (see below) |

### `AvailableKeyWords` properties

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `DataType` | string | `STRING` | `STRING`, `INT`, `INTEGER`, `DATETIME`, `DATE`, `TIME` |
| `Source` | string | `FILEINFO` | `FILEINFO` or `EXIFTOOL` |
| `ValAttribute` | string | `LOWEST` | `LOWEST` or `HIGHEST` |
| `Default` | string | — | Fallback value when no configured keyword is found. For `DATETIME`, supports a date string (e.g. `2024-12-31 00:00:00`) or `NOW`. |
| `KeyWords` | list | — | Ordered list of metadata tag names to try |

---

## Template Placeholders

Use `{KeywordName}` in `SortingPatern` and `FileNamePatern`.
Sub-properties are accessed with a dot.

| Placeholder | Example | Description |
|-------------|---------|-------------|
| `{FileName}` | `IMG_1234.jpg` | Original filename including extension |
| `{FileName.Noext}` | `IMG_1234` | Filename without extension |
| `{Make}` | `Apple` | Camera manufacturer |
| `{Model}` | `iPhone 15 Pro` | Camera model |
| `{MetaCreationTime.Year}` | `2026` | Four-digit year |
| `{MetaCreationTime.Month}` | `07` | Zero-padded month |
| `{MetaCreationTime.Day}` | `04` | Zero-padded day |
| `{MediaRootPath}` | `D:\Photos\Sorted` | Value of `MediaRootPath` |
| `{Counter:D3}` | `001`, `002`, `003` | Persistent running number per target path with `Dx` formatting; only active when the placeholder is used in the template |

---

## Keyword Selection

When a keyword lists multiple EXIF tags, FIRE queries each tag and picks one
value based on `ValAttribute`:

| ValAttribute | Behaviour |
|---|---|
| `LOWEST` (default) | Smallest numeric value or earliest date/time |
| `HIGHEST` | Largest numeric value or latest date/time |

If none of the listed tags yields a value, `NA` is written and a `[WARN]`
message is printed to the console.

---

## Metadata Sources

| Source | Description |
|--------|-------------|
| `FILEINFO` | File system timestamps: `CreationTime`, `ModificationTime`, `AccessTime` |
| `EXIFTOOL` | Any tag readable by ExifTool (EXIF, XMP, IPTC, QuickTime, ID3, …) |

---

## Sidecar File Handling

FIRE automatically detects and processes sidecar files (e.g., `.xmp`, `.pp3`) alongside their primary files.

### How It Works

1. **Configuration**: Define sidecar extensions in your primary file's configuration:
   ```yaml
   FileExtensions:
     .jpg:
       SidecarFileExtensions:
         - .xmp
         - .pp3
   ```

2. **Collection Phase**: When a primary file is processed, FIRE searches for configured sidecar files in the same directory with the same base name.

3. **Classification**: Each file record is marked as either:
   - `RegularFile` (0): Primary files
   - `SidecarFile` (1): Sidecar files

4. **Path Generation**: Sidecar files automatically inherit the target directory and base filename from their primary file, preserving only their own extension. If the primary template uses `{Counter...}`, the same persistent sequence is retained for the generated target path.

5. **Execution**: Sidecar files are copied/moved alongside their primary files using the global action setting.

### Example

**Source structure:**
```
D:\Import\
  IMG_1234.jpg
  IMG_1234.xmp
```

**After execution:**
```
D:\Sorted\2026\07\Apple\
  2026-07-04_001.jpg
  2026-07-04_001.xmp
```

---

## Examples

### Sort holiday photos by date and camera model

```yaml
SortingPatern: "{MediaRootPath}\\{MetaCreationTime.Year}\\{MetaCreationTime.Month}\\{Make} {Model}"
FileNamePatern: "{MetaCreationTime.Year}{MetaCreationTime.Month}{MetaCreationTime.Day}_{FileName}"
```

Result:
```
D:\Photos\Sorted\
  2026\
	07\
	  Apple iPhone 15 Pro\
		20260704_IMG_1234.jpg
```

### Move DNG raw files alongside their JPEG, keep XMP sidecars

```yaml
FileExtensions:
  .jpg:
	Action: Move
	SidecarFileExtensions:
	  - .xmp
	  - .dng
```

### Normalize camera model names

```yaml
StringReplacements:
  SM-S938B: Galaxy S25 Ultra
  SM-F766B: Galaxy Z Flip7
```

### Running number per target path with `{Counter:D3}`

`{Counter...}` enables numbering only when the placeholder is present in the template. The last used values are stored in the database so numbers are not reused after an application restart.

```yaml
FileNamePatern: "{MetaCreationTime.Year}-{MetaCreationTime.Month}-{MetaCreationTime.Day}_{Counter:D3}.JPG"
```

Example output within the same target path:
- `2026-07-04_001.JPG`
- `2026-07-04_002.JPG`
- `2026-07-04_003.JPG`

Note: For photo and video archives, use `{Counter...}` when a stable sequence must be preserved.
---

## Building from Source

```powershell
# Restore dependencies
dotnet restore FIRE.slnx

# Build (Release)
dotnet build FIRE.slnx -c Release

# Run the console
dotnet run --project FIRE.Console -- collect --config ConfigurationFiles\Configuration.yaml --culture de-DE
```

---

## Documentation

API documentation is generated with **Doxygen 1.17.0**.

```powershell
# Run from the repository root
doxygen docs/Doxyfile
```

The HTML output is written to `docs/html/index.html`.

> **Tip:** Install [Graphviz](https://graphviz.org/) and set `HAVE_DOT = YES`
> in `docs/Doxyfile` to generate class and call graphs.

### Metadata Inspection API

`FIRECatalog` provides two helper methods for metadata discovery and documentation.

#### `GetAllAvailableMetadata(string filePath)`

Returns all metadata entries for one file as tuples:

- `Source` (e.g. `FILEINFO`, `EXIFTOOL`)
- `Key` (metadata key/tag name)
- `Value` (string value)

Behavior:

- Returns an empty list if the file does not exist.
- Continues gracefully if one metadata source fails.
- Useful to discover keys for `AvailableKeyWords`.

Example:

```csharp
var metadata = catalog.GetAllAvailableMetadata(@"D:\Photos\IMG_1234.jpg");
foreach (var (source, key, value) in metadata)
{
    Console.WriteLine($"[{source}] {key}: {value}");
}
```

#### `WriteMetadataToMarkdown(string filePath, string? outputPath = null)`

Creates a Markdown report for one file.

Report contains:

- File information (path, name, size, timestamps)
- Metadata grouped by source
- Key/value tables
- Summary statistics

Behavior:

- If `outputPath` is omitted, a `.md` file is created next to the source file.
- If no metadata is available, the report still contains a warning section.

Example:

```csharp
// Creates "IMG_1234.md" in the same directory
catalog.WriteMetadataToMarkdown(@"D:\Photos\IMG_1234.jpg");

// Custom output location
catalog.WriteMetadataToMarkdown(
    @"D:\Photos\IMG_1234.jpg",
    @"D:\Reports\IMG_1234-metadata.md");
```

---

## Third-Party Credits

This project uses third-party components:

- **ExifTool** by Phil Harvey — https://exiftool.org  
  License: Artistic License 2.0 (alternatively GNU GPL)
- **SharpExifTool** by Junian Triajianto — https://www.nuget.org/packages/SharpExifTool  
  License: MIT License

For details, see [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).

---

## License

Copyright © 2026 by Thomas Stoll.  
Released under the [MIT License](LICENSE).
