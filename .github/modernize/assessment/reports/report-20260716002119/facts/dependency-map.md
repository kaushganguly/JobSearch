# Dependency Map

This repository has two .NET Framework projects with a moderate dependency footprint focused on web UI and Azure Cognitive Search integration.

## Dependencies

```mermaid
flowchart LR
    App["JobSearch Solution"]

    subgraph Web["Web Frameworks"]
        AspMvc["Microsoft.AspNet.Mvc 5.2.2"]
        Razor["Microsoft.AspNet.Razor 3.2.2"]
        WebPages["Microsoft.AspNet.WebPages 3.2.2"]
    end

    subgraph Search["Utilities"]
        AzureCore["Azure.Core 1.4.1"]
        AzureSearch["Azure.Search.Documents 11.1.1"]
        BingGeo["BingGeocodingHelper 1.1"]
    end

    subgraph Log["Logging"]
        Json["Newtonsoft.Json 10.0.3"]
    end

    subgraph Util["Utilities"]
        Buffers["System.Buffers 4.5.0"]
        Memory["System.Memory 4.5.3"]
        Unsafe["System.Runtime.CompilerServices.Unsafe 4.6.0"]
        TextJson["System.Text.Json 4.6.0"]
        TasksExt["System.Threading.Tasks.Extensions 4.5.2"]
    end

    App -->|"web"| Web
    App -->|"search integration"| Search
    App -->|"serialization"| Log
    App -->|"runtime helpers"| Util
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 3 | Microsoft.AspNet.Mvc, Razor, WebPages | Legacy ASP.NET MVC stack |
| Utilities | 8 | Azure.Search.Documents, Azure.Core, System.Text.Json | Search SDK and compatibility helpers |
| Logging | 1 | Newtonsoft.Json | JSON serialization used in both projects |
| Security | 0 | None detected | No auth-specific package declared |

### Version & Compatibility Risks

The projects target .NET Framework 4.7.2 and 4.5, which introduces upgrade pressure and compatibility work for modernization. Several support libraries are older versions pinned via `packages.config`, which typically requires migration to newer packages and/or PackageReference during upgrades.

### Notable Observations

- Uses `packages.config` instead of modern PackageReference dependency management.
- Multiple legacy ASP.NET MVC packages indicate tight coupling to System.Web.
- DataLoader and web app use different Newtonsoft.Json major versions (9.0.1 vs 10.0.3).
- Search functionality depends on Azure.Search.Documents as the primary external integration.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | N/A | No test project/package references found |

Total test-scope dependencies: 0
No test dependencies detected.
