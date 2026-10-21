# Security Assessment Report

**Generated:** 2026-10-21T10:33:42.4730200Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 7 |
| CVE Vulnerabilities | 2 |
| CWE Vulnerabilities | 5 |
| Total Rules Assessed | 59 |
| Rules Passed | 54 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 2 |
| potential | 3 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Newtonsoft.Json versions before 13.0.1 may suffer denial of service from deeply nested JSON due to excessive resource use or stack overflow. Affected dependencies: Newtonsoft.Json 10.0.3 (NYCJobsWeb/packages.config:17) and Newtonsoft.Json 9.0.1 (DataLoader/DataLoader/packages.config:3). Recommended fix: upgrade to 13.0.1 or later.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): System.Text.Encodings.Web 4.6.0 is in the vulnerable range >= 4.6.0 and < 4.7.2. Recommended fix: upgrade to 4.7.2 or later.

## CWE Findings (Code-Level Vulnerabilities)

### CWE-606: Unchecked Input for Loop Condition
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml

In CleanText, the while condition at line 31 derives directly from the indexed job-description text. If the text contains the literal characters /\n/g, indexOf finds that string while replace(/\n/g, ...) does not remove it, so untrusted indexed content can keep the loop running indefinitely.

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs

SendSearchRequest returns an HttpResponseMessage at line 65, and its callers in Program.DeleteIndex, CreateTargetIndex, and ImportFromJSON read/check the response without disposing it. Repeated requests in the import process leave response and underlying network resources unreleased until finalization.

### CWE-835: Loop with Unreachable Exit Condition ('Infinite Loop')
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml

In CleanText, the while loop at lines 31-32 tests text.indexOf(/\n/g), which searches for the regex literal's string representation, but replaces actual newline characters. For input containing the literal /\n/g but no newline, the condition remains true and the replacement makes no progress, causing an infinite loop.

### CWE-798: Use of Hard-coded Credentials
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml

The GetMap function assigns a literal Bing Maps API credential to the map configuration at line 114. The credential is embedded in browser-delivered source, making it a hard-coded credential; its value is intentionally omitted from this report.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml

In UpdateJobDetails, values returned from the job lookup response are written directly to the DOM with jQuery .html(), including job_description at line 68 and business_title at line 51. The job content originates from the Azure Search index, and CleanText only replaces selected characters rather than encoding HTML, so markup in the indexed data is interpreted in other users' browsers.
