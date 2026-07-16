# Configuration & Externalized Settings Inventory

This solution uses three configuration sources across two projects: `Web.config` (NYCJobsWeb) and `app.config` (DataLoader), both using `<appSettings>` key-value pairs with no external config server, secrets manager, or runtime profile system.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|--------|------|--------------|-------|
| Web.config | XML appSettings | `NYCJobsWeb/Web.config` | Primary runtime config for web app; contains Azure Search endpoint, API key, Bing API key, ASP.NET MVC settings |
| Web.Debug.config | XML transform | `NYCJobsWeb/Web.Debug.config` | Debug build transform — no active overrides defined (template only) |
| Web.Release.config | XML transform | `NYCJobsWeb/Web.Release.config` | Release build transform — removes `debug` attribute from `<compilation>` element |
| app.config | XML appSettings | `DataLoader/DataLoader/app.config` | Console app config; contains Azure Search target service name and API key |
| packages.config | NuGet manifest | `NYCJobsWeb/packages.config` | NuGet package references for NYCJobsWeb (legacy packages.config format) |
| packages.config | NuGet manifest | `DataLoader/DataLoader/packages.config` | NuGet package references for DataLoader |

No external config server, Azure App Configuration, environment variable injection, Docker Compose environment sections, Kubernetes ConfigMaps, or secrets manager references are used.

## Build Profiles

| Profile | Activation | Purpose | Key Changes |
|---------|-----------|---------|-------------|
| Debug | Default / manual selection in IDE or `msbuild /p:Configuration=Debug` | Development build with full debug symbols and no optimization | `DebugType=full`, `Optimize=false` |
| Release | Manual selection / CI pipeline `msbuild /p:Configuration=Release` | Production build with optimizations; applies `Web.Release.config` transform | `DebugType=pdbonly`, `Optimize=true`; `<compilation debug>` attribute removed from Web.config |

No Maven/Gradle profiles, conditional compilation symbols, or environment-specific build flags are defined beyond the standard Debug/Release pair.

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---------|-----------------|-------------|--------------|
| (Single profile) | N/A — no runtime profile system | `Web.config` | All settings are static in `Web.config`; no `ASPNETCORE_ENVIRONMENT` or `appsettings.{env}.json` — this is an ASP.NET 4.7.2 app, not ASP.NET Core |

There is no runtime environment profile system. Configuration values are hardcoded in `Web.config` and `app.config`. Environment-specific configuration requires manual file editing or a deployment-time config transform.

## Properties Inventory

### NYCJobsWeb (`Web.config`)

| Property Key | Default Value | Profiles | Source |
|-------------|--------------|---------|--------|
| `Searchendpoint` | _(not set — must be configured)_ | All | `Web.config` appSettings |
| `SearchServiceApiKey` | `<api-key>` (placeholder) | All | `Web.config` appSettings |
| `BingApiKey` | _(empty string / placeholder)_ | All | `Web.config` appSettings (duplicated key — second entry overrides first) |
| `webpages:Version` | `3.0.0.0` | All | `Web.config` appSettings |
| `webpages:Enabled` | `false` | All | `Web.config` appSettings |
| `ClientValidationEnabled` | `true` | All | `Web.config` appSettings |
| `UnobtrusiveJavaScriptEnabled` | `true` | All | `Web.config` appSettings |

> **Note**: The `BingApiKey` key appears **twice** in `Web.config` — the first occurrence has a placeholder `[ENTER BING API KEY]` and the second is an empty string. The second entry overrides the first; the effective runtime value is an empty string, meaning Bing geocoding is disabled unless the config is corrected.

### DataLoader (`app.config`)

| Property Key | Default Value | Profiles | Source |
|-------------|--------------|---------|--------|
| `TargetSearchServiceName` | `[TARGET SEARCH SERVICE - Excluding search.windows.net]` (placeholder) | All | `app.config` appSettings |
| `TargetSearchServiceApiKey` | `[TARGET SEARCH SERVICE API KEY]` (placeholder) | All | `app.config` appSettings |

### ASP.NET Runtime Properties (`Web.config` system sections)

| Property | Value | Notes |
|---------|-------|-------|
| `system.web/compilation[@debug]` | `true` (Debug), removed (Release) | Debug compilation flag; Release transform removes it |
| `system.web/httpRuntime[@targetFramework]` | `4.7.2` | ASP.NET runtime version |
| `system.web/compilation[@targetFramework]` | `4.7.2` | Compilation target framework |
| CORS (Azure Search) | `allowedOrigins: ["*"]` | Configured in the Azure Search index schema, not in web.config |

## Startup Parameters & Resource Requirements

