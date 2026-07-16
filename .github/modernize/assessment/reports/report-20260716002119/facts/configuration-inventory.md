# Configuration & Externalized Settings Inventory

Configuration is file-based and centralized in legacy .NET config files (`Web.config`, `App.config`) with environment behavior primarily driven by build configurations.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | Application settings/runtime | `NYCJobsWeb/Web.config` | Search endpoint/key and ASP.NET runtime settings |
| Web.Debug.config | Transform | `NYCJobsWeb/Web.Debug.config` | Debug transform template |
| Web.Release.config | Transform | `NYCJobsWeb/Web.Release.config` | Release transform template |
| App.config | Application settings/runtime | `DataLoader/DataLoader/App.config` | Target search service settings |
| packages.config | Dependency config | `NYCJobsWeb/packages.config`, `DataLoader/DataLoader/packages.config` | NuGet package declarations |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | Default/local build | Development build with symbols | .NET Framework build settings |
| Release | Manual or CI build config | Optimized build for deployment | .NET Framework build settings |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default IIS/ASP.NET runtime | IIS/IIS Express startup | `Web.config` | App settings and binding redirects |
| Console runtime | Executing DataLoader EXE | `App.config` | Target search service and key settings |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| SearchServiceName | `azs-playground` | Default | `NYCJobsWeb/Web.config` |
| SearchServiceApiKey | `[MASKED]` placeholder | Default | `NYCJobsWeb/Web.config` |
| BingApiKey | empty/placeholder | Default | `NYCJobsWeb/Web.config` |
| webpages:Enabled | `false` | Default | `NYCJobsWeb/Web.config` |
| ClientValidationEnabled | `true` | Default | `NYCJobsWeb/Web.config` |
| UnobtrusiveJavaScriptEnabled | `true` | Default | `NYCJobsWeb/Web.config` |
| TargetSearchServiceName | placeholder | Default | `DataLoader/DataLoader/App.config` |
| TargetSearchServiceApiKey | `[MASKED]` placeholder | Default | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | ASP.NET runtime on .NET Framework 4.7.2 | Not specified | Not specified |
| DataLoader | Console runtime on .NET Framework 4.5 | Not specified | On-demand execution |

## Startup Dependency Chain

1. `NYCJobsWeb` starts under IIS/IIS Express and registers MVC routes.
2. `JobsSearch` static constructor resolves search endpoint and API key from config.
3. Requests are served only after search client initialization succeeds.
4. `DataLoader` startup depends on valid target search service settings in `App.config`.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | API key | `Web.config` (`[MASKED]`) |
| `TargetSearchServiceApiKey` | API key | `App.config` (`[MASKED]`) |
| `BingApiKey` | API key | `Web.config` (`[MASKED]`/empty placeholder) |

### Secrets Provisioning Workflow

Secrets are expected to be provisioned outside source control and injected into config files before deployment/runtime. The web app and loader both require search service credentials; no managed identity, vault integration, or automated secret binding workflow was detected.

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
