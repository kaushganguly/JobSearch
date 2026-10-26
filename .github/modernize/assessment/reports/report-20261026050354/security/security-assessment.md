# Security Assessment Report

**Generated:** 2026-10-26T05:10:33.268546+00:00

**Minimum CVE severity:** medium

## Summary

| Metric | Count |
|--------|-------|
| TotalFindings | 8 |
| CveCount | 5 |
| CweCount | 3 |
| TotalRulesAssessed | 59 |
| RulesPassed | 56 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 4 |
| potential | 2 |

## Scope and limitations

- Core cloud-readiness and .NET 10 upgrade analyses failed to load the legacy web project on Linux; generated zero counts are not a compatibility clearance.
- Dependency CLI failed for both legacy projects. Inventory was parsed from both packages.config files; unresolved transitive dependency coverage is not complete. Advisory matches do not prove reachable exploitation.
- CWE review covered first-party managed code, custom views/JavaScript and configuration; third-party/generated code was excluded. NOT_FOUND and RulesPassed mean no matching pattern identified, not proof of safety.
- CWE-79 is conditional on untrusted index content. CWE-665 and CWE-772 are reliability/resource-release patterns; attacker exploitation was not established.
- OData query-control syntax is interpolated from request facets at NYCJobsWeb/JobsSearch.cs:82-109. This out-of-taxonomy query injection pattern is not mislabeled SQL/resource-identifier injection and is not included in CWE checklist counts. No authorization bypass was established.
- The checkout has malformed Web.config and lacks the consumed Searchendpoint setting; runtime reachability and external index provenance were not verified. No credentials were reproduced or validated.

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Declared affected packages:
- Newtonsoft.Json 10.0.3; affected range < 13.0.1; first patched version 13.0.1
- Newtonsoft.Json 9.0.1; affected range < 13.0.1; first patched version 13.0.1

Package-level advisory match, not proof of reachable exploitation. Upgrade to a supported patched release after compatibility testing. Dependency CLI failed; packages.config inventory used; unresolved transitive coverage remains.

### CVE-2022-26907: Azure SDK for .NET Information Disclosure Vulnerability.
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Declared affected packages:
- Microsoft.Rest.ClientRuntime 2.3.20; affected range < 2.3.24; first patched version 2.3.24

Package-level advisory match, not proof of reachable exploitation. Upgrade to a supported patched release after compatibility testing. Dependency CLI failed; packages.config inventory used; unresolved transitive coverage remains.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Declared affected packages:
- System.Text.Encodings.Web 4.6.0; affected range >= 4.6.0, < 4.7.2; first patched version 4.7.2

Package-level advisory match, not proof of reachable exploitation. Upgrade to a supported patched release after compatibility testing. Dependency CLI failed; packages.config inventory used; unresolved transitive coverage remains.

### CVE-2020-11023: Potential XSS vulnerability in jQuery
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Declared affected packages:
- jQuery 3.1.1; affected range >= 1.0.3, < 3.5.0; first patched version 3.5.0

Package-level advisory match, not proof of reachable exploitation. Upgrade to a supported patched release after compatibility testing. Dependency CLI failed; packages.config inventory used; unresolved transitive coverage remains.

### CVE-2019-11358: XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Declared affected packages:
- jQuery 3.1.1; affected range >= 1.1.4, < 3.4.0; first patched version 3.4.0

Package-level advisory match, not proof of reachable exploitation. Upgrade to a supported patched release after compatibility testing. Dependency CLI failed; packages.config inventory used; unresolved transitive coverage remains.

## CWE Findings (Code-Level Patterns)

### CWE-665: Improper Initialization
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs:17, NYCJobsWeb/JobsSearch.cs:19, NYCJobsWeb/JobsSearch.cs:32-38

Static client reference fields initially default to null. Client-construction exceptions are caught and only recorded, allowing static initialization to complete with one or both clients unset. Prerequisite: Invalid configuration or another exception during client construction. Impact: Subsequent client dereferences can fail. This is a reliability pattern, not demonstrated attacker exploitation.

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs:58-65, DataLoader/DataLoader/Program.cs:59-60

First example: HttpRequestMessage is allocated, optionally receives StringContent, and is passed to SendAsync without disposal. The first caller receives HttpResponseMessage and checks success without disposing it. Neither resource has deterministic cleanup for this operation. Prerequisite: Execution of the loader HTTP operation. Impact: Missing deterministic HTTP-resource cleanup. Actual resource exhaustion or security exploitation was not established.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:313, NYCJobsWeb/Views/Home/Index.cshtml:397, NYCJobsWeb/Views/Home/JobDetails.cshtml:49

Unsafe rendering pattern: search-index fields from HomeController.Search (HomeController.cs:52-60) and detail responses (86-96) enter HTML and generated inline event handlers without contextual encoding at Index.cshtml:313-334,397-416 and JobDetails.cshtml:49-80. Conditional stored XSS requires an attacker to influence indexed/imported data; no attacker-accessible index-write endpoint was identified. This reports the unsafe sink pattern, not a demonstrated remote exploit. Deployed configuration and index provenance were not verified.
