# Configuration & Externalized Settings Inventory

Configuration is primarily file-based (`Web.config` and `App.config`) with environment-specific transforms in the web app and API keys provided through app settings.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | .NET app config | `NYCJobsWeb/Web.config` | Main runtime settings and assembly redirects |
| Web.Debug.config | Transform | `NYCJobsWeb/Web.Debug.config` | Debug-time config transform |
| Web.Release.config | Transform | `NYCJobsWeb/Web.Release.config` | Release-time config transform |
| App.config | .NET app config | `DataLoader/DataLoader/App.config` | DataLoader service/API key settings |
| packages.config | NuGet manifest | `NYCJobsWeb/packages.config`, `DataLoader/DataLoader/packages.config` | Declared package versions |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | Build configuration | Local debugging with symbols | Same dependency set as project references |
| Release | Build configuration | Optimized release build | Same dependency set as project references |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | IIS/App start | `Web.config`, `App.config` | Azure Search endpoint and API keys from app settings |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| SearchServiceName | `azs-playground` | Default | `NYCJobsWeb/Web.config` |
| SearchServiceApiKey | `[MASKED]` | Default | `NYCJobsWeb/Web.config` |
| Searchendpoint | Not hardcoded in visible file | Default | `NYCJobsWeb/Web.config` (referenced by code) |
| BingApiKey | Empty/placeholder | Default | `NYCJobsWeb/Web.config` |
| TargetSearchServiceName | Placeholder | Default | `DataLoader/DataLoader/App.config` |
| TargetSearchServiceApiKey | `[MASKED]` placeholder | Default | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2 runtime (IIS/IIS Express) | Not specified | Not specified |
| DataLoader | .NET Framework 4.5 console runtime | Not specified | On-demand execution |

## Startup Dependency Chain

1. `NYCJobsWeb` starts and registers MVC routes.
2. On first `JobsSearch` use, Azure Search clients are initialized from app settings.
3. For data refresh scenarios, `DataLoader` should run first to recreate/upload index data before web queries.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search query key | `Web.config` value `[MASKED]` |
| `BingApiKey` | External API key | `Web.config` value `[MASKED]` |
| `TargetSearchServiceApiKey` | Azure Search admin key | `App.config` value `[MASKED]` |

### Secrets Provisioning Workflow

Secrets are currently expected in local configuration files as app settings. The application reads them at runtime through `ConfigurationManager`; no external vault integration, managed identity flow, or automated secret injection mechanism is defined in the repository.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | N/A |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework (NYCJobsWeb) | 4.7.2 | `NYCJobsWeb.csproj` |
| .NET Framework (DataLoader) | 4.5 | `DataLoader.csproj` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 / 9.0.1 | `packages.config` files |
