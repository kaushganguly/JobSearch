# Security Assessment Report

**Generated:** 2026-10-26T19:14:56.220023Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 6 |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 4 |
| Total Rules Assessed | 59 |
| Rules Passed | 55 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 2 |
| potential | 2 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Newtonsoft.Json may consume excessive CPU and memory or trigger a stack overflow when processing deeply nested JSON, causing denial of service.

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json 10.0.3 (declared at NYCJobsWeb/packages.config:17)
  - Newtonsoft.Json 9.0.1 (declared at DataLoader/DataLoader/packages.config:3)

Recommended fix:
  - Upgrade Newtonsoft.Json to 13.0.1 or later.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability in System.Text.Encodings.Web
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): System.Text.Encodings.Web versions 4.6.0 through 4.7.1 are affected by a remote code execution vulnerability related to text encoding.

Severity: CRITICAL

Affected dependencies:
  - System.Text.Encodings.Web 4.6.0 (declared at NYCJobsWeb/packages.config:23)

Recommended fix:
  - Upgrade to 4.7.2 or later.

## CWE Findings (Code-Level Vulnerabilities)

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs:52-65

SendSearchRequest returns an HttpResponseMessage, and DataLoader/Program.cs stores and uses returned responses at lines 59, 82, and 102 without disposing them. The response resources can remain unreleased after their use, causing connection/resource exhaustion during repeated imports.

### CWE-835: Loop with Unreachable Exit Condition ('Infinite Loop')
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:193-199

UpdateMap loops while map.entities.getLength() is greater than zero. If map.entities.clear() throws, the empty catch suppresses the failure and the entity count remains unchanged, so the loop has no reachable exit and can hang the browser thread.

### CWE-798: Use of Hard-coded Credentials
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:114

The client-side map initialization embeds a Bing Maps API credential directly in the web page source. The value is exposed to every visitor and is used by the application for outbound service access; the credential value is intentionally omitted from this report.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml:49-52

UpdateJobDetails inserts job fields returned by the Azure Search API, including job_id, work_location, business_title, and civil_service_title, directly into the DOM using jQuery .html(). These stored/search-index values are not HTML-encoded before insertion, so HTML or script markup in a field can execute in a user's browser when job details are displayed.
