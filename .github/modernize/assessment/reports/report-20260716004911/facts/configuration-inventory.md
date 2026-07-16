# Configuration & Externalized Settings Inventory

Configuration is primarily file-based through legacy .NET `Web.config` and `App.config`, with environment-specific build transforms and API keys supplied via app settings.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | Runtime app settings | NYCJobsWeb/Web.config | Search endpoint, API keys, MVC runtime settings |
| Web.Debug.config | Runtime transform | NYCJobsWeb/Web.Debug.config | Debug transform placeholders |
| Web.Release.config | Runtime transform | NYCJobsWeb/Web.Release.config | Release transform placeholders |
| App.config | Runtime app settings | DataLoader/DataLoader/App.config | Target search service credentials |
| packages.config | Package config | NYCJobsWeb/packages.config, DataLoader/DataLoader/packages.config | External dependency versions |
| .csproj properties | Build config | NYCJobsWeb/NYCJobsWeb.csproj, DataLoader/DataLoader/DataLoader.csproj | Debug/Release, framework versions |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | `Configuration=Debug` | Developer build with symbols | Legacy MSBuild web application targets |
| Release | `Configuration=Release` | Optimized production build | Legacy MSBuild web application targets |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default web runtime | IIS/IIS Express app startup | Web.config | Search endpoint/key, ASP.NET runtime settings |
| Debug transform | Build/publish transform | Web.Debug.config | Debug-time web.config transforms |
| Release transform | Build/publish transform | Web.Release.config | Release-time web.config transforms |
| Loader runtime | Console execution | App.config | Target search service name and API key |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| Searchendpoint | `https://<service>.search.windows.net` style value | Default web runtime | Web.config appSettings |
| SearchServiceApiKey | `<api-key>` placeholder | Default web runtime | Web.config appSettings |
| BingApiKey | Empty in sample | Default web runtime | Web.config appSettings |
| TargetSearchServiceName | Placeholder value | Loader runtime | App.config appSettings |
| TargetSearchServiceApiKey | Placeholder value | Loader runtime | App.config appSettings |
| webpages:Enabled | `false` | Default web runtime | Web.config appSettings |
| ClientValidationEnabled | `true` | Default web runtime | Web.config appSettings |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | ASP.NET on .NET Framework 4.7.2 via IIS/IIS Express | Not specified in repo | Not specified |
| DataLoader | Console .NET Framework 4.5 process | Not specified in repo | On-demand execution |

## Startup Dependency Chain

1. NYCJobsWeb startup reads `Web.config` and initializes MVC routes.
2. `JobsSearch` static constructor reads search endpoint and API key.
3. API actions become functional only after Azure Search endpoint is reachable.
4. DataLoader execution requires valid target search credentials before index recreation/import.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| SearchServiceApiKey | API key | Web.config `[MASKED]` |
| BingApiKey | API key | Web.config `[MASKED/empty]` |
| TargetSearchServiceApiKey | API key | App.config `[MASKED]` |

### Secrets Provisioning Workflow

Secrets are expected to be injected by operators directly into config files before running the applications. The codebase does not show managed identity, centralized secret vault, or automated CI/CD secret binding.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| webpages:Enabled | false | Web.config appSettings |
| ClientValidationEnabled | true | Web.config appSettings |
| UnobtrusiveJavaScriptEnabled | true | Web.config appSettings |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework (web) | 4.7.2 | NYCJobsWeb.csproj |
| .NET Framework (loader) | 4.5 | DataLoader.csproj/App.config |
| ASP.NET MVC | 5.2.2 | NYCJobsWeb/packages.config |
| Azure.Search.Documents | 11.1.1 | NYCJobsWeb/packages.config |
| Newtonsoft.Json | 10.0.3 (web), 9.0.1 (loader) | packages.config files |
| Bootstrap | 3.4.1 | NYCJobsWeb/packages.config |
| jQuery | 3.1.1 | NYCJobsWeb/packages.config |
