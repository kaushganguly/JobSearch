# Dependency Map

The repository contains 25 declared NuGet package entries across the web application and its separate data-loader utility. The dependencies are primarily an older ASP.NET MVC stack, Azure Search clients, browser libraries, and framework support packages.

## Dependencies

```mermaid
flowchart LR
    App["JobSearch repository"]
    subgraph Web["Web Frameworks"]
        MVC["Microsoft.AspNet.Mvc 5.2.2"]
        Razor["Microsoft.AspNet.Razor 3.2.2"]
        Pages["Microsoft.AspNet.WebPages 3.2.2"]
        WebInfra["Microsoft.Web.Infrastructure 1.0.0.0"]
    end
    subgraph Search["Search and Geospatial"]
        AzureCore["Azure.Core 1.4.1"]
        AzureSearch["Azure.Search.Documents 11.1.1"]
        Rest["Microsoft.Rest.ClientRuntime 2.3.20"]
        RestAzure["Microsoft.Rest.ClientRuntime.Azure 3.3.18"]
        Spatial["Microsoft.Spatial 7.5.3"]
    end
    subgraph Geo["Geocoding"]
        BingGeo["BingGeocodingHelper 1.1"]
    end
    subgraph Browser["Browser Libraries"]
        Bootstrap["bootstrap 3.4.1"]
        JQuery["jQuery 3.1.1"]
        Modernizr["Modernizr 2.8.3"]
    end
    subgraph Support["Serialization and Runtime Support"]
        JsonWeb["Newtonsoft.Json 10.0.3 web"]
        JsonLoader["Newtonsoft.Json 9.0.1 loader"]
        BclAsync["Microsoft.Bcl.AsyncInterfaces 1.0.0"]
        Buffers["System.Buffers 4.5.0"]
        Diagnostics["System.Diagnostics.DiagnosticSource 4.6.0"]
        Memory["System.Memory 4.5.3"]
        Vectors["System.Numerics.Vectors 4.5.0"]
        Unsafe["System.Runtime.CompilerServices.Unsafe 4.6.0"]
        Encodings["System.Text.Encodings.Web 4.6.0"]
        JsonSystem["System.Text.Json 4.6.0"]
        Tasks["System.Threading.Tasks.Extensions 4.5.2"]
        Tuples["System.ValueTuple 4.5.0"]
    end
    App -->|"web framework"| Web
    App -->|"search and location"| Search
    App -->|"geocoding"| Geo
    App -->|"browser UI"| Browser
    App -->|"serialization and support"| Support
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 4 | ASP.NET MVC, Razor, WebPages, Web Infrastructure | Classic System.Web application |
| Search and Geospatial | 5 | Azure.Search.Documents, Azure.Core, Microsoft.Spatial, Microsoft.Rest clients | Azure Search SDKs and supporting clients |
| Geocoding | 1 | BingGeocodingHelper | Declared helper package |
| Browser Libraries | 3 | Bootstrap, jQuery, Modernizr | Legacy browser dependencies |
| Serialization and Runtime Support | 12 | Newtonsoft.Json, System.Text.Json, System.Memory and related packages | Newtonsoft.Json is on two different versions across projects |

### Version & Compatibility Risks

Both projects use legacy, non-SDK project files: the web app targets .NET Framework 4.7.2 and the utility targets .NET Framework 4.5. The MVC/Razor stack and browser packages are old, and the separate utility pins Newtonsoft.Json 9.0.1 while the web app uses 10.0.3. Moving to modern .NET requires replacing or substantially adapting System.Web MVC and reviewing package compatibility.

### Notable Observations

- The web application declares 24 package entries; the separate loader declares one package entry.
- No package lockfile or central package-management file was found; package versions are in `packages.config`.
- The repository's source includes older client-side library copies in addition to NuGet package declarations.
- No ORM, database driver, messaging, or caching dependencies were detected.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | — | No test project or test-scope packages found |

Total test-scope dependencies: 0

No test dependencies or test infrastructure were detected.
