# Dependency Map

The two .NET projects declare 25 NuGet packages in their `packages.config` files. The legacy projects do not use central package management or checked-in dependency lock files.

## Dependencies

```mermaid
flowchart LR
    App["JobSearch repository"]
    subgraph WebFramework["Web Frameworks"]
        Mvc["Microsoft.AspNet.Mvc 5.2.2"]
        Razor["Microsoft.AspNet.Razor 3.2.2"]
        WebPages["Microsoft.AspNet.WebPages 3.2.2"]
    end
    subgraph Search["Search and Cloud"]
        AzureCore["Azure.Core 1.4.1"]
        AzureSearch["Azure.Search.Documents 11.1.1"]
        Rest["Microsoft.Rest.ClientRuntime 2.3.20"]
        RestAzure["Microsoft.Rest.ClientRuntime.Azure 3.3.18"]
        Spatial["Microsoft.Spatial 7.5.3"]
        Bing["BingGeocodingHelper 1.1"]
    end
    subgraph Browser["Browser Libraries"]
        Bootstrap["bootstrap 3.4.1"]
        JQuery["jQuery 3.1.1"]
        Modernizr["Modernizr 2.8.3"]
    end
    subgraph Utilities["Utilities"]
        JsonWeb["Newtonsoft.Json 10.0.3"]
        JsonLoader["Newtonsoft.Json 9.0.1"]
        BclAsync["Microsoft.Bcl.AsyncInterfaces 1.0.0"]
        WebInfra["Microsoft.Web.Infrastructure 1.0.0.0"]
        Buffers["System.Buffers 4.5.0"]
        Diagnostic["System.Diagnostics.DiagnosticSource 4.6.0"]
        Memory["System.Memory 4.5.3"]
        Vectors["System.Numerics.Vectors 4.5.0"]
        Unsafe["System.Runtime.CompilerServices.Unsafe 4.6.0"]
        Encodings["System.Text.Encodings.Web 4.6.0"]
        TextJson["System.Text.Json 4.6.0"]
        Tasks["System.Threading.Tasks.Extensions 4.5.2"]
        Tuple["System.ValueTuple 4.5.0"]
    end

    App -->|"web runtime"| WebFramework
    App -->|"search integration"| Search
    App -->|"client UI"| Browser
    App -->|"support libraries"| Utilities
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 3 | ASP.NET MVC, Razor, WebPages | Legacy ASP.NET MVC stack |
| Search and Cloud | 6 | Azure.Search.Documents, Azure.Core, Microsoft.Rest.ClientRuntime | Azure Search and spatial APIs |
| Browser Libraries | 3 | jQuery, Bootstrap, Modernizr | Front-end libraries, including vulnerable jQuery version |
| Utilities | 13 | Newtonsoft.Json, System.Text.Json, System.Memory | Framework support and JSON serialization |

### Version & Compatibility Risks

The projects target .NET Framework 4.7.2 and 4.5 and use older ASP.NET MVC and NuGet packages. The dependency assessment found vulnerable versions of Newtonsoft.Json, Microsoft.Rest.ClientRuntime, System.Text.Encodings.Web, and jQuery; see the security assessment for affected ranges and advisories.

### Notable Observations

- The projects declare different Newtonsoft.Json versions: 10.0.3 for the web application and 9.0.1 for DataLoader.
- `dotnet list package --include-transitive` could not resolve the legacy projects on this host, so this inventory reflects declared packages only; transitive packages are not available from a lock file.
- The web application declares both Newtonsoft.Json and System.Text.Json dependencies.
- No test-scoped dependencies are declared.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | — | No test dependencies or test projects are present in the repository. |

Total test-scope dependencies: 0
No test infrastructure was detected.
