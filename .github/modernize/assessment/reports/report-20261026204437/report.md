# Application Assessment Report

## Summary

| Metric | Count |
|---|---:|
| Security findings | 8 |
| CVE dependency findings (medium or higher) | 5 |
| CWE code findings | 3 |
| CWE rules assessed | 59 |
| CWE rules with no finding | 56 |
| Core AppCAT project findings | Not available |
| .NET upgrade issues | Not available |

## Application Information

| Property | Value |
|---|---|
| Application | JobSearch / NYCJobsWeb |
| Language | C# |
| Frameworks | ASP.NET MVC 5; .NET Framework 4.7.2 web project; .NET Framework 4.5 DataLoader |
| Dependencies | NuGet packages declared in legacy `packages.config` files |

## Assessment Limitations

The core AppCAT run was attempted but could not load the legacy ASP.NET WebApplication project because `Microsoft.WebApplication.targets` is not installed in the Linux environment. The .NET 10 upgrade assessment was also invoked; its generated report contains zero analyzed projects and zero issues after the same project-loading failure. No AppCAT or upgrade findings should be inferred from these unavailable analyses.

The Markdown renderer could not process the security-only `report.json` because AppCAT did not populate its required project list. This report was rendered manually using the available security results and supplementary facts.

## Security Findings

### CVE dependency findings

Five active advisories at the requested minimum severity of medium affect declared dependencies:

| ID | Severity | Dependency | Evidence |
|---|---|---|---|
| CVE-2024-21907 | High | Newtonsoft.Json 10.0.3 and 9.0.1 | `NYCJobsWeb/packages.config:17`; `DataLoader/DataLoader/packages.config:3` |
| CVE-2022-26907 | Medium | Microsoft.Rest.ClientRuntime 2.3.20 | `NYCJobsWeb/packages.config:12` |
| CVE-2021-26701 | Critical | System.Text.Encodings.Web 4.6.0 | `NYCJobsWeb/packages.config:23` |
| CVE-2020-11023 | Medium | jQuery 3.1.1 | `NYCJobsWeb/packages.config:7` |
| CVE-2019-11358 | Medium | jQuery 3.1.1 | `NYCJobsWeb/packages.config:7` |

Transitive dependencies could not be resolved because `dotnet list package --include-transitive` does not support these `packages.config` projects, and the web project cannot load its Visual Studio WebApplication targets on this host. The CVE scan therefore used direct package versions declared in the project manifests.

### CWE code findings

| ID | Category | Severity | Evidence |
|---|---|---|---|
| CWE-79 | Injection Attacks | Optional | Search results are concatenated into HTML and inserted with `.html()` in `NYCJobsWeb/Views/Home/Index.cshtml:397-416`; job detail fields are also inserted as HTML in `NYCJobsWeb/Views/Home/JobDetails.cshtml:49-80`. |
| CWE-606 | Code Quality | Potential | `CleanText` uses external job text in a loop condition that can remain true indefinitely: `NYCJobsWeb/Views/Home/JobDetails.cshtml:31-32`. |
| CWE-835 | Code Quality | Potential | The same `CleanText` loop has an unreachable exit for input containing the coerced regex string: `NYCJobsWeb/Views/Home/JobDetails.cshtml:31-32`. |

The other assessed file/path, injection, memory-safety, code-quality, credentials/secrets, and concurrency rules had no finding. See `security-assessment.md` for the complete merged result and rule counts.

## .NET 10 Upgrade Assessment

The requested target was `net10.0`. The scenario output is in [the upgrade assessment directory](scenarios/dotnet-version-upgrade/assessment.md). Its `assessment.json` reports zero projects and zero issues, so it is not evidence of successful project compatibility analysis.

## Codebase Insights

- [Assessment overview](facts/assessment-overview.md)
- [Architecture diagram](facts/architecture-diagram.md)
- [Dependency map](facts/dependency-map.md)
- [API and service contracts](facts/api-service-contracts.md)
- [Data architecture](facts/data-architecture.md)
- [Configuration inventory](facts/configuration-inventory.md)
- [Business workflows](facts/business-workflows.md)
- [Security assessment](security-assessment.md)
