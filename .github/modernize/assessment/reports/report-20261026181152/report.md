# JobSearch Assessment Report

**Generated:** 2026-10-26T18:11:52Z

> **Assessment limitation:** The core AppCAT run could not load the legacy web project because the Visual Studio WebApplication build targets are unavailable in this Linux environment. The .NET 10 upgrade assessment produced files but reported zero projects, so no compatibility conclusion can be drawn from it.

## Summary

| Metric | Count |
|---|---:|
| Total Issues | 6 security findings |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 4 |
| Total CWE Rules Assessed | 59 |
| CWE Rules Passed | 55 |
| Mandatory | 2 |
| Optional | 2 |
| Potential | 2 |

## Application Information

| Property | Value |
|---|---|
| Application | JobSearch |
| Project type | ASP.NET MVC web application and console data loader |
| Frameworks | .NET Framework 4.7.2 and .NET Framework 4.5 |
| Assessment target | .NET 10 (`net10.0`) |
| Core assessment status | Incomplete; AppCAT could not load the legacy web project |

## CVE Findings

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json

- **Severity:** Mandatory (high)
- **Files:** `NYCJobsWeb/packages.config:17`, `DataLoader/DataLoader/packages.config:3`

Newtonsoft.Json versions before 13.0.1 can consume excessive CPU and memory or cause a stack overflow when processing deeply nested JSON. [Advisory](https://github.com/advisories/GHSA-5crp-9r3c-p9vr).

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability in System.Text.Encodings.Web

- **Severity:** Mandatory (critical)
- **Files:** `NYCJobsWeb/packages.config:23`

System.Text.Encodings.Web 4.6.0 is within the affected version range for a remote code execution vulnerability involving text encoding. [Advisory](https://github.com/advisories/GHSA-ghhp-997w-qr28).

## CWE Findings

### Improper Neutralization of Input During Web Page Generation (Cross-site Scripting)

- **Rule:** CWE-79
- **Severity:** Optional
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml:397-416`, `NYCJobsWeb/Views/Home/Index.cshtml:316-319`, `NYCJobsWeb/Views/Home/JobDetails.cshtml:49-73`

Search result and facet values are concatenated into HTML and inserted through jQuery `.html()`. Markup or script in indexed content may execute for users viewing results.

### Missing Release of Resource after Effective Lifetime

- **Rule:** CWE-772
- **Severity:** Potential
- **Files:** `DataLoader/DataLoader/AzureSearchHelper.cs:58-65`, `DataLoader/DataLoader/Program.cs:58-60`, `DataLoader/DataLoader/Program.cs:82-83`, `DataLoader/DataLoader/Program.cs:97-103`

The data loader does not dispose request and response objects created for Search REST API calls, including responses in the repeated document-import loop.

### Loop with Unreachable Exit Condition (Infinite Loop)

- **Rule:** CWE-835
- **Severity:** Potential
- **Files:** `NYCJobsWeb/Views/Home/JobDetails.cshtml:31-32`

`CleanText` tests for a regex's string representation using `indexOf`, then replaces actual newline characters. If the indexed job text contains that literal sequence, the replacement does not change the text and the loop does not terminate.

### Use of Hard-coded Credentials

- **Rule:** CWE-798
- **Severity:** Optional
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml:114`

A map-service API credential is embedded in browser-delivered JavaScript. Its value is not reproduced here.

## .NET 10 Upgrade Assessment

The assessment requested `net10.0`. The tool generated scenario data, but project discovery returned zero projects because the legacy web project's Visual Studio WebApplication target is unavailable. See the [scenario assessment](scenarios/dotnet-version-upgrade/assessment.md), [machine-readable assessment](scenarios/dotnet-version-upgrade/assessment.json), and [dependency health](scenarios/dotnet-version-upgrade/dependencies-health.json).

## Codebase Insights

Supplementary documents are AI-generated and may contain inaccuracies; review them against the source:

1. [Architecture Diagram](facts/architecture-diagram.md)
2. [Dependency Map](facts/dependency-map.md)
3. [API & Service Communication Contracts](facts/api-service-contracts.md)
4. [Data Architecture & Persistence Layer](facts/data-architecture.md)
5. [Configuration & Externalized Settings Inventory](facts/configuration-inventory.md)
6. [Core Business Workflows](facts/business-workflows.md)
