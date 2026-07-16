# .NET Upgrade Plan: JobSearch

## Overview

Upgrade the JobSearch solution from .NET Framework to .NET 10.0 (latest LTS). Both projects use the legacy non-SDK project format and require SDK-style conversion as part of the upgrade.

## Source → Target

| Project | Source Framework | Target Framework |
|---------|-----------------|-----------------|
| NYCJobsWeb | .NET Framework 4.7.2 (ASP.NET MVC 5) | net10.0 |
| DataLoader (AzureSearchBackupRestore) | .NET Framework 4.5 | net10.0 |

## Projects in Solution

- **NYCJobsWeb** (`NYCJobsWeb/NYCJobsWeb.csproj`) — ASP.NET MVC 5 web application targeting .NET Framework 4.7.2
- **DataLoader** (`DataLoader/DataLoader/DataLoader.csproj`) — Console application targeting .NET Framework 4.5

## Upgrade Scope

1. **SDK-style project file conversion** — Both projects use the legacy MSBuild project format and must be converted to SDK-style `.csproj` files.
2. **Target framework update** — Change `<TargetFrameworkVersion>` to `<TargetFramework>net10.0</TargetFramework>` in both projects.
3. **NuGet package updates** — Replace old packages (Azure.Search.Documents 11.1.1, Newtonsoft.Json 9/10, Microsoft.AspNet.Mvc 5.x, etc.) with their current equivalents compatible with net10.0.
4. **ASP.NET MVC → ASP.NET Core migration** — NYCJobsWeb uses `System.Web` / ASP.NET MVC 5 which is not supported on .NET 10. The web project must be migrated to ASP.NET Core (Razor views, routing, middleware pipeline, etc.).
5. **API compatibility fixes** — Address any breaking API changes introduced between .NET Framework and .NET 10.
6. **Build validation** — Ensure both projects compile and all existing unit tests pass after upgrade.

## Reason for Upgrade

- **DataLoader** targets .NET Framework 4.5, which is EOL and does not support `netstandard2.0`, making it incompatible with modern Azure SDK (`Azure.*`) packages.
- **NYCJobsWeb** targets .NET Framework 4.7.2 and uses `System.Web`-based ASP.NET MVC 5, which is not available on modern .NET; migration to ASP.NET Core is required for .NET 10 compatibility.
- The user explicitly requested an upgrade to the latest LTS version (.NET 10).
