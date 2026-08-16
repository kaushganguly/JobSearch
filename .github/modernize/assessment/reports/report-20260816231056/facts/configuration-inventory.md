# Configuration & Externalized Settings Inventory

The repository uses classic .NET configuration files, Visual Studio build configurations, and NuGet packages.config files; secrets are represented as appSettings placeholders rather than an external secret-store workflow.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | ASP.NET application configuration | `NYCJobsWeb/Web.config` | Main web app settings, runtime binding redirects, target framework settings |
| Web.Debug.config | Web.config transform | `NYCJobsWeb/Web.Debug.config` | Debug transform template; no active overrides beyond comments |
| Web.Release.config | Web.config transform | `NYCJobsWeb/Web.Release.config` | Removes debug compilation attribute for Release builds |
| Views web.config | ASP.NET MVC view configuration | `NYCJobsWeb/Views/web.config` | View-engine namespaces and handler settings |
| App.config | Console application configuration | `DataLoader/DataLoader/App.config` | Target Azure Search service and API-key references |
| packages.config | NuGet package list | `NYCJobsWeb/packages.config`, `DataLoader/DataLoader/packages.config` | Package versions for each legacy project |
| Solution files | Visual Studio solution configuration | `NYCJobsWeb.sln`, `DataLoader/DataLoader.sln` | Debug and Release Any CPU solution configurations |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug AnyCPU | Visual Studio/MSBuild configuration | Development build with debug symbols and `DEBUG;TRACE` constants | Legacy MSBuild project format, packages.config restore |
| Release AnyCPU | Visual Studio/MSBuild configuration | Optimized build with `TRACE` constant and Release web.config transform | Legacy MSBuild project format, packages.config restore |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default web runtime | IIS/IIS Express loads application | `NYCJobsWeb/Web.config` | Search service name, search API key, MVC client validation, .NET Framework 4.7.2 runtime settings |
| Debug web transform | Build configuration transform | `NYCJobsWeb/Web.Debug.config` | No concrete override detected |
| Release web transform | Build configuration transform | `NYCJobsWeb/Web.Release.config` | Removes debug compilation attribute |
| Data loader runtime | Console app startup | `DataLoader/DataLoader/App.config` | Target Azure Search service name and API key |

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `BingApiKey` | Masked or blank placeholder; duplicated key appears | Default | `Web.config` appSettings |
| `SearchServiceName` | `azs-playground` | Default | `Web.config` appSettings |
| `SearchServiceApiKey` | Masked placeholder | Default | `Web.config` appSettings |
| `webpages:Version` | `3.0.0.0` | Default | `Web.config` appSettings |
| `webpages:Enabled` | `false` | Default | `Web.config` appSettings |
| `ClientValidationEnabled` | `true` | Default | `Web.config` appSettings |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `Web.config` appSettings |
| `compilation targetFramework` | `4.7.2` | Default | `Web.config` system.web |
| `httpRuntime targetFramework` | `4.7.2` | Default | `Web.config` system.web |
| Assembly binding redirects | Multiple ASP.NET, Newtonsoft.Json, and System.* versions | Default | `Web.config` runtime |

### DataLoader

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | Placeholder | Default | `App.config` appSettings |
| `TargetSearchServiceApiKey` | Masked placeholder | Default | `App.config` appSettings |
| `supportedRuntime` | `.NETFramework,Version=v4.5` | Default | `App.config` startup |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2 under IIS/IIS Express; IIS Express project URL is `http://localhost:51269/` | Not specified | Not specified |
| DataLoader | .NET Framework 4.5 console executable | Not specified | One manual console process |

## Startup Dependency Chain

1. Azure AI Search service must exist and be reachable before either application can perform useful work.
2. DataLoader reads `App.config`, deletes and recreates the `zipcodes` and `nycjobs` indexes, and imports schema/data files.
3. NYCJobsWeb starts under IIS/IIS Express, registers MVC routes, reads `Web.config`, and initializes `JobsSearch` clients for the configured search endpoint.
4. No health checks, readiness probes, or wait-for-service mechanisms were detected.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `SearchServiceApiKey` | Azure Search query/admin API key | `NYCJobsWeb/Web.config` appSettings placeholder |
| `TargetSearchServiceApiKey` | Azure Search admin API key | `DataLoader/DataLoader/App.config` appSettings placeholder |
| `BingApiKey` | Bing service API key | `NYCJobsWeb/Web.config` appSettings placeholder or blank value |

### Secrets Provisioning Workflow

No external secret provisioning workflow was detected. Operators are expected to place service names and API keys in local `.config` files or transforms before deployment. There is no Key Vault, managed identity, encrypted configuration provider, or CI/CD secret-binding configuration in the repository.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | N/A |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---:|---|
| Visual Studio solution format | Visual Studio 2013 / format 12.00 | `.sln` files |
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb.csproj`, `Web.config` |
| DataLoader target framework | .NET Framework 4.5 | `DataLoader.csproj`, `App.config` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET Razor | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Azure.Core | 1.4.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 and 9.0.1 | Web and loader `packages.config` files |
| Bootstrap | 3.4.1 | `NYCJobsWeb/packages.config` |
| jQuery | 3.1.1 | `NYCJobsWeb/packages.config` |
