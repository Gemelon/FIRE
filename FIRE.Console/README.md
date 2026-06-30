# FIRE.Console

(C) 2026 by Thomas Stoll

## Purpose

`FIRE.Console` is a command-line adapter around the `FIRE` API.
It is intended for:

- API testing
- quick start usage
- reproducible command execution for documentation and troubleshooting

## Non-Goals

`FIRE.Console` is **not** the main product layer and should not be used as an architectural dependency for future UI applications.
A future UI should consume the `FIRE` library directly.

## Commands

- `collect`
- `generate`
- `execute`
- `inspect`
- `diagnose`

## Typical Run

```powershell
dotnet run --project FIRE.Console -- collect --config ConfigurationFiles\Configuration.yaml --culture de-DE
```

## Related Documentation

- API-first project overview: `../README.md`
- Core library details: `../FIRE/README.md`
- Local wiki draft pages: `../docs/wiki-local/` (git-ignored)
