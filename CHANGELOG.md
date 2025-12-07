# Changelog

## [1.0.1] - 2025-12-07

### Added

- Embedded SBOM generation in CI pipeline and included SBOM artifacts in release builds.
- Added CHANGELOG.md file to document changes, SECURITY.md for security policies, and CONTRIBUTING.md for contribution guidelines.

### Changed

- `ArgumentOutOfRangeException` is now thrown when invalid arguments are provided to the Generate method,
  instead of `ArgumentException`.
- Build infrastructure modernization: Moved signing and release to GitHub Actions, retiring Azure DevOps pipelines.
- Pre-release packages are now published to [GitHub Packages](https://github.com/blowdart?tab=packages&repo_name=idunno.PasswordGenerator)
  and [MyGet](https://www.myget.org/feed/blowdart/package/nuget/idunno.Password.Generator) instead of Azure Artifacts.

## [1.0.0](https://github.com/blowdart/idunno.PasswordGenerator/releases/tag/v1.0.0) - 2022-02-23

_First release._
