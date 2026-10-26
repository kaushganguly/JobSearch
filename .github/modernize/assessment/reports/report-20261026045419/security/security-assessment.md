# Security Assessment Report

**Generated:** 2026-10-26T04:59:40.632704+00:00
**Scope:** full-repository static assessment (not a diff); minimum CVE severity: medium.

## Summary

### Interpretation and Reviewer Caveats

- The five CVE findings are confirmed matches between declared dependency versions and active GitHub advisory ranges; application exploitability is not established. Newtonsoft.Json CVE-2024-21907 concerns denial of service.
- The five CWE findings are valid prescribed-rule source patterns, not five confirmed exploitable vulnerabilities. Indexed-content XSS requires attacker influence over upstream/indexed data; no anonymous write route was found. The browser map key may be public by design, and its restrictions, validity and abuse potential are unknown.
- Code-quality findings include an always-true condition, partially initialized clients after configuration failure, and undisposed HTTP resources. Their demonstrated effects concern correctness, initialization/availability and resource management; no data compromise or privilege escalation was established.
- The specialist reviewer produced no qualifying changed-code findings because source was unchanged; this is not full-repository clearance and was not reused as a clean verdict. This inventory independently assessed full application source against all 59 specified CWE rules.
- Coverage is static and limited: mandatory dependency enumeration failed on missing Visual Studio WebApplication targets, so manifests were used; undeclared transitive dependencies, runtime-selected CDN/vendor versions and deployed platform components may be incomplete. No runtime/exploit/key-validity, authorization, logging, ACL, fuzz or concurrency testing was performed. NOT_FOUND/RulesPassed is not a security guarantee.

| Metric | Count |
|--------|-------|
| TotalFindings | 10 |
| CveCount | 5 |
| CweCount | 5 |
| TotalRulesAssessed | 59 |
| RulesPassed | 54 |

RulesPassed is the number of checklist rules without a confirmed match, not a security guarantee.

### By Severity

| Severity | Count |
|----------|-------|
| mandatory | 2 |
| optional | 6 |
| potential | 2 |

## CVE Findings (Dependency Vulnerabilities)

### CVE-2024-21907: Improper Handling of Exceptional Conditions in Newtonsoft.Json
- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** DataLoader/DataLoader/packages.config:3, NYCJobsWeb/packages.config:17

