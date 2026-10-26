# Configuration & Externalized Settings Inventory

Configuration is held in the web application's `Web.config`, the console utility's `App.config`, legacy MSBuild project files, and Azure Search schema files. No environment-specific configuration profiles or central secret store integration were found.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web configuration | Runtime XML | `NYCJobsWeb/Web.config` | MVC settings, framework target, binding redirects, and search-related app settings |
| Loader configuration | Runtime XML | `DataLoader/DataLoader/App.config` | Target Azure Search service name and API-key placeholder |
| Web project | Build XML | `NYCJobsWeb/NYCJobsWeb.csproj` | Targets .NET Framework 4.7.2; Debug and Release groups |
| Loader project | Build XML | `DataLoader/DataLoader/DataLoader.csproj` | Targets .NET Framework 4.5; Debug and Release groups |
| Search index definitions | JSON data | `NYCJobsWeb/Schema_and_Data/*.schema` | Azure Search index schemas |
| Search seed data | JSON data | `NYCJobsWeb/Schema_and_Data/*.json` | Static job and ZIP code documents |
| External services | Hosted settings | Azure Search and Bing Maps | Search endpoint/key and browser map credential are configured or embedded; values are not reproduced here |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild `Debug` configuration | Debug symbols enabled and optimization disabled | Legacy project references and NuGet packages |
| Release | MSBuild `Release` configuration | Optimized build with PDB-only symbols | Legacy project references and NuGet packages |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | No profile selector found | `NYCJobsWeb/Web.config` | Single set of application settings |
| Console utility | Launch of console application | `DataLoader/DataLoader/App.config` | Target search service settings |

No Development/Production-specific config files or ASP.NET environment selector were found.

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `SearchServiceName` | Placeholder/demo service name | Default | `Web.config` |
| `SearchServiceApiKey` | `[MASKED]` placeholder | Default | `Web.config` |
| `BingApiKey` | Placeholder/empty duplicate entries | Default | `Web.config` |
| `webpages:Version` | `3.0.0.0` | Default | `Web.config` |
| `webpages:Enabled` | `false` | Default | `Web.config` |
| `ClientValidationEnabled` | `true` | Default | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `Web.config` |
| `compilation.targetFramework` | `4.7.2` | Default | `Web.config` |
| `httpRuntime.targetFramework` | `4.7.2` | Default | `Web.config` |

`JobsSearch` reads `Searchendpoint`, while the checked-in `Web.config` declares `SearchServiceName`; no mapping between these names was found.

### AzureSearchBackupRestore

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | Placeholder | Console utility | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[MASKED]` placeholder | Console utility | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; IIS/IIS Express | Not specified | Not specified |
| AzureSearchBackupRestore | .NET Framework 4.5 console process | Not specified | Run on demand |

No explicit heap, CPU, container resource, or scaling parameters were found.

## Startup Dependency Chain

1. NYCJobsWeb requires the configured Azure Search endpoint and a valid credential before search requests can succeed. No startup readiness check is defined.
2. AzureSearchBackupRestore requires target service settings and local index schema/JSON files before starting its delete/create/import sequence.

No health-check probes, wait scripts, or service startup orchestration were found.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search API credential | `[MASKED]` placeholder in `Web.config` |
| `TargetSearchServiceApiKey` | Azure Search API credential | `[MASKED]` placeholder in `App.config` |
| Bing Maps browser credential | Browser API key | Present in client-side source; value intentionally omitted |

### Secrets Provisioning Workflow

The loader and web application expect Azure Search credentials through app settings. No vault, managed identity, secret injection workflow, or deployment-time provisioning mechanism was found. The browser map credential is embedded in the view instead of being provisioned through an external secret store.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | — | No feature-flag framework or conditional feature configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb.csproj` |
| AzureSearchBackupRestore target framework | .NET Framework 4.5 | `DataLoader.csproj` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| Razor / WebPages | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 web; 9.0.1 loader | Project `packages.config` files |
| Bootstrap / jQuery | 3.4.1 / 3.1.1 | `NYCJobsWeb/packages.config` |
| MSBuild project format | Legacy non-SDK | `.csproj` files |
