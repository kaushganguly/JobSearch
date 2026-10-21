# .NET 10 upgrade summary

**Task:** `001-upgrade-dotnet-to-net10`
**Status:** Success

## Changes

- Converted NYCJobsWeb and AzureSearchBackupRestore to SDK-style projects targeting `net10.0`, and added both projects to `NYCJobsWeb.sln`.
- Replaced NYCJobsWeb's MVC 5 / Global.asax / Web.config startup with ASP.NET Core MVC hosting, routing, dependency injection, configuration, and Razor imports. Kept the existing search, suggestion, lookup, and view routes; JSON property casing remains compatible with the client.
- Moved site assets under `wwwroot`, updated asset paths, and configured Azure Search and Bing Maps through appsettings/environment configuration.
- Migrated DataLoader off `packages.config` and App.config. It now reads `TARGET_SEARCH_SERVICE_NAME` and `TARGET_SEARCH_SERVICE_API_KEY`, locates the repository data files, and uses a supported Azure Search API version.
- Removed obsolete MVC 5 configuration/project files and vulnerable unused tour/jQuery source assets. Updated the page to use current jQuery and jQuery UI CDN versions.

## Validation

- **passBuild:** PASS — `dotnet build NYCJobsWeb.sln --no-restore` (0 warnings, 0 errors).
- **passUnitTests:** PASS — `dotnet test NYCJobsWeb.sln --no-restore` exited successfully. No test projects are configured, so no test cases were discovered.
- **securityComplianceCheck:** PASS — dependency advisory check found no vulnerabilities; final CodeQL scan found 0 alerts. Secret scan completed before commit.
- **Runtime smoke test:** PASS — the ASP.NET Core home page returned HTTP 200.
- Confirmed no remaining MVC 5 / .NET Framework references in either upgraded project.

## Files changed

- `NYCJobsWeb.sln`
- NYCJobsWeb project files: `NYCJobsWeb.csproj`, `Program.cs`, `appsettings.json`, `README.md`, `JobsSearch.cs`, `Controllers/HomeController.cs`, `Controllers/ErrorController.cs`, `Models/Jobs.cs`
- NYCJobsWeb views: `Views/_ViewImports.cshtml`, `Views/Home/Index.cshtml`, `Views/Home/JobDetails.cshtml`, `Views/Shared/_Layout.cshtml`
- Website assets relocated from `Content`, `Fonts`, `Images`, and `Scripts` into `wwwroot`; obsolete tour and jQuery source files removed.
- DataLoader: `DataLoader.csproj`, `Program.cs`, `AzureSearchHelper.cs`, and `DataLoader/README.md`; removed `App.config`, `packages.config`, and legacy assembly metadata.
- Removed NYCJobsWeb's legacy MVC 5 project, startup, Web.config, packages.config, and assembly metadata files.
