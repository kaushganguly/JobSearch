# Security Assessment Report

**Generated:** 2026-10-21T13:08:26.3630270Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 5 |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 3 |
| Total Rules Assessed | 59 |
| Rules Passed | 56 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 2 |
| potential | 1 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Newtonsoft.Json versions before 13.0.1 can process deeply nested JSON in a way that causes excessive resource consumption or stack overflow, resulting in denial of service.

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json 10.0.3 (NYCJobsWeb/packages.config:17)
  - Newtonsoft.Json 9.0.1 (DataLoader/DataLoader/packages.config:3)

Recommended fix: Upgrade both package references to 13.0.1 or later, or configure an appropriate maximum JSON depth.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): System.Text.Encodings.Web versions 4.6.0 through 4.7.1 are affected by a remote code execution vulnerability related to text encoding.

Severity: CRITICAL

Affected dependency:
  - System.Text.Encodings.Web 4.6.0 (NYCJobsWeb/packages.config:23)

Recommended fix: Upgrade to 4.7.2 or later (or the patched version appropriate for the target framework).

## CWE Findings (Code-Level Vulnerabilities)

### CWE-835: Loop with Unreachable Exit Condition ('Infinite Loop')
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml

In UpdateMap, the while loop at line 193 repeats while map.entities.getLength() is greater than zero. The only attempted exit is map.entities.clear() inside a try; if clear throws, the empty catch at lines 195-198 suppresses the exception and the loop condition remains true, so the browser thread can hang.

### CWE-798: Use of Hard-coded Credentials
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml

The page embeds a non-empty mapping-service API credential directly in the client-side map initialization at line 114. It is shipped to every browser as application source. The credential value is intentionally omitted from this report.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml

UpdateBusinessTitleFacets and UpdatePostingTypeFacets concatenate Azure Search facet values directly into HTML and pass the result to jQuery .html() (lines 313-335). UpdateJobDetails also inserts indexed business_title and work_location values into HTML at lines 397-398 without output encoding, allowing stored script markup from indexed content to execute in visitors’ browsers.
