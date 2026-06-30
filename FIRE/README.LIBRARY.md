# FIRE Library

(C) 2026 by Thomas Stoll

## Purpose

`FIRE` is the core API library of this repository.
It provides the metadata-driven catalog pipeline and is the primary integration target for other applications.

## Scope

- Configuration loading and validation (`FIREConfigration`)
- Metadata extraction abstraction (`IMetadataSource`, source registry)
- SQLite persistence and record lifecycle (`FIREDatabase`)
- Pipeline orchestration (`FIRECatalog`)
- Progress/state notifications for host applications (`FIRECatalogProgress`)
- Localized API messages (`Localization/ApiLocalizer` + resource files)

## Integration Guidance

UI applications should reference this library directly and subscribe to `FIRECatalog` progress/state updates.
Do not use `FIRE.Console` as a runtime dependency for UI integration.

## Pipeline Overview

1. **Collect**: scan files and persist metadata.
2. **Generate**: resolve templates and compute target paths.
3. **Execute**: perform configured file operations (Copy/Move/Link).

## Related Documentation

- Root project documentation: `../README.md`
- Doxygen source page: `../docs/mainpage.dox`
- Local wiki draft pages: `../docs/wiki-local/`
