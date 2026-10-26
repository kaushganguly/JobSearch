# Configuration & Externalized Settings Inventory

Configuration is primarily stored in legacy ASP.NET `Web.config` and console `App.config` files, with Debug/Release transformations for the web project. No runtime profile system, deployment manifests, or centralized secret store is defined in the repository.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web application settings | XML | `NYCJobsWeb/Web.config` | MVC settings, application settings, compilation/runtime settings, and assembly redirects |
| Web transformations | XML transforms | `NYCJobsWeb/Web.Debug.config`, `NYCJobsWeb/Web.Release.config` | Build/publish transformations |
| Console application settings | XML | `DataLoader/DataLoader/App.config` | Target Azure Search service name and API key placeholders |
| Package declarations | NuGet XML | `NYCJobsWeb/packages.config`, `DataLoader/DataLoader/packages.config` | Legacy package versions |
| Index schemas and data | JSON | `NYCJobsWeb/Schema_and_Data/` | Azure Search index definitions and seed records |
| Project/build configuration | MSBuild XML | `NYCJobsWeb/NYCJobsWeb.csproj`, `DataLoader/DataLoader.csproj` | Framework targets and Debug/Release properties |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild `Debug` configuration | Development/debug build; web config transform available | No profile-specific package references |
| Release | MSBuild `Release` configuration | Optimized/publish build; web config transform available | No profile-specific package references |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Web Debug | Build/publish configuration | `Web.config`, `Web.Debug.config` | Transform-based settings; no named runtime environment |
| Web Release | Build/publish configuration | `Web.config`, `Web.Release.config` | Transform-based settings; no named runtime environment |
| DataLoader | Application startup | `App.config` | Target search service settings; no environment-specific file |

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `SearchServiceName` | `azs-playground` | Shared | `Web.config`; not referenced by `JobsSearch` |
| `SearchServiceApiKey` | `[MASKED/placeholder]` | Shared | `Web.config` |
| `Searchendpoint` | No value declared | Shared | Read by `JobsSearch`; no matching key found in `Web.config` |
| `BingApiKey` | `[MASKED/empty placeholder]` | Shared | `Web.config`; repeated key declarations are present |
| `webpages:Version` | `3.0.0.0` | Shared | `Web.config` |
| `webpages:Enabled` | `false` | Shared | `Web.config` |
| `ClientValidationEnabled` | `true` | Shared | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Shared | `Web.config` |
| `compilation/@targetFramework` | `4.7.2` | Shared | `Web.config` |
| `httpRuntime/@targetFramework` | `4.7.2` | Shared | `Web.config` |

### DataLoader

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | `[MASKED/placeholder]` | Shared | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[MASKED/placeholder]` | Shared | `DataLoader/DataLoader/App.config` |
| `startup/supportedRuntime` | .NET Framework 4.5 | Shared | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; IIS Express enabled for development | Not specified | Not specified |
| DataLoader | .NET Framework 4.5 console executable | Not specified | Runs when invoked |

No container CPU/memory limits, JVM settings, or instance scaling configuration were found.

## Startup Dependency Chain

1. NYCJobsWeb requires a reachable Azure Search endpoint and valid settings when `JobsSearch` initializes its clients.
2. DataLoader requires target Azure Search service settings, then reads the repository's schema and JSON seed files before issuing index operations.
3. No readiness probes, health-check waits, or service orchestration dependencies are defined.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search API credential | `[MASKED/placeholder in Web.config]` |
| `BingApiKey` | Bing Maps API credential | `[MASKED/empty placeholder in Web.config]`; a client-side key is also embedded in `Views/Home/Index.cshtml` and is intentionally not reproduced |
| `TargetSearchServiceApiKey` | Azure Search API credential | `[MASKED/placeholder in App.config]` |

### Secrets Provisioning Workflow

The repository contains no Key Vault, Vault, AWS Secrets Manager, managed identity, or automated secret provisioning configuration. The application reads credentials from app settings, while the map credential is also included in browser-delivered source. The exact values are omitted.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | — | No feature-flag framework or runtime toggle configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| .NET Framework - NYCJobsWeb | 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj` and `Web.config` |
| .NET Framework - DataLoader | 4.5 | `DataLoader/DataLoader/DataLoader.csproj` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json - NYCJobsWeb | 10.0.3 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json - DataLoader | 9.0.1 | `DataLoader/DataLoader/packages.config` |
| Build system | Legacy MSBuild project format | Both `.csproj` files |
