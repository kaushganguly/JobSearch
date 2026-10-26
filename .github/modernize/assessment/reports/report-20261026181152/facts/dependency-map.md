# Dependency Map

JobSearch consists of an ASP.NET MVC web application and a separate Azure Search data-loader utility. The two `packages.config` files declare 25 package entries across the projects (24 for the web application and one for the data loader).

## Dependencies

```mermaid
flowchart LR
    App["JobSearch"]
    subgraph Web["Web Frameworks and UI"]
        MVC["Microsoft.AspNet.Mvc 5.2.2"]
        Razor["Microsoft.AspNet.Razor 3.2.2"]
        WebPages["Microsoft.AspNet.WebPages 3.2.2"]
        WebInfra["Microsoft.Web.Infrastructure 1.0.0.0"]
        Bootstrap["bootstrap 3.4.1"]
        JQuery["jQuery 3.1.1"]
        Modernizr["Modernizr 2.8.3"]
    end
    subgraph Search["Search and Cloud"]
        AzureCore["Azure.Core 1.4.1"]
        AzureSearch["Azure.Search.Documents 11.1.1"]
        Bing["BingGeocodingHelper 1.1"]
        ClientRuntime["Microsoft.Rest.ClientRuntime 2.3.20"]
        AzureClientRuntime["Microsoft.Rest.ClientRuntime.Azure 3.3.18"]
        Spatial["Microsoft.Spatial 7.5.3"]
    end
    subgraph JSON["Serialization"]
        JsonWeb["Newtonsoft.Json 10.0.3"]
        JsonLoader["Newtonsoft.Json 9.0.1"]
    end
    subgraph Util["Utilities"]
        Async["Microsoft.Bcl.AsyncInterfaces 1.0.0"]
        Buffers["System.Buffers 4.5.0"]
        Diagnostic["System.Diagnostics.DiagnosticSource 4.6.0"]
        Memory["System.Memory 4.5.3"]
        Vectors["System.Numerics.Vectors 4.5.0"]
        Unsafe["System.Runtime.CompilerServices.Unsafe 4.6.0"]
        Encodings["System.Text.Encodings.Web 4.6.0"]
        SystemJson["System.Text.Json 4.6.0"]
        Tasks["System.Threading.Tasks.Extensions 4.5.2"]
        Tuples["System.ValueTuple 4.5.0"]
    end
    App -->|"web user interface"| Web
    App -->|"Azure Search integration"| Search
    App -->|"JSON processing"| JSON
    App -->|"runtime support"| Util
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks and UI | 7 | ASP.NET MVC 5.2.2, Razor 3.2.2, Bootstrap 3.4.1, jQuery 3.1.1 | Classic ASP.NET MVC and browser UI packages |
| Search and Cloud | 6 | Azure.Search.Documents 11.1.1, Azure.Core 1.4.1, BingGeocodingHelper 1.1 | Search access, Azure support, and geocoding |
| Serialization | 2 | Newtonsoft.Json 10.0.3, Newtonsoft.Json 9.0.1 | Separate package versions in web and data-loader projects |
| Utilities | 10 | System.Text.Json 4.6.0, System.Memory 4.5.3, System.Buffers 4.5.0 | Compatibility and runtime support packages |

### Version & Compatibility Risks

The projects target .NET Framework 4.7.2 and 4.5 and use the legacy `packages.config` dependency model. Several packages and UI libraries are older releases, and the data loader's Newtonsoft.Json 9.0.1 differs from the web project's 10.0.3. Package compatibility and framework support should be reviewed as part of any future migration.

### Notable Observations

- Newtonsoft.Json is declared at different versions in the two projects.
- The web project includes both Newtonsoft.Json and System.Text.Json package references.
- The app uses the Azure Search SDK while the separate loader talks to the service through REST.
- No package-managed test framework or test-scoped dependencies were detected.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|

Total test-scope dependencies: 0

No test dependencies were detected in the package manifests, and no test project was found.
