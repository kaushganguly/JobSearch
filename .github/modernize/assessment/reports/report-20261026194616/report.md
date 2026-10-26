# JobSearch Application Assessment

## Summary

| Metric | Value |
|---|---:|
| Total Issues | 12 |
| CVE Findings | 7 |
| CWE Findings | 5 |
| Mandatory | 2 |
| Potential | 3 |
| Optional | 7 |

The core AppCAT assessment could not load the legacy ASP.NET Web Application project because the Linux .NET SDK does not include `Microsoft.WebApplication.targets`. The .NET 10 upgrade assessment was attempted; its generated data reports zero projects analyzed for the same import failure. See the upgrade assessment files under `scenarios/dotnet-version-upgrade/`.

## Application Information

| Property | Value |
|---|---|
| Application | JobSearch / NYCJobsWeb |
| Language | C# |
| Frameworks | .NET Framework 4.7.2 web app; .NET Framework 4.5 data loader |
| Build format | Legacy MSBuild project files |
| Test projects | None detected |

## Security Findings

<a id="improper_handling_of_exceptional_conditions_in_newtonsoft_json"></a>
### Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **ID:** CVE-2024-21907
- **Category:** CVE
- **Severity:** mandatory
- **Story points:** 1
- **Files:** DataLoader/DataLoader/packages.config:3

**Description**

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json:9.0.1 (declared at DataLoader/DataLoader/packages.config:3)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

**Evidence**

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Affected dependencies:
  - Newtonsoft.Json:9.0.1 (declared at DataLoader/DataLoader/packages.config:3)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

<a id="azure_sdk_for_net_information_disclosure_vulnerability"></a>
### Azure SDK for .NET Information Disclosure Vulnerability.
- **ID:** CVE-2022-26907
- **Category:** CVE
- **Severity:** optional
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:12

**Description**

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Affected dependencies:
  - Microsoft.Rest.ClientRuntime:2.3.20 (declared at NYCJobsWeb/packages.config:12)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

**Evidence**

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Affected dependencies:
  - Microsoft.Rest.ClientRuntime:2.3.20 (declared at NYCJobsWeb/packages.config:12)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

<a id="net_core_remote_code_execution_vulnerability"></a>
### .NET Core Remote Code Execution Vulnerability
- **ID:** CVE-2021-26701
- **Category:** CVE
- **Severity:** mandatory
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:23

**Description**

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Affected dependencies:
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

**Evidence**

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Affected dependencies:
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)
  - System.Text.Encodings.Web:4.6.0 (declared at NYCJobsWeb/packages.config:23)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

<a id="potential_xss_vulnerability_in_jquery"></a>
### Potential XSS vulnerability in jQuery
- **ID:** CVE-2020-11023
- **Category:** CVE
- **Severity:** optional
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:7

**Description**

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

**Evidence**

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

<a id="xss_in_jquery_as_used_in_drupal_backdrop_cms_and_other_products"></a>
### XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **ID:** CVE-2019-11358
- **Category:** CVE
- **Severity:** optional
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:7

**Description**

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

**Evidence**

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Affected dependencies:
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)
  - jQuery:3.1.1 (declared at NYCJobsWeb/packages.config:7)

Recommended fix:
  - Upgrade to a patched version identified in the advisory.

<a id="bootstrap_vulnerable_to_cross-site_scripting"></a>
### Bootstrap Vulnerable to Cross-Site Scripting
- **ID:** CVE-2019-8331
- **Category:** CVE
- **Severity:** optional
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:6

**Description**

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

**Evidence**

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

<a id="bootstrap_cross-site_scripting_vulnerability"></a>
### Bootstrap Cross-site Scripting vulnerability
- **ID:** CVE-2016-10735
- **Category:** CVE
- **Severity:** optional
- **Story points:** 1
- **Files:** NYCJobsWeb/packages.config:6

**Description**

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

**Evidence**

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

<a id="improper_initialization"></a>
### Improper Initialization
- **ID:** CWE-665
- **Category:** Code Quality
- **Severity:** potential
- **Story points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs

**Description**

The product does not initialize or incorrectly initializes a resource, which might leave the resource in an unexpected state when it is accessed or used.

**Evidence**

JobsSearch static constructor (lines 24-39) catches client/configuration initialization failures and leaves _indexClient and _indexZipClient null; the public query methods can subsequently dereference those uninitialized clients.

<a id="missing_release_of_resource_after_effective_lifetime"></a>
### Missing Release of Resource after Effective Lifetime
- **ID:** CWE-772
- **Category:** Code Quality
- **Severity:** potential
- **Story points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs, DataLoader/DataLoader/Program.cs

**Description**

The product does not release a resource after its effective lifetime has ended, i.e., after the resource is no longer needed.

**Evidence**

SendSearchRequest (line 65) returns each HttpResponseMessage without disposing it. DataLoader callers in Program.cs retain and inspect the response but do not dispose it, so the response and its underlying resources are not deterministically released.

<a id="missing_release_of_file_descriptor_or_handle_after_effective_lifetime"></a>
### Missing Release of File Descriptor or Handle after Effective Lifetime
- **ID:** CWE-775
- **Category:** Code Quality
- **Severity:** potential
- **Story points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs, DataLoader/DataLoader/Program.cs

**Description**

The product does not release a file descriptor or handle after its effective lifetime has ended, i.e., after the file descriptor/handle is no longer needed.

**Evidence**

SendSearchRequest (line 65) returns HttpResponseMessage instances without disposing them. DataLoader callers in Program.cs do not dispose these responses, leaving underlying response/connection resources open until nondeterministic cleanup.

<a id="use_of_hard-coded_credentials"></a>
### Use of Hard-coded Credentials
- **ID:** CWE-798
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:114

**Description**

The product contains hard-coded credentials, such as a password or cryptographic key.

**Evidence**

The client-side map initialization embeds a non-placeholder API credential directly in the source view. This is an exposed hard-coded credential; the value is redacted from this assessment.

<a id="improper_neutralization_of_input_during_web_page_generation_cross-site_scripting"></a>
### Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **ID:** CWE-79
- **Category:** Injection Attacks
- **Severity:** optional
- **Story points:** 8
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml

**Description**

The product does not neutralize or incorrectly neutralizes user-controllable input before it is placed in output that is used as a web page that is served to other users.

**Evidence**

UpdateJobDetails inserts Azure Search document values into HTML using jQuery .html() without HTML encoding. Fields such as business_title and work_location are rendered directly at lines 49-52 and 79-80; CleanText at line 68 only performs text replacements and does not encode markup before rendering job_description. If indexed job content contains attacker-controlled markup, it can execute in viewers’ browsers.

## .NET 10 Upgrade Assessment

The upgrade assessment request targeted `net10.0`. The assessment tool initialized the scenario and wrote its report files, but it could not load `NYCJobsWeb.csproj` because `Microsoft.WebApplication.targets` is missing in this Linux environment. `assessment.json` therefore contains 0 projects and 0 issues; this is not evidence that the project is compatible.

## Codebase Insights

- [architecture-diagram.md](facts/architecture-diagram.md) — Application layers and component relationships.
- [dependency-map.md](facts/dependency-map.md) — External dependencies and compatibility risks.
- [api-service-contracts.md](facts/api-service-contracts.md) — MVC routes and service communication contracts.
- [data-architecture.md](facts/data-architecture.md) — Azure Search data model and ownership.
- [configuration-inventory.md](facts/configuration-inventory.md) — Configuration sources, settings, and secret provisioning.
- [business-workflows.md](facts/business-workflows.md) — Job search and index maintenance workflows.
