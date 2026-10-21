# JobSearch Assessment

> **Assessment limitation:** The core AppCAT analysis could not load the legacy ASP.NET `Microsoft.WebApplication.targets` on this Linux runner. This report therefore contains the completed security assessment and supplementary project analysis, not core AppCAT migration findings. The .NET 10 upgrade analyzer produced an empty project list after encountering the same missing target.

## Summary

| Metric | Count |
|---|---:|
| Total Issues | 5 |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 3 |
| .NET Upgrade Issues | 0 reported |
| Mandatory | 2 |
| Optional | 2 |
| Potential | 1 |

## Application Information

| Property | Value |
|---|---|
| Application | JobSearch |
| Language | C# and JavaScript |
| Frameworks | .NET Framework 4.7.2 web application; .NET Framework 4.5 data loader; ASP.NET MVC 5 |
| Core assessment status | Not completed: required legacy web build targets are unavailable in this environment |
| .NET target assessed | net10.0 |
| Upgrade assessment status | Analyzer output contains no project records because project loading failed |

## Security Findings

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json

- **Severity:** Mandatory
- **Story Points:** 1
- **Files:** `NYCJobsWeb/packages.config:17`, `DataLoader/DataLoader/packages.config:3`

Newtonsoft.Json 10.0.3 and 9.0.1 are below the patched 13.0.1 version. Deeply nested JSON can cause excessive resource consumption or stack overflow, resulting in denial of service. [Advisory](https://github.com/advisories/GHSA-5crp-9r3c-p9vr).

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability

- **Severity:** Mandatory
- **Story Points:** 1
- **Files:** `NYCJobsWeb/packages.config:23`

System.Text.Encodings.Web 4.6.0 is within the vulnerable range; the patched release for this version range is 4.7.2. [Advisory](https://github.com/advisories/GHSA-ghhp-997w-qr28).

### CWE-79: Improper Neutralization of Input During Web Page Generation (Cross-site Scripting)

- **Category:** Injection Attacks
- **Severity:** Optional
- **Story Points:** 8
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml`

Search index values are concatenated into HTML and inserted with jQuery `.html()` in facet and result rendering. Untrusted indexed content can be interpreted as markup or script in visitors' browsers.

### CWE-835: Loop with Unreachable Exit Condition (Infinite Loop)

- **Category:** Code Quality
- **Severity:** Potential
- **Story Points:** 3
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml`

The map cleanup loop repeats while entities remain. If `map.entities.clear()` throws, its exception is swallowed and the unchanged condition can keep the browser thread in the loop.

### CWE-798: Use of Hard-coded Credentials

- **Category:** Credentials & Secrets
- **Severity:** Optional
- **Story Points:** 5
- **Files:** `NYCJobsWeb/Views/Home/Index.cshtml`

A mapping-service API credential is embedded in the browser-side page source. The credential value is intentionally omitted from this report.

## Codebase Insights

- [Assessment Overview](facts/assessment-overview.md) — navigation to supplementary documents.
- [Architecture Diagram](facts/architecture-diagram.md) — system layers and component relationships.
- [Dependency Map](facts/dependency-map.md) — declared packages and compatibility risks.
- [API & Service Contracts](facts/api-service-contracts.md) — MVC endpoints and communication flows.
- [Data Architecture](facts/data-architecture.md) — search indexes, ownership, and data classification.
- [Configuration Inventory](facts/configuration-inventory.md) — configuration sources and sensitive settings.
- [Business Workflows](facts/business-workflows.md) — job search and data loading workflows.
