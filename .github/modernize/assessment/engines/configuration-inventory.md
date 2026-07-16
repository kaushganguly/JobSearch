# Configuration & Externalized Settings Inventory

The solution uses two configuration sources — a single `Web.config` (with Debug/Release XDT transforms) for the web application and an `app.config` for the DataLoader console tool — with no environment-profile system, no secret store integration, and no externalized configuration service.

## Configuration Sources

| Source | Type | Path / Location | Notes |
|--------|------|----------------|-------|
| `Web.config` | ASP.NET XML configuration | `NYCJobsWeb/Web.config` | Primary configuration for the web application; contains `<appSettings>`, `<system.web>`, and `<runtime>` assembly binding redirects |
| `Web.Debug.config` | XDT transform | `NYCJobsWeb/Web.Debug.config` | Debug-environment transform; currently contains no active transformations (template only) |
| `Web.Release.config` | XDT transform | `NYCJobsWeb/Web.Release.config` | Release-environment transform; removes `debug="true"` from `<compilation>` at publish time |
| `app.config` | .NET XML configuration | `DataLoader/DataLoader/app.config` | Console tool configuration with Azure Search target service name and API key |
| `packages.config` (NYCJobsWeb) | NuGet package manifest | `NYCJobsWeb/packages.config` | Declares NuGet package versions for the web project |
| `packages.config` (DataLoader) | NuGet package manifest | `DataLoader/DataLoader/packages.config` | Declares NuGet package versions for the DataLoader project |

No external config server (Spring Cloud Config, Azure App Configuration, AWS AppConfig), Docker Compose, Kubernetes manifests, `.env` files, or secret store references (Azure Key Vault, HashiCorp Vault) were found.

## Build Profiles

| Profile | Activation | Purpose | Key Changes |
|---------|-----------|---------|------------|
| Debug | Default in Visual Studio / MSBuild `Configuration=Debug` | Development and local debugging | `debug="true"` set in `<compilation>`; assembly PDBs generated; no XDT overrides applied |
| Release | Manual — MSBuild `Configuration=Release` or Web Publish | Production deployment package | XDT `Web.Release.config` removes `debug="true"` attribute from `<compilation>` at publish time |

No Maven, Gradle, npm, or multi-stage build profiles exist. There are no conditional compilation symbols or MSBuild property overrides beyond the standard Debug/Release distinction.

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---------|-----------------|-------------|--------------|
| (Default) | Always active | `Web.config` | All settings are in the single base config file |
| Debug | `Configuration=Debug` build | `Web.Debug.config` (no-op) | No active overrides — template file only |
| Release | `Configuration=Release` build + Web Publish | `Web.Release.config` | Removes `debug="true"` from `<compilation>` |

There is no runtime environment concept (e.g., `ASPNETCORE_ENVIRONMENT`, `spring.profiles.active`). The application is configured purely at publish time via XDT transforms. There is no mechanism to swap configuration values between development, staging, and production environments without rebuilding and republishing.

## Properties Inventory

### NYCJobsWeb (`Web.config` — `<appSettings>`)

| Property Key | Default Value | Profiles | Source | Notes |
|-------------|--------------|---------|--------|-------|
| `BingApiKey` | `[ENTER BING API KEY]` (placeholder) | All | `Web.config` static | Duplicated key — appears twice in file; second entry overrides with empty string; effectively unconfigured |
| `SearchServiceName` | `azs-playground` | All | `Web.config` static | Azure AI Search service name (excluding `.search.windows.net`) |
| `SearchServiceApiKey` | `<api-key>` (placeholder) | All | `Web.config` static | Azure AI Search Query API key — currently a placeholder value |
| `Searchendpoint` | _(not found in Web.config)_ | All | `Web.config` static | Referenced in `JobsSearch.cs` as `ConfigurationManager.AppSettings["Searchendpoint"]`; absent from current `Web.config` — would cause `NullReferenceException` at startup |
| `webpages:Version` | `3.0.0.0` | All | `Web.config` static | ASP.NET Web Pages framework version binding |
| `webpages:Enabled` | `false` | All | `Web.config` static | Disables Razor Pages alongside MVC |
| `ClientValidationEnabled` | `true` | All | `Web.config` static | Enables ASP.NET MVC client-side validation |
| `UnobtrusiveJavaScriptEnabled` | `true` | All | `Web.config` static | Enables unobtrusive JavaScript mode for validation |

### DataLoader (`app.config` — `<appSettings>`)

| Property Key | Default Value | Profiles | Source | Notes |
|-------------|--------------|---------|--------|-------|
| `TargetSearchServiceName` | `[TARGET SEARCH SERVICE - Excluding search.windows.net]` (placeholder) | All | `app.config` static | Azure AI Search service name for index provisioning |
| `TargetSearchServiceApiKey` | `[TARGET SEARCH SERVICE API KEY]` (placeholder) | All | `app.config` static | Azure AI Search Admin API key for write operations |

## Startup Parameters and Resource Requirements