| Service | Runtime Options | Memory | Instance Count |
|---------|----------------|--------|----------------|
| NYCJobsWeb | IIS-hosted ASP.NET 4.7.2 process; no explicit JVM or CLR heap settings | Not specified — IIS default application pool limits | 1 (no scaling configuration) |
| DataLoader | .NET 4.5 console process; `supportedRuntime version="v4.0"` | Not specified | 1 (run-once utility) |

No JVM parameters, Docker memory limits, Kubernetes resource requests/limits, or auto-scaling rules are defined.

## Startup Dependency Chain

There is no orchestrated multi-service startup. The two components are independent:

1. **DataLoader** (prerequisite): Must be run first to create and populate Azure Cognitive Search indexes (`nycjobs`, `zipcodes`). There is no automated dependency check or readiness probe — if run before indexes exist, the web app will fail silently with null search results.
2. **NYCJobsWeb**: Starts via IIS application pool initialization. Reads `Web.config` at first request and initializes the `JobsSearch` static constructor. If the Azure Search endpoint is unreachable at startup, the static constructor catches the exception and stores the error message, but no circuit breaker or retry mechanism prevents the application from starting.

No health checks, readiness probes, Docker Compose `depends_on`, or Kubernetes liveness probes are configured.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Location | Storage |
|-----------------|------|----------|---------|
| `SearchServiceApiKey` | Azure Search Query API key | `NYCJobsWeb/Web.config` appSettings | Plaintext in source file — [MASKED] |
| `Searchendpoint` | Azure Search service URL | `NYCJobsWeb/Web.config` appSettings | Plaintext in source file |
| `BingApiKey` | Bing Maps API key | `NYCJobsWeb/Web.config` appSettings | Plaintext in source file — currently empty |
| `TargetSearchServiceApiKey` | Azure Search Admin API key | `DataLoader/app.config` appSettings | Plaintext in source file — [MASKED] (placeholder value in repo) |
| `TargetSearchServiceName` | Azure Search service name | `DataLoader/app.config` appSettings | Plaintext in source file — placeholder value in repo |

### Secrets Provisioning Workflow

There is no automated secrets provisioning workflow. All secrets are configured by **manually editing `Web.config` and `app.config`** before deployment. The Azure Search API key and endpoint are committed to the source repository as placeholder values and must be replaced by the deploying developer.

No Azure Key Vault, AWS Secrets Manager, HashiCorp Vault, environment variable injection, GitHub Actions secrets binding, or DPAPI encryption is implemented. This represents a significant operational security gap: anyone with repository read access who replaces the placeholders with real values would commit live credentials to source control.

**Recommended remediation**: Externalize secrets to Azure Key Vault with a managed identity binding, or at minimum use `ASPNETCORE_` environment variables (after migrating to .NET 8) or IIS application pool environment overrides rather than `Web.config` literals.

## Feature Flags

No feature flag framework, `@ConditionalOnProperty`-style toggles, or runtime feature switches are implemented. There are no A/B testing flags, gradual rollout configurations, or environment-conditional beans/services. All application behavior is statically compiled.

| Flag Name | Default | Controlled By |
|-----------|---------|--------------|
| _(none)_ | N/A | N/A |

## Framework & Runtime Versions

| Component | Version | Source |
|-----------|---------|--------|
| Target Framework (NYCJobsWeb) | .NET Framework 4.7.2 | `NYCJobsWeb.csproj`, `Web.config` httpRuntime |
| Target Framework (DataLoader) | .NET Framework 4.5 | `DataLoader.csproj`, `app.config` supportedRuntime |
| ASP.NET MVC | 5.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET Razor | 3.2.2 | `NYCJobsWeb/packages.config` |
| ASP.NET WebPages | 3.2.2 | `NYCJobsWeb/packages.config` |
| Azure.Search.Documents | 11.1.1 | `NYCJobsWeb/packages.config` |
| Azure.Core | 1.4.1 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (NYCJobsWeb) | 10.0.3 | `NYCJobsWeb/packages.config` |
| Newtonsoft.Json (DataLoader) | 9.0.1 | `DataLoader/packages.config` |
| Bootstrap | 3.4.1 | `NYCJobsWeb/packages.config` |
| jQuery | 3.1.1 | `NYCJobsWeb/packages.config` |
| Modernizr | 2.8.3 | `NYCJobsWeb/packages.config` |
| BingGeocodingHelper | 1.1 | `NYCJobsWeb/packages.config` |
| Microsoft.Spatial | 7.5.3 | `NYCJobsWeb/packages.config` |
| Build Tool | MSBuild (Visual Studio / .NET SDK) | `*.csproj` (legacy non-SDK format) |
| NuGet format | packages.config (legacy) | `NYCJobsWeb/packages.config`, `DataLoader/packages.config` |
