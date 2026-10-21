# .NET Upgrade Plan: Upgrade to Latest LTS (net10.0)

## Summary

| | |
|---|---|
| **Source version** | .NET Framework 4.7.2 (NYCJobsWeb), .NET Framework 4.5 (DataLoader / AzureSearchBackupRestore) |
| **Target version** | .NET 10.0 (net10.0) — latest LTS |
| **SDK-style conversion required** | Yes (both projects use legacy non-SDK-style `.csproj`) |

## Projects in Solution

| Project | Path | Current TFM | Output Type | Notes |
|---|---|---|---|---|
| NYCJobsWeb | `NYCJobsWeb\NYCJobsWeb.csproj` | v4.7.2 | Library (ASP.NET MVC 5 web app) | Uses `System.Web.Mvc`, Global.asax, Web.config, `packages.config` with legacy Azure SDK references (`Azure.Core`, `Azure.Search.Documents`, `Microsoft.Rest.ClientRuntime.*`). Requires migration to ASP.NET Core MVC patterns. |
| AzureSearchBackupRestore (DataLoader) | `DataLoader\DataLoader\DataLoader.csproj` | v4.5 | Exe (console app) | Uses `Newtonsoft.Json`, `App.config`, `packages.config`. .NET Framework 4.5 does not support `netstandard2.0` (Azure SDK incompatible). |

## Why an Upgrade Is Needed

- **Azure SDK compatibility**: `DataLoader` targets .NET Framework 4.5, which is below the 4.6.2 minimum required for `netstandard2.0`/modern Azure SDK packages.
- **EOL / outdated framework**: Both projects target legacy, non-SDK-style .NET Framework TFMs that are out of mainstream support and lack modern tooling, performance, and security improvements.
- **User request**: The user explicitly requested upgrading to the latest LTS version.

## High-Level Upgrade Scope

1. Convert both `.csproj` files from the legacy MSBuild project format to SDK-style projects.
2. Update `TargetFramework` to `net10.0` for both projects.
3. Replace `packages.config` references with `PackageReference` entries, updating NuGet packages (`Newtonsoft.Json`, `Azure.Core`, `Azure.Search.Documents`, `Microsoft.Rest.ClientRuntime.*`, etc.) to their latest compatible versions.
4. Migrate `NYCJobsWeb` from ASP.NET MVC 5 (`System.Web.Mvc`, Global.asax, `Web.config`) to ASP.NET Core MVC conventions (startup/hosting model, routing, configuration via `appsettings.json`), preserving existing controllers, views, and models.
5. Update `DataLoader`/`AzureSearchBackupRestore` console app project settings and resolve any API breaking changes from the Azure SDK and `Newtonsoft.Json` upgrades.
6. Resolve any compilation or API incompatibilities introduced by the framework and package upgrades.
7. Validate solution build and update/verify any existing tests pass after the upgrade.

## Out of Scope

- Feature additions, refactoring unrelated to the upgrade, containerization, or deployment changes.
