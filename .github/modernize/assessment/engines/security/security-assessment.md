# Security Assessment Report

**Generated:** 2026-10-26T05:22:52.923879Z

## Summary

| Metric | Count |
|--------|-------|
| Total Findings | 13 |
| CVE Vulnerabilities | 5 |
| CWE Vulnerabilities | 8 |
| Total Rules Assessed | 59 |
| Rules Passed | 51 |

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 5 |
| potential | 6 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Newtonsoft.Json versions before 13.0.1 can consume excessive CPU and memory or cause a stack overflow when processing deeply nested JSON. Severity: HIGH. Affected dependencies: Newtonsoft.Json 10.0.3 (`NYCJobsWeb/packages.config:17`) and 9.0.1 (`DataLoader/DataLoader/packages.config:3`). Recommended fix: upgrade both to 13.0.1 or later, or constrain JSON nesting with `MaxDepth`.

### CVE-2022-26907: Azure SDK for .NET Information Disclosure Vulnerability
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Microsoft.Rest.ClientRuntime versions before 2.3.24 are affected by an information disclosure vulnerability. Severity: MEDIUM. Affected dependency: Microsoft.Rest.ClientRuntime 2.3.20 (`NYCJobsWeb/packages.config:12`). Recommended fix: upgrade to 2.3.24 or later.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): System.Text.Encodings.Web 4.6.0 falls in the vulnerable range >=4.6.0 and <4.7.2. Severity: CRITICAL. Affected dependency: System.Text.Encodings.Web 4.6.0 (`NYCJobsWeb/packages.config:23`). Recommended fix: upgrade to 4.7.2 or later.

### CVE-2020-11023: Potential XSS vulnerability in jQuery
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): jQuery versions from 1.0.3 up to (but not including) 3.5.0 have a potential XSS vulnerability. Severity: MEDIUM. Affected dependency: jQuery 3.1.1 (`NYCJobsWeb/packages.config:7`). Recommended fix: upgrade to 3.5.0 or later.

### CVE-2019-11358: XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): jQuery versions from 1.1.4 up to (but not including) 3.4.0 are affected by a prototype pollution vulnerability that can contribute to XSS. Severity: MEDIUM. Affected dependency: jQuery 3.1.1 (`NYCJobsWeb/packages.config:7`). Recommended fix: upgrade to 3.4.0 or later; 3.5.0 or later also addresses CVE-2020-11023.

## CWE Findings (Code-Level Vulnerabilities)

### CWE-456: Missing Initialization of a Variable
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 2
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/Web.config:8-12

The static constructor attempts to initialize _indexClient and _indexZipClient from Searchendpoint, but catches initialization failures and continues with both fields null. Web.config does not declare Searchendpoint (it declares SearchServiceName instead), so the required search clients are not initialized by the checked-in configuration.

### CWE-457: Use of Uninitialized Variable
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 2
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/JobsSearch.cs:109-115

After the static constructor catches a failed client initialization, Search() dereferences the still-null _indexClient. The exception is caught and converted to a null return, which HomeController later dereferences while building the response.

### CWE-665: Improper Initialization
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/JobsSearch.cs:109-115

SearchClient resources are initialized only inside a try block; initialization failures are swallowed, leaving the clients null for later use. The checked-in Web.config omits the Searchendpoint setting read by the constructor.

### CWE-681: Incorrect Conversion between Numeric Types
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Controllers/HomeController.cs:60

HomeController.Search narrows the Azure Search TotalCount value to Int32 using Convert.ToInt32. A count outside the Int32 range raises OverflowException and prevents the response from being created.

### CWE-682: Incorrect Calculation
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 5
- **Files:** NYCJobsWeb/Controllers/HomeController.cs:28-30, NYCJobsWeb/JobsSearch.cs:48-55

The controller defaults currentPage to 0, but JobsSearch.Search assigns Skip = currentPage - 1. The default request therefore produces a negative skip count rather than the first page offset.

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/Program.cs:59-60, DataLoader/DataLoader/Program.cs:82-83, DataLoader/DataLoader/Program.cs:102-103

The importer creates HttpResponseMessage instances for each delete, index creation, and document upload request, then reads the status but never disposes the responses. The repeated upload loop can retain response/content resources until collection rather than releasing them at the end of each request.

### CWE-732: Incorrect Permission Assignment for Critical Resource
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Schema_and_Data/nycjobs.schema:1

The Azure Search index schema configures CORS allowedOrigins as ["*"], allowing browser requests from arbitrary origins to access the service when otherwise authorized. This is a permissive origin policy on a security-sensitive search resource.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:397-416, NYCJobsWeb/Views/Home/JobDetails.cshtml:49-80

UpdateJobDetails inserts Azure AI Search document fields such as business_title, work_location, job_description, and id into HTML using string concatenation and jQuery .html() without output encoding (Index.cshtml:397-416; JobDetails.cshtml:49-80). A stored value containing markup or script can therefore execute in another visitor’s browser.
