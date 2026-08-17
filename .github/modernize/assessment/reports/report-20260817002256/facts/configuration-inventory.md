# Configuration & Externalized Settings Inventory

Configuration is file-driven and split across ASP.NET web configuration and console app settings, with service credentials provided through app settings placeholders. Build/runtime profile usage is minimal and mostly limited to Debug/Release transforms.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | ASP.NET app config | `NYCJobsWeb/Web.config` | Primary runtime settings for web app and assembly binding redirects |
| Web.Debug.config | Transform | `NYCJobsWeb/Web.Debug.config` | Debug transform template (mostly comments) |
| Web.Release.config | Transform | `NYCJobsWeb/Web.Release.config` | Removes debug compilation attribute for release |
| packages.config (web) | Package config | `NYCJobsWeb/packages.config` | Declared NuGet package versions |
| App.config | Console config | `DataLoader/DataLoader/App.config` | Target Azure Search service name and API key placeholders |
| packages.config (loader) | Package config | `DataLoader/DataLoader/packages.config` | Loader dependency declarations |
| Schema and seed JSON | Data/config assets | `NYCJobsWeb/Schema_and_Data/*` | Defines index schemas and import payloads used by DataLoader |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug (NYCJobsWeb/DataLoader) | Default or `/p:Configuration=Debug` | Local development with debug symbols | Standard MSBuild targets |
| Release (NYCJobsWeb/DataLoader) | `/p:Configuration=Release` | Optimized build artifacts | Standard MSBuild targets |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default (web) | ASP.NET host startup | `Web.config` | Search service/app settings and assembly binding redirects |
| Default (loader) | Console startup | `App.config` | Target service name/API key for import operations |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| BingApiKey | Empty/placeholder | Default | `NYCJobsWeb/Web.config` |
| SearchServiceName | `azs-playground` | Default | `NYCJobsWeb/Web.config` |
| SearchServiceApiKey | `<api-key>` placeholder | Default | `NYCJobsWeb/Web.config` |
| webpages:Version | `3.0.0.0` | Default | `NYCJobsWeb/Web.config` |
| webpages:Enabled | `false` | Default | `NYCJobsWeb/Web.config` |
| ClientValidationEnabled | `true` | Default | `NYCJobsWeb/Web.config` |
| UnobtrusiveJavaScriptEnabled | `true` | Default | `NYCJobsWeb/Web.config` |
| TargetSearchServiceName | Placeholder string | Default | `DataLoader/DataLoader/App.config` |
| TargetSearchServiceApiKey | Placeholder string | Default | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2 ASP.NET runtime | Not explicitly configured | Not specified |
| DataLoader | .NET Framework 4.5 console runtime | Not explicitly configured | On-demand execution |

## Startup Dependency Chain

1. Azure AI Search service must be available before DataLoader or web search queries can succeed.
2. DataLoader (optional but operationally important) populates `nycjobs` and `zipcodes` indexes before web usage.
3. NYCJobsWeb starts and serves routes; search endpoints depend on configured service endpoint and API key.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | API key | `Web.config` appSetting value `[MASKED/placeholder]` |
| `TargetSearchServiceApiKey` | API key | `App.config` appSetting value `[MASKED/placeholder]` |
| `BingApiKey` | API key | `Web.config` appSetting value `[MASKED/placeholder]` |

### Secrets Provisioning Workflow

Secrets are expected to be injected into configuration files (or equivalent deployment-time transforms) before runtime. The web app and DataLoader both read API keys from local configuration and attach them to outbound requests to Azure services. No managed identity or external secret store integration is declared in the repository.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | N/A |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework (web app) | 4.7.2 | `NYCJobsWeb.csproj`, `Web.config` |
| .NET Framework (DataLoader) | 4.5 | `DataLoader.csproj`, `App.config` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Azure.Core | 1.4.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 (web), 9.0.1 (loader) | `packages.config` files |
| jQuery | 3.1.1 | `NYCJobsWeb/packages.config` |
| Bootstrap | 3.4.1 | `NYCJobsWeb/packages.config` |
