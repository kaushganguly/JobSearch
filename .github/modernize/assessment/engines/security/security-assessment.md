# Security Assessment Report

**Generated:** 2026-10-26T20:48:23.577995Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 8 |
| CVE Vulnerabilities | 5 |
| CWE Vulnerabilities | 3 |
| Total Rules Assessed | 59 |
| Rules Passed | 56 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 4 |
| potential | 2 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json 10.0.3 (NYCJobsWeb/packages.config:17)
  - Newtonsoft.Json 9.0.1 (DataLoader/DataLoader/packages.config:3)

Both versions are below the affected-range fix of 13.0.1. Recommended fix: upgrade to Newtonsoft.Json 13.0.1 or later.

### CVE-2022-26907: Azure SDK for .NET Information Disclosure Vulnerability.
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Affected dependency:
  - Microsoft.Rest.ClientRuntime 2.3.20 (NYCJobsWeb/packages.config:12)

The declared version is below 2.3.24. Recommended fix: upgrade Microsoft.Rest.ClientRuntime to 2.3.24 or later.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Affected dependency:
  - System.Text.Encodings.Web 4.6.0 (NYCJobsWeb/packages.config:23)

Version 4.6.0 is in the affected range >= 4.6.0 and < 4.7.2. Recommended fix: upgrade to 4.7.2 or later.

### CVE-2020-11023: Potential XSS vulnerability in jQuery
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Affected dependency:
  - jQuery 3.1.1 (NYCJobsWeb/packages.config:7)

The declared version is below 3.5.0. Recommended fix: upgrade jQuery to 3.5.0 or later.

### CVE-2019-11358: XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Affected dependency:
  - jQuery 3.1.1 (NYCJobsWeb/packages.config:7)

The declared version is below 3.4.0. Recommended fix: upgrade jQuery to 3.4.0 or later.

## CWE Findings (Code-Level Vulnerabilities)

### CWE-606: Unchecked Input for Loop Condition
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml:31-32

CleanText(text) loops while text.indexOf(/\n/g) > -1. String.indexOf coerces the regex to the literal string "/\n/g"; if external indexed job text contains that sequence, the condition remains true while text.replace(/\n/g, "<br />") does not alter it, causing an unbounded loop and browser denial of service.

### CWE-835: Loop with Unreachable Exit Condition ('Infinite Loop')
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml:31-32

CleanText(text) loops while text.indexOf(/\n/g) > -1. String.indexOf coerces the regex to the literal string "/\n/g"; if external indexed job text contains that sequence, the condition remains true while text.replace(/\n/g, "<br />") does not alter it, causing an unbounded loop and browser denial of service.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:397-416, NYCJobsWeb/Views/Home/JobDetails.cshtml:49-80

The search page concatenates Azure Search document fields (business_title, work_location, id, and job_description/highlights) into an HTML string and inserts it with jQuery .html() in UpdateJobDetails (lines 397-416). The detail page also inserts returned document fields through .html() (lines 49-80); CleanText only performs text substitutions and does not HTML-encode the values. Search-index content can therefore be interpreted as markup/script by other users.
