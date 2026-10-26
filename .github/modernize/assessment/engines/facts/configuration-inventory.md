# Configuration & Externalized Settings Inventory

The repository contains two XML application configuration files and legacy MSBuild project settings. No environment-specific profiles or external secret-store integration were identified; service credentials are represented by placeholders in configuration.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web configuration | XML application settings and runtime config | `NYCJobsWeb/Web.config` | Contains application settings, framework compilation settings, and binding redirects |
| Importer configuration | XML application settings | `DataLoader/DataLoader/App.config` | Contains target search service name and API key placeholders |
| Web project | Legacy MSBuild project | `NYCJobsWeb/NYCJobsWeb.csproj` | Targets .NET Framework 4.7.2; declares Debug and Release configurations |
| Importer project | Legacy MSBuild project | `DataLoader/DataLoader/DataLoader.csproj` | Targets .NET Framework 4.5; declares Debug and Release configurations |
| Solution files | Visual Studio solutions | `NYCJobsWeb.sln`, `DataLoader/DataLoader.sln` | Separate solution files; no unified solution found |
| Search index definitions | Azure Search schema JSON | `NYCJobsWeb/Schema_and_Data/*.schema` | Defines the `nycjobs` and `zipcodes` indexes |

No Azure App Configuration, Key Vault, Vault, external configuration repository, container manifest, or Kubernetes configuration was identified.

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | MSBuild configuration `Debug` | Development build with debug symbols and no optimization | Project package references |
| Release | MSBuild configuration `Release` | Optimized build without full debug symbols | Project package references |

The legacy projects contain no additional named build profiles or conditional package sources.

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | No profile selector found | `NYCJobsWeb/Web.config` | Web and Search service settings |
| Default | No profile selector found | `DataLoader/DataLoader/App.config` | Target Search service settings |

No environment-specific configuration files, environment name selectors, or profile composition rules were found.

## Properties Inventory

### NYCJobsWeb

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `BingApiKey` | `[MASKED]` / empty (duplicate declarations) | Default | `NYCJobsWeb/Web.config` |
| `SearchServiceName` | `azs-playground` | Default | `NYCJobsWeb/Web.config` |
| `SearchServiceApiKey` | `[MASKED]` | Default | `NYCJobsWeb/Web.config` |
| `webpages:Version` | `3.0.0.0` | Default | `NYCJobsWeb/Web.config` |
| `webpages:Enabled` | `false` | Default | `NYCJobsWeb/Web.config` |
| `ClientValidationEnabled` | `true` | Default | `NYCJobsWeb/Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `NYCJobsWeb/Web.config` |
| `compilation.debug` | `true` | Default | `NYCJobsWeb/Web.config` |
| `compilation.targetFramework` | `4.7.2` | Default | `NYCJobsWeb/Web.config` |
| `httpRuntime.targetFramework` | `4.7.2` | Default | `NYCJobsWeb/Web.config` |

`JobsSearch` reads a setting named `Searchendpoint`; that key is not present in the inspected `Web.config`, which instead declares `SearchServiceName`.

### AzureSearchBackupRestore

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `TargetSearchServiceName` | `[MASKED]` placeholder | Default | `DataLoader/DataLoader/App.config` |
| `TargetSearchServiceApiKey` | `[MASKED]` | Default | `DataLoader/DataLoader/App.config` |
| Supported runtime | .NET Framework 4.5 | Default | `DataLoader/DataLoader/App.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| NYCJobsWeb | .NET Framework 4.7.2; no explicit runtime flags found | Not specified | Not specified |
| AzureSearchBackupRestore | .NET Framework 4.5; no explicit runtime flags found | Not specified | Not applicable |

No heap, CPU, memory limit, or instance scaling configuration was found in the repository.

## Startup Dependency Chain

1. NYCJobsWeb starts under its hosting environment and initializes MVC routes. Search operations require the configured Azure AI Search endpoint and credentials to be valid.
2. AzureSearchBackupRestore runs as an independent console process and sends requests to the target Azure AI Search service. No automatic startup ordering, readiness checks, or wait mechanism is configured.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| Web `SearchServiceApiKey` | Azure AI Search API key | XML application setting, `[MASKED]` placeholder |
| Web `BingApiKey` | Bing API key | XML application setting, placeholder and empty duplicate |
| Importer `TargetSearchServiceApiKey` | Azure AI Search API key | XML application setting, `[MASKED]` placeholder |

### Secrets Provisioning Workflow

No automated secrets workflow, vault reference, managed identity, or identity access policy is present in the inspected files. The placeholders indicate that deployment operators must provide service keys in the application configuration; no encryption or protected-configuration mechanism is declared.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| No feature flags identified | — | No feature flag framework or custom toggle configuration found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| NYCJobsWeb target framework | .NET Framework 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj`, `NYCJobsWeb/Web.config` |
| AzureSearchBackupRestore target framework | .NET Framework 4.5 | `DataLoader/DataLoader/DataLoader.csproj`, `DataLoader/DataLoader/App.config` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json | 10.0.3 (web), 9.0.1 (importer) | Project `packages.config` files |
| MSBuild project format | Legacy non-SDK style | Project files |