| Service | Runtime Options | Memory | CPU | Instance Count |
|---------|----------------|--------|-----|---------------|
| NYCJobsWeb | IIS / IIS Express application pool defaults; no custom JVM or CLR startup options specified | Not configured | Not configured | 1 (no scale-out configuration) |
| DataLoader | .NET Framework 4.5 console process; no custom CLR options | Not configured | Not configured | 1 (single run) |

No Docker/container configuration, Kubernetes resource limits, or cloud deployment manifests exist. The application is designed for deployment on IIS (Windows) using Web Deploy or manual file copy.

## Startup Dependency Chain

The `JobsSearch` constructor runs during the first HTTP request that triggers ASP.NET's lazy controller instantiation (or at application startup via static initialization). It reads `Searchendpoint` and `SearchServiceApiKey` from `Web.config` and instantiates two `SearchClient` objects. If the Azure AI Search endpoint is unreachable or the key is invalid, the exception is caught silently and the `errorMessage` static field is set — the application starts but all search operations return `null`.

```
[IIS/IIS Express start]
  → Global.asax Application_Start
    → AreaRegistration.RegisterAllAreas()
    → RouteConfig.RegisterRoutes()
  → First HTTP request
    → HomeController constructed
      → JobsSearch static ctor (reads Web.config, creates SearchClient)
        → Azure AI Search endpoint reachable? → ready
                                               → fail silently, errorMessage set
```

There are no Kubernetes readiness probes, Docker Compose `depends_on` health checks, or Spring Cloud Config retry mechanisms.

## Secrets and Sensitive Configuration

| Secret Reference | Type | Location | Storage Method |
|-----------------|------|----------|---------------|
| `SearchServiceApiKey` / `Searchendpoint` | Azure AI Search Query API key + endpoint URL | `Web.config` `<appSettings>` | Plaintext in source-controlled XML file |
| `BingApiKey` | Bing Maps API key | `Web.config` `<appSettings>` | Plaintext in source-controlled XML file (currently empty placeholder) |
| `TargetSearchServiceApiKey` | Azure AI Search Admin API key | `DataLoader/app.config` `<appSettings>` | Plaintext in source-controlled XML file |
| `TargetSearchServiceName` | Azure AI Search service name | `DataLoader/app.config` `<appSettings>` | Plaintext in source-controlled XML file |

> **Security note**: All secrets are stored as plaintext in XML configuration files that are committed to source control. No encryption (DPAPI, Azure Key Vault, ASP.NET DPAPI-protected config sections), environment-variable injection, or secret store integration is used. The `SearchServiceApiKey` value is currently the placeholder `<api-key>` — the actual deployed key is expected to be substituted manually before deployment.

### Secrets Provisioning Workflow

There is no automated secrets provisioning workflow. The current approach is fully manual:

1. A developer manually edits `Web.config` to insert the real `Searchendpoint` URL and `SearchServiceApiKey` value.
2. The edited `Web.config` is included in the Web Deploy package or copied to the IIS server.
3. The DataLoader operator edits `app.config` to insert `TargetSearchServiceName` and `TargetSearchServiceApiKey` before running the console tool.

No managed identity, service principal, Key Vault reference, or CI/CD secret injection is configured. Secrets are expected to be present as literal strings in the deployed config files.

## Feature Flags

No feature flag framework (LaunchDarkly, .NET FeatureManagement, Spring Feature Flags), conditional beans/services, `@ConditionalOnProperty`, or A/B testing configuration was found. There are no runtime-togglable features in the application.

| Flag Name | Default | Controlled By |
|-----------|---------|--------------|
| — | — | None |

## Framework and Runtime Versions

| Component | Version | Source |
|-----------|---------|--------|
| .NET Framework (NYCJobsWeb) | 4.7.2 | `NYCJobsWeb/NYCJobsWeb.csproj` `<TargetFrameworkVersion>` |
| .NET Framework (DataLoader) | 4.5 | `DataLoader/DataLoader/DataLoader.csproj` `<TargetFrameworkVersion>` |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET Razor | 3.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET Web Pages | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents (SDK) | 11.1.1 | `NYCJobsWeb/packages.config` |
| Azure.Core | 1.4.1 | `NYCJobsWeb/packages.config` |
| BingGeocodingHelper | 1.1 | `NYCJobsWeb/packages.config` |
| Bootstrap | 3.4.1 | `NYCJobsWeb/packages.config` |
| jQuery | 3.1.1 | `NYCJobsWeb/packages.config` |
| Modernizr | 2.8.3 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (NYCJobsWeb) | 10.0.3 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (DataLoader) | 9.0.1 | `DataLoader/DataLoader/packages.config` |
| MSBuild / Visual Studio ToolsVersion | 12.0 | `NYCJobsWeb/NYCJobsWeb.csproj` `ToolsVersion` attribute |
| IIS Express (dev server) | Default for Visual Studio | `NYCJobsWeb/NYCJobsWeb.csproj` `<UseIISExpress>true` |
