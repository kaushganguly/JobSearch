# Dependency Map

JobSearch contains two legacy .NET Framework projects with 25 declared NuGet package entries across their `packages.config` files. The map summarizes those direct dependencies by project and functional category.

## Dependencies

```mermaid
flowchart LR
    Web["NYCJobsWeb"]
    Loader["DataLoader"]

    subgraph WebFramework["Web Frameworks"]
        MVC["Microsoft.AspNet.Mvc 5.2.2"]
        Razor["Microsoft.AspNet.Razor 3.2.2"]
        WebPages["Microsoft.AspNet.WebPages 3.2.2"]
        WebInfra["Microsoft.Web.Infrastructure 1.0.0.0"]
    end
    subgraph Azure["Search and Azure"]
        AzureCore["Azure.Core 1.4.1"]
        SearchDocuments["Azure.Search.Documents 11.1.1"]
        RestClient["Microsoft.Rest.ClientRuntime 2.3.20"]
        RestAzure["Microsoft.Rest.ClientRuntime.Azure 3.3.18"]
        Spatial["Microsoft.Spatial 7.5.3"]
    end
    subgraph Browser["Browser Utilities"]
        Bootstrap["bootstrap 3.4.1"]
        JQuery["jQuery 3.1.1"]
        Modernizr["Modernizr 2.8.3"]
    end
    subgraph Serialization["Serialization and Framework Utilities"]
        Bing["BingGeocodingHelper 1.1"]
        JsonWeb["Newtonsoft.Json 10.0.3"]
        Async["Microsoft.Bcl.AsyncInterfaces 1.0.0"]
        Buffers["System.Buffers 4.5.0"]
        Diagnostics["System.Diagnostics.DiagnosticSource 4.6.0"]
        Memory["System.Memory 4.5.3"]
        Vectors["System.Numerics.Vectors 4.5.0"]
        Unsafe["System.Runtime.CompilerServices.Unsafe 4.6.0"]
        Encodings["System.Text.Encodings.Web 4.6.0"]
        Json["System.Text.Json 4.6.0"]
        Tasks["System.Threading.Tasks.Extensions 4.5.2"]
        Tuple["System.ValueTuple 4.5.0"]
        JsonLoader["Newtonsoft.Json 9.0.1"]
    end

    Web -->|"web framework"| WebFramework
    Web -->|"Azure search SDK"| Azure
    Web -->|"browser and utility packages"| Browser
    Web -->|"other declared packages"| Serialization
    Loader -->|"JSON serialization"| JsonLoader
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 4 | ASP.NET MVC 5.2.2, Razor 3.2.2, WebPages 3.2.2 | Legacy ASP.NET MVC stack targeting .NET Framework |
| Search and Azure | 5 | Azure.Search.Documents 11.1.1, Azure.Core 1.4.1 | Used by the web application's Azure Search integration |
| Browser Utilities | 3 | Bootstrap 3.4.1, jQuery 3.1.1 | Older client-side presentation dependencies |
| Serialization and Framework Utilities | 13 | Newtonsoft.Json 10.0.3/9.0.1, System.Text.Json 4.6.0 | Includes framework compatibility packages and the data-loader JSON dependency |

### Version & Compatibility Risks

Both projects target .NET Framework 4.5 or 4.7.2, and the package manifests contain older releases of ASP.NET MVC, jQuery, Newtonsoft.Json, and Azure libraries. These versions may present compatibility and support concerns when upgrading to modern .NET; the projects also use legacy `packages.config` dependency management.

### Notable Observations

- The solution pins two different Newtonsoft.Json versions: 10.0.3 in the web project and 9.0.1 in the loader.
- The web project references Azure SDK and legacy Microsoft REST client packages together.
- Several `System.*` packages support compatibility with the older target frameworks.
- BingGeocodingHelper is declared by the web project, while its use is not evident from package metadata alone.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | — | No test-scoped packages are declared in either `packages.config` |

Total test-scope dependencies: 0

No test dependencies detected in the available package manifests.
