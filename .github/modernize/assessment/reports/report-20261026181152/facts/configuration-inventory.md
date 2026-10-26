# Configuration & Externalized Settings Inventory

Configuration is held primarily in two XML settings files, with Debug and Release web.config transforms. No explicit runtime profiles, environment-variable overrides, remote configuration service, or centralized secret store were identified.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web application settings | ASP.NET configuration | `NYCJobsWeb/Web.config` | App settings, runtime binding redirects, and framework settings |
| Debug transform | XML transform | `NYCJobsWeb/Web.Debug.config` | Contains template examples; no active service-specific overrides identified |
| Release transform | XML transform | `NYCJobsWeb/Web.Release.config` | Removes the compilation debug attribute |
| Data loader settings | .NET application configuration | `DataLoader/DataLoader/App.config` | Target Search service name and API key placeholders |
| Client map configuration | Inline browser configuration | `NYCJobsWeb/Views/Home/Index.cshtml` | A map-service credential is embedded in the view; value intentionally omitted |
| Index definitions | Search index schemas | `NYCJobsWeb/Schema_and_Data/*.schema` | Defines job and ZIP-code index fields and search behavior |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild configuration selection | Debug symbols and non-optimized build; web config enables debug compilation | No profile-specific dependencies identified |
| Release | MSBuild configuration selection | Optimized build; web transform removes compilation debug attribute | No profile-specific dependencies identified |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default web runtime | IIS / IIS Express hosting | `NYCJobsWeb/Web.config` | Uses app settings and .NET Framework 4.7.2 configuration |
| Data loader runtime | .NET Framework application config | `DataLoader/DataLoader/App.config` | Uses configured target search service name and API key |

No environment-specific runtime profile files or environment-variable activation were found.

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `BingApiKey` | Masked; one entry is empty | Default | `NYCJobsWeb/Web.config` |
| `SearchServiceName` | Configured sample service name | Default | `NYCJobsWeb/Web.config` |
| `SearchServiceApiKey` | `[MASKED]` placeholder | Default | `NYCJobsWeb/Web.config` |
| `Searchendpoint` | Not configured in inspected `Web.config` | Default | Read by `JobsSearch` from app settings |
| `webpages:Version` | `3.0.0.0` | Default | `NYCJobsWeb/Web.config` |
| `webpages:Enabled` | `false` | Default | `NYCJobsWeb/Web.config` |
| `ClientValidationEnabled` | `true` | Default | `NYCJobsWeb/Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `NYCJobsWeb/Web.config` |

### AzureSearchBackupRestore

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | `[MASKED]` placeholder | Default | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[MASKED]` placeholder | Default | `DataLoader/DataLoader/App.config` |
| Supported runtime | .NET Framework 4.5 | Default | `DataLoader/DataLoader/App.config` |

No environment-variable substitutions or remote configuration property sources were identified.

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; IIS Express development port 51269 | Not specified | Not specified |
| AzureSearchBackupRestore | .NET Framework 4.5 | Not specified | Single console process |

No JVM options, container resource limits, or scaling settings were found.

## Startup Dependency Chain

1. NYCJobsWeb requires valid search endpoint and credential settings before search operations can initialize successfully.
2. AzureSearchBackupRestore requires its target service name and API key before it can issue index-management REST calls.

No health-check wait, startup retry, readiness probe, or explicit timeout configuration was identified.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search API key | XML app settings; `[MASKED]` placeholder |
| `TargetSearchServiceApiKey` | Azure Search API key | XML app settings; `[MASKED]` placeholder |
| `BingApiKey` | Bing Maps API key | XML app settings; `[MASKED]` or empty |
| Inline map-service credential | Browser-facing service credential | Embedded in a Razor view; value omitted |

No Key Vault, environment-based secret injection, encrypted configuration mechanism, or managed secret store was identified.

### Secrets Provisioning Workflow

The application reads service credentials from XML configuration at runtime, and the client map credential is embedded in the browser view. The repository does not show a provisioning pipeline or managed identity/access model; no automatic secret rotation or deployment binding mechanism was identified.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None identified | Not applicable | No feature-flag framework or toggle configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj` |
| AzureSearchBackupRestore target framework | .NET Framework 4.5 | `DataLoader/DataLoader/DataLoader.csproj` and `App.config` |
| ASP.NET MVC | 5.2.2 package | `NYCJobsWeb/packages.config` |
| Azure Search Documents | 11.1.1 package | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (web) | 10.0.3 package | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (data loader) | 9.0.1 package | `DataLoader/DataLoader/packages.config` |
| Bootstrap | 3.4.1 package | `NYCJobsWeb/packages.config` |
| jQuery | 3.1.1 package | `NYCJobsWeb/packages.config` |
