# Configuration & Externalized Settings Inventory

Configuration is held in two legacy XML configuration files, with no environment-specific profile files or external configuration service identified. Some cloud credentials are intended to be supplied via settings, but the browser-side mapping credential is embedded in source code.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| NYCJobsWeb configuration | XML web configuration | `/home/runner/work/JobSearch/JobSearch/NYCJobsWeb/Web.config` | Contains application settings and ASP.NET runtime configuration; XML is not well-formed at line 8 in this checkout |
| DataLoader configuration | XML application configuration | `/home/runner/work/JobSearch/JobSearch/DataLoader/DataLoader/App.config` | Holds target search service settings; credential value not reproduced |
| Build configuration | MSBuild project configuration | Both `.csproj` files | Debug and Release configurations |
| Map credential | Client-side source setting | `/home/runner/work/JobSearch/JobSearch/NYCJobsWeb/Views/Home/Index.cshtml` | Non-empty mapping API credential is embedded in the page source; value intentionally omitted |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | Default MSBuild configuration | Debug symbols, unoptimized build, `DEBUG;TRACE` constants | No profile-specific packages |
| Release | Explicit `Release` configuration | Optimized build and `TRACE` constant | No profile-specific packages |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | Application settings; no environment profile selector found | Web.config and App.config | Search service name/endpoint, API-key settings, MVC validation settings |
| Development/Production | Not detected | None | No `Web.Debug.config`/`Web.Release.config` transform settings were detected in repository configuration |

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `BingApiKey` | One malformed placeholder and one empty setting in Web.config; client-side view contains an embedded credential | Default | Web.config and Index.cshtml |
| `SearchServiceName` | `azs-playground` | Default | Web.config |
| `SearchServiceApiKey` | Placeholder value in Web.config, not a configured credential | Default | Web.config |
| `Searchendpoint` | Not present in Web.config; read by the search client | Default | JobsSearch.cs |
| `webpages:Version` | `3.0.0.0` | Default | Web.config |
| `webpages:Enabled` | `false` | Default | Web.config |
| `ClientValidationEnabled` | `true` | Default | Web.config |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | Web.config |

### DataLoader

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | Placeholder for target service name | Default | App.config |
| `TargetSearchServiceApiKey` | Not populated in tracked configuration; value masked | Default | App.config |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | IIS-hosted ASP.NET application; no explicit runtime flags documented | Not specified | Not specified |
| DataLoader | Console process; no runtime flags documented | Not specified | One process per import run |

## Startup Dependency Chain

1. NYCJobsWeb requires its hosting environment and configured Azure Cognitive Search service to handle search requests; no readiness wait or health-check mechanism is defined.
2. DataLoader requires network access to the configured Azure Cognitive Search endpoint and local schema/JSON files before index provisioning/import. It is run separately from the web application.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Cognitive Search credential | Web.config setting is empty in this checkout |
| `TargetSearchServiceApiKey` | Azure Cognitive Search credential | App.config setting; value is not reproduced |
| Client-side Bing Maps API credential | Mapping-service credential | Embedded in the view source; value is not reproduced |

### Secrets Provisioning Workflow

No Key Vault, secret manager, managed identity, environment-variable binding, encrypted configuration, or CI provisioning workflow was found. Azure Search credentials are expected to be supplied in application configuration; the browser map credential is currently delivered as source. No identity/RBAC model is documented in the repository.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | — | No feature-flag framework or toggle configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | NYCJobsWeb.csproj |
| DataLoader target framework | .NET Framework 4.5 | DataLoader.csproj |
| ASP.NET MVC | 5.2.2 | NYCJobsWeb/packages.config |
| Azure Search SDK | Azure.Search.Documents 11.1.1 | NYCJobsWeb/packages.config |
| Web UI libraries | jQuery 3.1.1, Bootstrap 3.4.1 | NYCJobsWeb/packages.config |
| DataLoader JSON library | Newtonsoft.Json 9.0.1 | DataLoader/packages.config |
| Build system | Legacy MSBuild project format; Visual Studio 2013 solution format | `.sln` and `.csproj` files |
| Container/base image | None detected | No container build files found |
