# Configuration & Externalized Settings Inventory

Configuration is held in legacy XML configuration files and Web.config transforms. Azure Search credentials are represented by placeholders; no external configuration service or secret store was identified.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | XML application configuration | `NYCJobsWeb/Web.config` | Search service and API key placeholders; MVC settings and binding redirects |
| App.config | XML application configuration | `DataLoader/DataLoader/App.config` | Target service name and API key placeholders |
| Web.Debug.config | Build transform | `NYCJobsWeb/Web.Debug.config` | Debug deployment transform |
| Web.Release.config | Build transform | `NYCJobsWeb/Web.Release.config` | Release deployment transform |
| packages.config | NuGet package manifest | Both projects | Declared package versions |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild `Debug` configuration | Debug symbols, unoptimized builds | No profile-specific package additions identified |
| Release | MSBuild `Release` configuration | Optimized builds | No profile-specific package additions identified |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default web runtime | IIS / ASP.NET hosting | `NYCJobsWeb/Web.config` | Search settings and MVC defaults |
| Data loader | Console launch | `DataLoader/DataLoader/App.config` | Target Azure Search service settings |
| Environment-specific web config | Web.config transforms at publish/build time | `Web.Debug.config`, `Web.Release.config` | No environment-specific setting changes identified in the transforms |

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `SearchServiceName` | `azs-playground` | All | `NYCJobsWeb/Web.config` |
| `SearchServiceApiKey` | `[api-key placeholder]` | All | `NYCJobsWeb/Web.config` |
| `BingApiKey` | Empty/placeholder | All | `NYCJobsWeb/Web.config` |
| `webpages:Version` | `3.0.0.0` | All | `NYCJobsWeb/Web.config` |
| `webpages:Enabled` | `false` | All | `NYCJobsWeb/Web.config` |
| `ClientValidationEnabled` | `true` | All | `NYCJobsWeb/Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | All | `NYCJobsWeb/Web.config` |

### DataLoader

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | `[service name placeholder]` | All | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[API key placeholder]` | All | `DataLoader/DataLoader/App.config` |

The web application's search endpoint is assembled in code from the configured service name; the API key is sent as the Azure Search `api-key` header by the loader.

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | No explicit startup options found | Not specified | Not specified |
| DataLoader | No explicit startup options found | Not specified | One console process when manually run |

## Startup Dependency Chain

1. NYCJobsWeb loads Web.config, constructs Azure Search clients, and requires the configured Azure Search service for search actions.
2. DataLoader reads App.config, creates its Azure Search service URI, then deletes/recreates indexes and uploads local JSON records.
3. No readiness probes, startup wait scripts, or health-check chain were identified.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search API key | Placeholder in `NYCJobsWeb/Web.config` |
| `TargetSearchServiceApiKey` | Azure Search API key | Placeholder in `DataLoader/DataLoader/App.config` |
| `BingApiKey` | External API key | Empty/placeholder in `NYCJobsWeb/Web.config` |

### Secrets Provisioning Workflow

No Key Vault, Vault, cloud secret manager, environment-variable binding, encryption, or managed identity workflow was found. The configuration files instruct operators to provide service credentials manually; the checked-in values are placeholders, not secret values.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None identified | — | No feature-flag framework or conditional feature configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj` |
| DataLoader target framework | .NET Framework 4.5 | `DataLoader/DataLoader/DataLoader.csproj` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| Razor / WebPages | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| MSBuild project format | Legacy project files, ToolsVersion 12.0 | Both `.csproj` files |
