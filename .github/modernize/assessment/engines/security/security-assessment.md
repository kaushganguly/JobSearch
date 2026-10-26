# Security Assessment Report

**Generated:** 2026-10-26T19:46:16.935105Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 12 |
| CVE Vulnerabilities | 7 |
| CWE Vulnerabilities | 5 |
| Total Rules Assessed | 59 |
| Rules Passed | 54 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 7 |
| potential | 3 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json:9.0.1 (declared at DataLoader/DataLoader/packages.config:3)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2022-26907: Azure SDK for .NET Information Disclosure Vulnerability.
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Affected dependencies:
  - Microsoft.Rest.ClientRuntime:2.3.20 (declared at NYCJobsWeb/packages.config:12)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Affected dependencies:
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2020-11023: Potential XSS vulnerability in jQuery
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2019-11358: XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2019-8331: Bootstrap Vulnerable to Cross-Site Scripting
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:6

[CVE-2019-8331](https://github.com/advisories/GHSA-9v3m-8fp8-mj99): Bootstrap Vulnerable to Cross-Site Scripting

Severity: MEDIUM

Affected dependencies:
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

### CVE-2016-10735: Bootstrap Cross-site Scripting vulnerability
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:6

[CVE-2016-10735](https://github.com/advisories/GHSA-4p24-vmcr-4gqj): Bootstrap Cross-site Scripting vulnerability

Severity: MEDIUM

Affected dependencies:
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)
  - bootstrap:3.4.1 (declared at NYCJobsWeb/packages.config:6)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

## CWE Findings (Code-Level Vulnerabilities)

### CWE-665: Improper Initialization
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs

JobsSearch static constructor (lines 24-39) catches client/configuration initialization failures and leaves _indexClient and _indexZipClient null; the public query methods can subsequently dereference those uninitialized clients.

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs, DataLoader/DataLoader/Program.cs

SendSearchRequest (line 65) returns each HttpResponseMessage without disposing it. DataLoader callers in Program.cs retain and inspect the response but do not dispose it, so the response and its underlying resources are not deterministically released.

### CWE-775: Missing Release of File Descriptor or Handle after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs, DataLoader/DataLoader/Program.cs

SendSearchRequest (line 65) returns HttpResponseMessage instances without disposing them. DataLoader callers in Program.cs do not dispose these responses, leaving underlying response/connection resources open until nondeterministic cleanup.

### CWE-798: Use of Hard-coded Credentials
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:114

The client-side map initialization embeds a non-placeholder API credential directly in the source view. This is an exposed hard-coded credential; the value is redacted from this assessment.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml

UpdateJobDetails inserts Azure Search document values into HTML using jQuery .html() without HTML encoding. Fields such as business_title and work_location are rendered directly at lines 49-52 and 79-80; CleanText at line 68 only performs text replacements and does not encode markup before rendering job_description. If indexed job content contains attacker-controlled markup, it can execute in viewers’ browsers.
