# Release Checklist

(C) 2026 by Thomas Stoll

This checklist defines the standard process to publish a new FIRE release.

## 1) Prepare Version

- Choose semantic version (`vMAJOR.MINOR.PATCH`).
- Confirm scope (features, fixes, breaking changes).

## 2) Verify Repository State

- Ensure branch is `master` (or intended release branch).
- Ensure working tree is clean.
- Pull latest changes and fetch tags.

## 3) Quality Gate

- Build solution in Release mode.
- Run all tests and confirm all pass.

## 4) Build Release Artifacts

- Publish library output.
- Publish console output.
- Create ZIP artifacts for both outputs.

## 5) Create and Push Tag

- Create annotated tag (example: `v1.0.0`).
- Push branch and tag to `origin`.

## 6) Create GitHub Release

- Open GitHub Releases and create a new release from the tag.
- Use **Generate release notes**.
- Upload ZIP artifacts.
- Publish release.

## 7) Post-Release Validation

- Verify release page assets and notes.
- Confirm tag and release visibility.
- Optionally announce release and update roadmap/changelog.

---

## Suggested Commands

```powershell
git status --short --branch
git fetch origin --tags
dotnet build FIRE.slnx -c Release
dotnet test FIRE.Tests/FIRE.Tests.csproj -c Release
dotnet publish FIRE/FIRE.csproj -c Release -o artifacts/release/FIRE.Library
dotnet publish FIRE.Console/FIRE.Console.csproj -c Release -o artifacts/release/FIRE.Console
Compress-Archive -Path artifacts/release/FIRE.Library/* -DestinationPath artifacts/release-zips/FIRE.Library.vX.Y.Z.zip -Force
Compress-Archive -Path artifacts/release/FIRE.Console/* -DestinationPath artifacts/release-zips/FIRE.Console.vX.Y.Z.zip -Force
git tag -a vX.Y.Z -m "FIRE release vX.Y.Z"
git push origin master
git push origin vX.Y.Z
```
