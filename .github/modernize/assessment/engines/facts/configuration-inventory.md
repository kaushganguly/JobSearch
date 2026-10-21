# Configuration & Externalized Settings Inventory

Configuration is primarily held in legacy .NET XML files, with no runtime profile system or external configuration service found. Azure Search credentials are intended to be supplied through configuration, while the Bing Maps browser credential is embedded in a view and is redacted here.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web configuration | Runtime application configuration | `NYCJobsWeb/Web.config` | MVC framework settings and app settings; contains masked/example credential placeholders |
| Web transformations | Build/deployment configuration transforms | `NYCJobsWeb/Web.Debug.config`, `NYCJobsWeb/Web.Release.config` | Template transforms; no active environment-specific overrides found |
| Console configuration | Runtime application configuration | `DataLoader/DataLoader/App.config` | Target Azure Search service name and API-key placeholder |
| Project files | Build configuration | `NYCJobsWeb/NYCJobsWeb.csproj`, `DataLoader/DataLoader/DataLoader.csproj` | Legacy MSBuild projects, framework targets, Debug/Release properties |
| Browser map configuration | Embedded client configuration | `NYCJobsWeb/Views/Home/Index.cshtml` | Bing Maps credential is present in page source; value intentionally omitted |
| Search index definitions | Service data configuration | `NYCJobsWeb/Schema_and_Data/*.schema` | Azure Search index fields, suggesters, scoring, and index names |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | `Debug|AnyCPU` MSBuild configuration | Debug symbols and unoptimized output | Existing project references |
| Release | `Release|AnyCPU` MSBuild configuration | Optimized output and PDB-only debug symbols | Existing project references |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | No environment selector found | `Web.config`, `App.config` | Static app settings |
| Debug/Release web transforms | Build configuration | `Web.Debug.config`, `Web.Release.config` | Only template/commented transforms found |

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `Searchendpoint` | Not present in checked-in `Web.config` | None found | Read by `JobsSearch`; the checked-in config instead names `SearchServiceName` |
| `SearchServiceApiKey` | Masked/example placeholder | None found | `Web.config`; read by `JobsSearch` |
| `SearchServiceName` | Example service name | None found | `Web.config`; no matching use was identified in `JobsSearch` |
| `webpages:Version` | `3.0.0.0` | None found | `Web.config` |
| `webpages:Enabled` | `false` | None found | `Web.config` |
| `ClientValidationEnabled` | `true` | None found | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | None found | `Web.config` |
| `BingApiKey` | Masked/example placeholder | None found | `Web.config`; a separate Bing Maps credential is embedded in `Index.cshtml` and omitted from this report |

### AzureSearchBackupRestore

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | Masked/example placeholder | None found | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | Masked/example placeholder | None found | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; hosted through IIS/IIS Express configuration | Not specified | Not specified |
| AzureSearchBackupRestore | .NET Framework 4.5 console process | Not specified | One invocation at a time is implied by the console workflow |

No container resource limits, JVM options, or instance scaling settings were found.

## Startup Dependency Chain

1. The website starts under IIS/IIS Express and loads its configured Azure Search client when `JobsSearch` initializes.
2. Search actions require the Azure Search endpoint and API key to be correctly configured.
3. The console loader requires its target service settings and then performs index delete/create/upload calls.

No readiness probes, wait scripts, or startup retry policies were found.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search API key | `Web.config` example placeholder; value masked |
| `TargetSearchServiceApiKey` | Azure Search API key | `App.config` example placeholder; value masked |
| Bing Maps `credentials` setting | Browser API credential | Embedded in `Index.cshtml`; value omitted |

### Secrets Provisioning Workflow

The website reads its Azure Search endpoint and key through `ConfigurationManager.AppSettings`; the loader reads its target service name and key from `App.config` and sends the key in the REST `api-key` header. No Key Vault, managed identity, environment-variable substitution, encrypted configuration, or deployment secret-binding workflow is present in the repository. The map credential is shipped to browsers as part of the view.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None found | — | No feature-flag framework or toggle configuration was identified |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target | .NET Framework 4.7.2 | `NYCJobsWeb.csproj`, `Web.config` |
| NYCJobsWeb web framework | ASP.NET MVC 5.2.2 | `NYCJobsWeb/packages.config` |
| Azure Search SDK | 11.1.1 | `NYCJobsWeb/packages.config` |
| DataLoader target | .NET Framework 4.5 | `DataLoader.csproj`, `App.config` |
| DataLoader JSON library | Newtonsoft.Json 9.0.1 | `DataLoader/packages.config` |
| Web JSON library | Newtonsoft.Json 10.0.3 | `NYCJobsWeb/packages.config` |
| Build system | Legacy MSBuild project format | Both `.csproj` files |
