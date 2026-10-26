# JobSearch Assessment

## Summary

| Metric | Value |
|---|---:|
| Total reported security findings | 6 |
| CVE dependency findings | 2 |
| CWE code findings | 4 |
| .NET upgrade assessment | Incomplete; project load failed |

> The core AppCAT assessment did not produce a report because the environment does not contain the legacy MSBuild web-application targets required by `NYCJobsWeb.csproj`. The .NET 10 upgrade assessment likewise generated no analyzed projects or issues. The security findings and supplementary facts below were completed independently; this is not a complete AppCAT assessment.

## Application Information

| Property | Value |
|---|---|
| Language | C# |
| Frameworks | ASP.NET MVC 5; .NET Framework 4.7.2 (web); .NET Framework 4.5 (DataLoader) |
| Build tools | Legacy MSBuild project format |
| Projects | NYCJobsWeb and DataLoader |

## .NET 10 Upgrade Assessment

The requested target was `net10.0`. The assessment tool could not load the web project because `/usr/share/dotnet/sdk/10.0.401/Microsoft/VisualStudio/v18.0/WebApplications/Microsoft.WebApplication.targets` is unavailable. The resulting scenario report records zero analyzed projects, packages, and issues, so it does not establish upgrade compatibility.

See [scenario assessment details](scenarios/dotnet-version-upgrade/assessment.md).

## Security Issues

The CVE scan used the medium minimum severity threshold. Two qualifying dependency findings were identified.

| ID | Finding | Severity | Files |
|---|---|---|---|
| CVE-2024-21907 | Improper handling of exceptional conditions in Newtonsoft.Json | High | `NYCJobsWeb/packages.config:17`; `DataLoader/DataLoader/packages.config:3` |
| CVE-2021-26701 | Remote code execution in System.Text.Encodings.Web | Critical | `NYCJobsWeb/packages.config:23` |
| CWE-79 | Unencoded search document fields rendered as HTML | Optional | `NYCJobsWeb/Views/Home/JobDetails.cshtml:49-52` |
| CWE-772 | HTTP responses are not disposed after use | Potential | `DataLoader/DataLoader/AzureSearchHelper.cs:52-65` |
| CWE-835 | Map entity cleanup loop can fail to terminate if `clear()` throws | Potential | `NYCJobsWeb/Views/Home/Index.cshtml:193-199` |
| CWE-798 | Client-side Bing Maps API credential is hard-coded in the page | Optional | `NYCJobsWeb/Views/Home/Index.cshtml:114` |

The vulnerable package versions and full assessment evidence are included in `report.json`; the security scan files are under `.github/modernize/assessment/engines/security/`.

## Codebase Insights

- [Assessment Facts Overview](facts/assessment-overview.md)
- [Architecture Diagram](facts/architecture-diagram.md)
- [Dependency Map](facts/dependency-map.md)
- [API & Service Communication Contracts](facts/api-service-contracts.md)
- [Data Architecture & Persistence Layer](facts/data-architecture.md)
- [Configuration & Externalized Settings Inventory](facts/configuration-inventory.md)
- [Core Business Workflows](facts/business-workflows.md)
