# .NET Upgrade Plan: JobSearch

## Overview

Upgrade all projects in the **JobSearch** solution from .NET Framework to **.NET 10.0** (latest LTS).

## Source and Target Versions

| Project | Source Framework | Target Framework |
|---------|-----------------|-----------------|
| DataLoader (`DataLoader\DataLoader\DataLoader.csproj`) | .NET Framework 4.5 | .NET 10.0 (`net10.0`) |
| NYCJobsWeb (`NYCJobsWeb\NYCJobsWeb.csproj`) | .NET Framework 4.7.2 | .NET 10.0 (`net10.0`) |

## Upgrade Rationale

- **DataLoader**: Targets .NET Framework 4.5, which is end-of-life (EOL) and does not meet the minimum .NET Framework 4.6.2 requirement for Azure SDK (`Azure.*`) compatibility. Upgrading ensures long-term support and modern Azure SDK access.
- **NYCJobsWeb**: Targets .NET Framework 4.7.2, which is EOL. Upgrading to .NET 10.0 LTS provides security support, performance improvements, and access to modern APIs.
- The user explicitly requested an upgrade to the latest LTS version.

## Upgrade Scope

Both projects use the legacy (non-SDK-style) `.csproj` format and will require:

1. **SDK-style project file conversion** — Migrate from the legacy MSBuild project format to the modern SDK-style `.csproj` format.
2. **Target Framework Moniker (TFM) update** — Change `<TargetFrameworkVersion>` to `<TargetFramework>net10.0</TargetFramework>`.
3. **NuGet package updates** — Update all NuGet dependencies to versions compatible with `net10.0`, removing `packages.config` in favour of PackageReference.
4. **API compatibility fixes** — Address any breaking API changes between .NET Framework and .NET 10.0 (e.g., `System.Web` removal, ASP.NET MVC → ASP.NET Core migration for NYCJobsWeb).
5. **ASP.NET → ASP.NET Core migration** (NYCJobsWeb) — The web project uses ASP.NET MVC 5 on .NET Framework, which requires migration to ASP.NET Core MVC on .NET 10.0.

## Projects in Solution

- `NYCJobsWeb.sln`
  - `NYCJobsWeb\NYCJobsWeb.csproj` — ASP.NET MVC 5 web application (.NET Framework 4.7.2)
  - `DataLoader\DataLoader\DataLoader.csproj` — Console application (.NET Framework 4.5)
