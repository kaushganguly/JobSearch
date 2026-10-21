# JobSearch Assessment

## Summary

| Metric | Count |
|---|---:|
| Total Issues | 7 |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 5 |
| CWE Rules Assessed | 59 |
| CWE Rules Passed | 54 |

## Application Information

| Property | Value |
|---|---|
| Application | JobSearch |
| Languages | C# and browser JavaScript |
| Projects | NYCJobsWeb; AzureSearchBackupRestore |
| Target frameworks | .NET Framework 4.7.2; .NET Framework 4.5 |
| Application assessment | AppCAT could not load the legacy MVC project because `Microsoft.WebApplication.targets` is unavailable in this Linux environment. No core AppCAT findings were generated. |
| .NET 10 upgrade assessment | Requested target: `net10.0`. The assessment artifacts contain no analyzed projects; the project load failed on the same missing web application targets. |

## Security Findings

The following findings include all CVEs at or above the requested high-severity threshold and all confirmed findings from the six CWE reviews.

| Issue | Criticality | Story Points | Files |
|---|---|---:|---|
| [CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json](#improper-handling-of-exceptional-conditions-in-newtonsoft-json) | Mandatory | 1 | 2 |
| [CVE-2021-26701: .NET Core Remote Code Execution Vulnerability](#net-core-remote-code-execution-vulnerability) | Mandatory | 1 | 1 |
| [CWE-606: Unchecked Input for Loop Condition](#unchecked-input-for-loop-condition) | Potential | 3 | 1 |
| [CWE-772: Missing Release of Resource after Effective Lifetime](#missing-release-of-resource-after-effective-lifetime) | Potential | 3 | 1 |
| [CWE-835: Loop with Unreachable Exit Condition (Infinite Loop)](#loop-with-unreachable-exit-condition-infinite-loop) | Potential | 3 | 1 |
| [CWE-79: Cross-site Scripting](#improper-neutralization-of-input-during-web-page-generation-cross-site-scripting) | Optional | 8 | 1 |
| [CWE-798: Hard-coded Credentials](#use-of-hard-coded-credentials) | Optional | 5 | 1 |

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json

- **Severity:** Mandatory (raw advisory severity: high)
- **Story Points:** 1
- **Files:** `NYCJobsWeb/packages.config:17`, `DataLoader/DataLoader/packages.config:3`

Newtonsoft.Json versions before 13.0.1 may suffer denial of service from deeply nested JSON due to excessive resource use or stack overflow. Both declared versions, 10.0.3 and 9.0.1, are affected. [GitHub Advisory](https://github.com/advisories/GHSA-5crp-9r3c-p9vr).

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability

- **Severity:** Mandatory (raw advisory severity: critical)
- **Story Points:** 1
- **Files:** `NYCJobsWeb/packages.config:23`

System.Text.Encodings.Web 4.6.0 is in the vulnerable range `>= 4.6.0, < 4.7.2`. [GitHub Advisory](https://github.com/advisories/GHSA-ghhp-997w-qr28).

### CWE-606: Unchecked Input for Loop Condition

- **Severity:** Potential
- **Story Points:** 3
- **Files:** `NYCJobsWeb/Views/Home/JobDetails.cshtml`

In `CleanText`, the loop condition depends on job-description text. If it contains the literal characters `/\\n/g`, `indexOf` continues to find that string while the replacement does not remove it, allowing the loop to run indefinitely.

### CWE-772: Missing Release of Resource after Effective Lifetime

- **Severity:** Potential
- **Story Points:** 3
- **Files:** `DataLoader/DataLoader/AzureSearchHelper.cs`

`SendSearchRequest` returns an `HttpResponseMessage`; callers in the loader check responses without disposing them. Repeated import requests leave response and network resources unreleased until finalization.

### CWE-835: Loop with Unreachable Exit Condition (Infinite Loop)

- **Severity:** Potential
- **Story Points:** 3
- **Files:** `NYCJobsWeb/Views/Home/JobDetails.cshtml`

In `CleanText`, the loop checks `text.indexOf(/\\n/g)` but replaces actual newline characters. If input contains the literal `/\\n/g` and no newline, replacement makes no progress and the loop does not terminate.

### CWE-79: Improper Neutralization of Input During Web Page Generation (Cross-site Scripting)

- **Severity:** Optional
- **Story Points:** 8
- **Files:** `NYCJobsWeb/Views/Home/JobDetails.cshtml`

`UpdateJobDetails` writes Azure Search response values, including job descriptions and business titles, directly to the DOM using jQuery `.html()`. The content is not HTML-encoded before browser interpretation.

### CWE-798: Use of Hard-coded Credentials

- **Severity:** Optional
- **Story Points:** 5
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml`

`GetMap` assigns a literal Bing Maps API credential in browser-delivered source. The credential value is intentionally omitted from this report.

## Codebase Insights

- [Architecture Diagram](facts/architecture-diagram.md) — Application layers and component relationships.
- [Dependency Map](facts/dependency-map.md) — Declared dependencies, compatibility risks, and test packages.
- [API & Service Communication Contracts](facts/api-service-contracts.md) — MVC actions, service catalog, and communication flows.
- [Data Architecture & Persistence Layer](facts/data-architecture.md) — Azure Search indexes, data ownership, and persistence patterns.
- [Configuration & Externalized Settings Inventory](facts/configuration-inventory.md) — Configuration sources and masked sensitive settings.
- [Core Business Workflows](facts/business-workflows.md) — Job search, details retrieval, and index loading workflows.