[CVE-2024-21907](https://github.com/advisories/GHSA-5crp-9r3c-p9vr): Improper Handling of Exceptional Conditions in Newtonsoft.Json

Severity: HIGH

Affected dependencies:
- Newtonsoft.Json:10.0.3 (NYCJobsWeb/packages.config:17); vulnerable range < 13.0.1; first patched version 13.0.1.
- Newtonsoft.Json:9.0.1 (DataLoader/DataLoader/packages.config:3); vulnerable range < 13.0.1; first patched version 13.0.1.

Recommended fix: upgrade each affected package to a supported release at or above the corresponding first patched version; validate compatibility. Manifest match confirms a vulnerable dependency, not exploitability of application call sites.

### CVE-2022-26907: Azure SDK for .NET Information Disclosure Vulnerability.
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:12

[CVE-2022-26907](https://github.com/advisories/GHSA-whph-446h-6m9v): Azure SDK for .NET Information Disclosure Vulnerability.

Severity: MEDIUM

Affected dependencies:
- Microsoft.Rest.ClientRuntime:2.3.20 (NYCJobsWeb/packages.config:12); vulnerable range < 2.3.24; first patched version 2.3.24.

Recommended fix: upgrade each affected package to a supported release at or above the corresponding first patched version; validate compatibility. Manifest match confirms a vulnerable dependency, not exploitability of application call sites.

### CVE-2021-26701: .NET Core Remote Code Execution Vulnerability
- **Category:** CVE
- **Severity:** mandatory
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:23

[CVE-2021-26701](https://github.com/advisories/GHSA-ghhp-997w-qr28): .NET Core Remote Code Execution Vulnerability

Severity: CRITICAL

Affected dependencies:
- System.Text.Encodings.Web:4.6.0 (NYCJobsWeb/packages.config:23); vulnerable range >= 4.6.0, < 4.7.2; first patched version 4.7.2.

Recommended fix: upgrade each affected package to a supported release at or above the corresponding first patched version; validate compatibility. Manifest match confirms a vulnerable dependency, not exploitability of application call sites.

### CVE-2020-11023: Potential XSS vulnerability in jQuery
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2020-11023](https://github.com/advisories/GHSA-jpcq-cgw6-v4j6): Potential XSS vulnerability in jQuery

Severity: MEDIUM

Affected dependencies:
- jQuery:3.1.1 (NYCJobsWeb/packages.config:7); vulnerable range >= 1.0.3, < 3.5.0; first patched version 3.5.0.

Recommended fix: upgrade each affected package to a supported release at or above the corresponding first patched version; validate compatibility. Manifest match confirms a vulnerable dependency, not exploitability of application call sites.

### CVE-2019-11358: XSS in jQuery as used in Drupal, Backdrop CMS, and other products
- **Category:** CVE
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/packages.config:7

[CVE-2019-11358](https://github.com/advisories/GHSA-6c3j-c64m-qhgq): XSS in jQuery as used in Drupal, Backdrop CMS, and other products

Severity: MEDIUM

Affected dependencies:
- jQuery:3.1.1 (NYCJobsWeb/packages.config:7); vulnerable range >= 1.1.4, < 3.4.0; first patched version 3.4.0.

Recommended fix: upgrade each affected package to a supported release at or above the corresponding first patched version; validate compatibility. Manifest match confirms a vulnerable dependency, not exploitability of application call sites.

## CWE Findings (Code-Level Patterns)

### CWE-571: Expression is Always True
- **Category:** Code Quality
- **Severity:** optional
- **Story Points:** 1
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml:9-15

LookUp splits the query string with query.split("&") at line 11 and checks vars.length > 0 at line 12. JavaScript split always returns at least one item here, including for an empty string, so the condition is always true and an empty/missing query still sends a lookup request. Confirmed control-flow defect; no independent privilege escalation or server compromise demonstrated.

### CWE-665: Improper Initialization
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** NYCJobsWeb/JobsSearch.cs:24-39, NYCJobsWeb/JobsSearch.cs:109-115, NYCJobsWeb/Controllers/HomeController.cs:52-60

JobsSearch static constructor catches credential/endpoint/client initialization exceptions and only stores e.Message at lines 36-38. Type initialization therefore completes with one or both static search clients unset. Search later dereferences the client, catches the failure and returns null; HomeController.Search dereferences that result. Invalid/missing configuration can leave the service permanently partially initialized for the process lifetime and produce request failures. Configuration-dependent availability defect, not a demonstrated remote configuration-write exploit.

### CWE-772: Missing Release of Resource after Effective Lifetime
- **Category:** Code Quality
- **Severity:** potential
- **Story Points:** 3
- **Files:** DataLoader/DataLoader/AzureSearchHelper.cs:58-65, DataLoader/DataLoader/Program.cs:58-60, DataLoader/DataLoader/Program.cs:97-104

AzureSearchHelper.SendSearchRequest creates HttpRequestMessage and optional StringContent without disposal and returns an HttpResponseMessage. Loader callers only inspect status and never dispose the response; ImportFromJSON repeats this per data file. Request/response resources remain undisposed beyond each request lifetime. Finite local import workload; socket/handle exhaustion was not dynamically established, and this is not asserted as a separately verified file-descriptor leak.

### CWE-798: Use of Hard-coded Credentials
- **Category:** Credentials & Secrets
- **Severity:** optional
- **Story Points:** 5
- **Files:** NYCJobsWeb/Views/Home/Index.cshtml:112-115

Home view GetMap passes a non-placeholder, hard-coded Bing Maps credential to the browser map constructor at line 114. The credential is persisted in source and delivered to every page visitor. Value intentionally redacted and never tested. Browser map keys may be intentionally public; validity, domain/referrer restrictions, allowed APIs, billing exposure and administrative capability are unknown. This is a confirmed embedded API credential pattern, not evidence that a privileged server secret is active. Search service configuration values are placeholders or empty and are not reported as real credentials.

### CWE-79: Improper Neutralization of Input During Web Page Generation ('Cross-site Scripting')
- **Category:** Injection Attacks
- **Severity:** optional
- **Story Points:** 8
- **Files:** NYCJobsWeb/Views/Home/JobDetails.cshtml:24-40, NYCJobsWeb/Views/Home/JobDetails.cshtml:49-73, NYCJobsWeb/Controllers/HomeController.cs:86-96, DataLoader/DataLoader/Program.cs:97-103

Stored/DOM XSS sink: HomeController.LookUp returns search-index fields as JSON; JobDetails.UpdateJobDetails inserts work_location, business_title and other fields directly through jQuery .html() at lines 49-73. CleanText only replaces formatting sequences (24-40), not HTML or event attributes. An attacker able to influence imported/indexed job content can introduce active markup rendered for visitors. No anonymous index-write route was found; exploitation requires control of upstream data or index write access, not merely an id query parameter. Static source confirmation; no live payload executed.

## Scope and Limitations

- Full-repository static application assessment, not a changed-lines/diff assessment. Scope: NYCJobsWeb ASP.NET MVC and DataLoader C# source, Razor/JavaScript request/render paths, NuGet manifests and application configuration. Agent/skill definitions and generated obj/bin code are not target application code.
- Mandatory dotnet list package --include-transitive --format json failed because Microsoft.WebApplication.targets is absent in the Linux .NET 10 SDK. No restore or dependency installation attempted. Fallback inventory contains 25 unique name/version pairs across both legacy packages.config files, including recorded direct/transitive installed packages. Parent chains and undeclared/unrestored transitive dependencies could not be reconstructed.
- GitHub Security Advisories REST API queried successfully on 2026-10-26 with ecosystem nuget, 25 coordinates in one batch (limit 30), per_page=100 and Link pagination handling. Ten advisories returned: three withdrawn excluded; two active Bootstrap advisories excluded because the exact NuGet bootstrap 3.4.1 ranges are not affected. Five active exact-version findings remain. Minimum merged CVE severity is medium.
- NuGet manifest coverage does not establish complete coverage of unmanifested vendored JavaScript (including older jQuery/plugins), remote CDN scripts, deployment OS/IIS/.NET Framework components, native libraries or externally resolved dependencies. Vendor/minified implementations were inventoried but not exhaustively source-audited. No claim that the five CVEs are all vulnerabilities in the deployment.
- No successful .NET Framework build, application execution, live Azure/Bing access, payload execution, fuzzing, concurrency/load tests, binary/native dependency analysis, git-history secret scan or external authorization/logging/ACL inspection performed. No credential validity testing. Actual credential values are omitted; only source locations are recorded.
- CWE FOUND is a source-pattern checklist result, not automatically an exploitable vulnerability or CVSS rating. XSS requires attacker-controlled upstream/indexed job content; no anonymous write path was found. Embedded Bing browser key may be intentionally public and has unknown restrictions/validity. Code-quality findings have bounded/configuration-dependent availability or correctness impact. CWE severities are skill priority labels, not CVSS scores.
- NOT_FOUND and RulesPassed mean no confirmed match for the specified rules in examined source, not proof that code or deployment is secure. OData filter concatenation is recorded as an additional concern in injection raw evidence; it is not mislabeled SQL injection and not counted among the prescribed CWE checklist findings.
- NYCJobsWeb/Web.config is pre-existing malformed XML at line 8, uses a placeholder search key at line 10 and lacks the Searchendpoint key consumed by JobsSearch.cs:28. Configuration-based runtime initialization could not be validated. No source/configuration changes were made.
- write_assessment_result is unavailable. Per explicit task authorization all raw/merged results were serialized directly with Python shell commands. Raw execution_time_seconds=0 means timing was not instrumented, not a claim of instantaneous scanning.

## Raw Assessments

- `cve-assessment-result.json`
- `result-cwe-code-quality.json`
- `result-cwe-concurrency-synchronization.json`
- `result-cwe-credentials-secrets.json`
- `result-cwe-file-path-security.json`
- `result-cwe-injection-attacks.json`
- `result-cwe-memory-safety.json`
