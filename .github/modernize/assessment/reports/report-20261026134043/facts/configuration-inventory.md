# Configuration & Externalized Settings Inventory

The repository has XML-based configuration for two .NET Framework projects, with web configuration transforms for Debug and Release builds. Search API credentials are represented in application configuration; no managed secret-store or environment-variable provisioning workflow was identified.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web application configuration | Runtime XML | `NYCJobsWeb/Web.config` | Application settings, framework runtime configuration, and assembly binding redirects |
| Web configuration transforms | Build/deployment XML transforms | `NYCJobsWeb/Web.Debug.config`, `NYCJobsWeb/Web.Release.config` | Release transform removes the debug compilation attribute; sample transform examples are commented out |
| Data loader configuration | Runtime XML | `DataLoader/DataLoader/App.config` | Search service name and API-key placeholders; declares .NET Framework 4.5 runtime |
| MVC view configuration | Runtime XML | `NYCJobsWeb/Views/web.config` | Razor and view-engine settings |
| Package manifests | Dependency configuration | `NYCJobsWeb/packages.config`, `DataLoader/DataLoader/packages.config` | NuGet package versions |
| Build project files | Build configuration | `NYCJobsWeb/NYCJobsWeb.csproj`, `DataLoader/DataLoader/DataLoader.csproj` | Framework targets, Debug/Release configuration and references |
| Assessment configuration | Tool configuration | `.github/modernize/assessment/reports/assessment-config.yaml` | Declares assessment domains, target runtimes, and a medium CVE threshold |

No external configuration server, Key Vault, Vault, AWS Secrets Manager, Docker Compose, Kubernetes configuration, or `appsettings` files were found.

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild `Debug` configuration | Debug symbols and non-optimized builds; web config enables debug compilation | Defines `DEBUG;TRACE` |
| Release | MSBuild `Release` configuration | Optimized builds; removes web compilation debug attribute | Defines `TRACE` |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Web Debug | Web.config transform during a Debug build | `NYCJobsWeb/Web.config`, `NYCJobsWeb/Web.Debug.config` | No substantive overrides identified |
| Web Release | Web.config transform during a Release build | `NYCJobsWeb/Web.config`, `NYCJobsWeb/Web.Release.config` | Removes the `debug` attribute from compilation |
| DataLoader | No profile activation identified | `DataLoader/DataLoader/App.config` | One static set of search service settings |

No environment-specific runtime profiles or profile-composition mechanism were identified.

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `BingApiKey` | `[MASKED]` (two entries; one empty) | None | `NYCJobsWeb/Web.config` |
| `SearchServiceName` | Configured service name | None | `NYCJobsWeb/Web.config` |
| `SearchServiceApiKey` | `[MASKED]` | None | `NYCJobsWeb/Web.config` |
| `webpages:Version` | `3.0.0.0` | None | `NYCJobsWeb/Web.config` |
| `webpages:Enabled` | `false` | None | `NYCJobsWeb/Web.config` |
| `ClientValidationEnabled` | `true` | None | `NYCJobsWeb/Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | None | `NYCJobsWeb/Web.config` |
| `compilation.debug` | `true` in base configuration | Release removes attribute | `NYCJobsWeb/Web.config`, `NYCJobsWeb/Web.Release.config` |
| `compilation.targetFramework` | `4.7.2` | None | `NYCJobsWeb/Web.config` |
| `httpRuntime.targetFramework` | `4.7.2` | None | `NYCJobsWeb/Web.config` |

The search client reads an app-setting named `Searchendpoint`, while the web configuration provides `SearchServiceName`; this mismatch may leave the endpoint unset unless configuration is supplied elsewhere.

### DataLoader

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | `[MASKED]` | None | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[MASKED]` | None | `DataLoader/DataLoader/App.config` |
| Supported runtime | .NET Framework 4.5 | None | `DataLoader/DataLoader/App.config` |

No configuration values are sourced from environment variables in the application code.

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; hosted by IIS/IIS Express | Not specified | Not specified |
| DataLoader | .NET Framework 4.5; console application | Not specified | One-shot utility; no deployment count specified |

No explicit process startup flags, heap parameters, CPU/memory limits, or scaling configuration were found.

## Startup Dependency Chain

1. NYCJobsWeb starts under its configured IIS host; it requires the Azure Search endpoint and credentials to perform search operations.
2. DataLoader runs as a separate console process and requires its configured Azure Search service name and API key when it performs index operations.

Neither project defines startup health checks, readiness probes, service wait loops, or startup timeout policies.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `NYCJobsWeb` `SearchServiceApiKey` | Azure Search API key | XML application settings (`[MASKED]`) |
| `NYCJobsWeb` `BingApiKey` | Bing API key setting | XML application settings (`[MASKED]`) |
| `DataLoader` `TargetSearchServiceApiKey` | Azure Search API key | XML application settings (`[MASKED]`) |

### Secrets Provisioning Workflow

The applications read API keys directly from local XML configuration. The repository does not identify Key Vault, managed identity, environment-variable substitution, encryption, or automated deployment-time provisioning. Operators would need to provide values in the relevant configuration before running the application; no identity/access model is documented.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None identified | — | No feature-flag framework or conditional feature configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj` |
| DataLoader target framework | .NET Framework 4.5 | `DataLoader/DataLoader/DataLoader.csproj` and `App.config` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET Razor | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure Search Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 (web); 9.0.1 (loader) | Both `packages.config` files |
| MSBuild project format | Legacy non-SDK-style projects | Both `.csproj` files |
| Assessment target runtime | net10.0 | `.github/modernize/assessment/reports/assessment-config.yaml` |
