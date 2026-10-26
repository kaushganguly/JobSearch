# JobSearch Assessment Report

**Report ID:** 20261026052252

## Summary

| Metric | Count |
|---|---:|
| Total Issues | 13 |
| Mandatory Issues | 2 |
| Optional Issues | 5 |
| Potential Issues | 6 |
| CVE Findings | 5 |
| CWE Findings | 8 |

## Application Information

| Item | Value |
|---|---|
| Application | JobSearch |
| Language | C# |
| Target frameworks | .NET Framework 4.7.2 (web), .NET Framework 4.5 (import utility) |
| Project format | Legacy non-SDK MSBuild |
| Report generation | AppCAT did not produce core project data; see assessment limitations below |

## Assessment Limitations

The core AppCAT analysis could not load `NYCJobsWeb.csproj` because the environment lacks `Microsoft.WebApplication.targets` (Visual Studio Web Application targets). The .NET 10 upgrade assessment created scenario artifacts but reported the same missing targets and contains zero analyzed projects, dependencies, or issues. Cloud-readiness and upgrade findings are therefore unavailable; the security findings and supplementary fact documents are reported separately.

## Cloud Readiness Issues

No core AppCAT cloud-readiness issues were generated.

## Upgrade Issues

The .NET 10 upgrade assessment output contains no project details because project loading failed; do not interpret the zero issue count as a compatibility result.

## Security Issues

<a id="net-core-remote-code-execution-vulnerability"></a>
### .NET Core Remote Code Execution Vulnerability

- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): System.Text.Encodings.Web 4.6.0 falls in the vulnerable range >=4.6.0 and <4.7.2. Severity: CRITICAL. Affected dependency: System.Text.Encodings.Web 4.6.0 (`NYCJobsWeb/packages.config:23`). Recommended fix: upgrade to 4.7.2 or later.

<a id="improper-handling-of-exceptional-conditions-in-newtonsoft-json"></a>
### Improper Handling of Exceptional Conditions in Newtonsoft.Json

- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:17, DataLoader/DataLoader/packages.config:3

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Newtonsoft.Json versions before 13.0.1 can consume excessive CPU and memory or cause a stack overflow when processing deeply nested JSON. Severity: HIGH. Affected dependencies: Newtonsoft.Json 10.0.3 (`NYCJobsWeb/packages.config:17`) and 9.0.1 (`DataLoader/DataLoader/packages.config:3`). Recommended fix: upgrade both to 13.0.1 or later, or constrain JSON nesting with `MaxDepth`.

<a id="azure-sdk-for-net-information-disclosure-vulnerability"></a>
### Azure SDK for .NET Information Disclosure Vulnerability

- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Microsoft.Rest.ClientRuntime versions before 2.3.24 are affected by an information disclosure vulnerability. Severity: MEDIUM. Affected dependency: Microsoft.Rest.ClientRuntime 2.3.20 (`NYCJobsWeb/packages.config:12`). Recommended fix: upgrade to 2.3.24 or later.

<a id="improper-neutralization-of-input-during-web-page-generation-cross-site-scripting"></a>
### Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')

- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:397-416, NYCJobsWeb/Views/Home/JobDetails.cshtml:49-80

The product does not neutralize or incorrectly neutralizes user-controllable input before it is placed in output that is used as a web page that is served to other users.

**Evidence:** UpdateJobDetails inserts Azure AI Search document fields such as business_title, work_location, job_description, and id into HTML using string concatenation and jQuery .html() without output encoding (Index.cshtml:397-416; JobDetails.cshtml:49-80). A stored value containing markup or script can therefore execute in another visitor’s browser.

<a id="incorrect-permission-assignment-for-critical-resource"></a>
### Incorrect Permission Assignment for Critical Resource

- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Schema_and_Data/nycjobs.schema:1

The product specifies permissions for a security-critical resource in a way that allows that resource to be read or modified by unintended actors.

**Evidence:** The Azure Search index schema configures CORS allowedOrigins as ["*"], allowing browser requests from arbitrary origins to access the service when otherwise authorized. This is a permissive origin policy on a security-sensitive search resource.

<a id="potential-xss-vulnerability-in-jquery"></a>
### Potential XSS vulnerability in jQuery

- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): jQuery versions from 1.0.3 up to (but not including) 3.5.0 have a potential XSS vulnerability. Severity: MEDIUM. Affected dependency: jQuery 3.1.1 (`NYCJobsWeb/packages.config:7`). Recommended fix: upgrade to 3.5.0 or later.

<a id="xss-in-jquery-as-used-in-drupal-backdrop-cms-and-other-products"></a>
### XSS in jQuery as used in Drupal, Backdrop CMS, and other products

- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): jQuery versions from 1.1.4 up to (but not including) 3.4.0 are affected by a prototype pollution vulnerability that can contribute to XSS. Severity: MEDIUM. Affected dependency: jQuery 3.1.1 (`NYCJobsWeb/packages.config:7`). Recommended fix: upgrade to 3.4.0 or later; 3.5.0 or later also addresses CVE-2020-11023.

<a id="improper-initialization"></a>
### Improper Initialization

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/JobsSearch.cs:109-115

The product does not initialize or incorrectly initializes a resource, which might leave the resource in an unexpected state when it is accessed or used.

**Evidence:** SearchClient resources are initialized only inside a try block; initialization failures are swallowed, leaving the clients null for later use. The checked-in Web.config omits the Searchendpoint setting read by the constructor.

<a id="incorrect-calculation"></a>
### Incorrect Calculation

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 5
- **Files:** NYCJobsWeb/Controllers/HomeController.cs:28-30, NYCJobsWeb/JobsSearch.cs:48-55

The product performs a calculation that generates incorrect or unintended results that are later used in security-critical decisions or resource management.

**Evidence:** The controller defaults currentPage to 0, but JobsSearch.Search assigns Skip = currentPage - 1. The default request therefore produces a negative skip count rather than the first page offset.

<a id="incorrect-conversion-between-numeric-types"></a>
### Incorrect Conversion between Numeric Types

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/Controllers/HomeController.cs:60

When converting from one data type to another, such as long to integer, data can be omitted or translated in a way that produces unexpected values. If the resulting values are used in a sensitive context, then dangerous behaviors may occur.

**Evidence:** HomeController.Search narrows the Azure Search TotalCount value to Int32 using Convert.ToInt32. A count outside the Int32 range raises OverflowException and prevents the response from being created.

<a id="missing-initialization-of-a-variable"></a>
### Missing Initialization of a Variable

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 2
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/Web.config:8-12

The product does not initialize critical variables, which causes the execution environment to use unexpected values.

**Evidence:** The static constructor attempts to initialize _indexClient and _indexZipClient from Searchendpoint, but catches initialization failures and continues with both fields null. Web.config does not declare Searchendpoint (it declares SearchServiceName instead), so the required search clients are not initialized by the checked-in configuration.

<a id="missing-release-of-resource-after-effective-lifetime"></a>
### Missing Release of Resource after Effective Lifetime

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/Program.cs:59-60, DataLoader/DataLoader/Program.cs:82-83, DataLoader/DataLoader/Program.cs:102-103

The product does not release a resource after its effective lifetime has ended, i.e., after the resource is no longer needed.

**Evidence:** The importer creates HttpResponseMessage instances for each delete, index creation, and document upload request, then reads the status but never disposes the responses. The repeated upload loop can retain response/content resources until collection rather than releasing them at the end of each request.

<a id="use-of-uninitialized-variable"></a>
### Use of Uninitialized Variable

- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 2
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/JobsSearch.cs:109-115

The code uses a variable that has not been initialized, leading to unpredictable or unintended results.

**Evidence:** After the static constructor catches a failed client initialization, Search() dereferences the still-null _indexClient. The exception is caught and converted to a null return, which HomeController later dereferences while building the response.

## Codebase Insights

| Document | Description |
|---|---|
| [architecture-diagram.md](facts/architecture-diagram.md) | Application layers and component relationships. |
| [dependency-map.md](facts/dependency-map.md) | Declared dependencies, versions, and compatibility risks. |
| [api-service-contracts.md](facts/api-service-contracts.md) | API surface, services, and communication patterns. |
| [data-architecture.md](facts/data-architecture.md) | Search indexes and data ownership. |
| [configuration-inventory.md](facts/configuration-inventory.md) | Configuration files, settings, and secrets handling. |
| [business-workflows.md](facts/business-workflows.md) | Job search and index import processes. |

[Share feedback](https://aka.ms/ghcp-appmod/feedback)
