# Architecture Diagram

This solution contains a legacy ASP.NET MVC web app and a console data-loader utility, both centered on Azure AI Search indexes for NYC jobs and zipcode geodata.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end
    subgraph App["Application Layer - ASP.NET MVC 5 on .NET Framework 4.7.2"]
        HomeCtrl["HomeController"]
        SearchSvc["JobsSearch"]
    end
    subgraph Loader["Data Loader - .NET Framework 4.5 Console"]
        ImportTool["AzureSearchBackupRestore"]
    end
    subgraph Data["Data Layer"]
        JobIndex[("Azure AI Search index nycjobs")]
        ZipIndex[("Azure AI Search index zipcodes")]
    end
    subgraph External["External Services"]
        BingGeo["Bing Geocoding API"]
        AzureSearch["Azure Search endpoint"]
    end

    Browser -->|"HTTP requests"| HomeCtrl
    HomeCtrl -->|"query/suggest/lookup"| SearchSvc
    SearchSvc -->|"search docs"| JobIndex
    SearchSvc -->|"zip geo lookup"| ZipIndex
    HomeCtrl -->|"zip geocoding"| BingGeo
    JobIndex -->|"served by"| AzureSearch
    ZipIndex -->|"served by"| AzureSearch
    ImportTool -->|"create/delete index"| AzureSearch
    ImportTool -->|"upload nycjobs/zipcodes JSON"| JobIndex
    ImportTool -->|"upload zipcode JSON"| ZipIndex
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---|---|---|
| Presentation | ASP.NET MVC | 5.2.2 | Server-rendered UI and JSON endpoints |
| Application | .NET Framework | 4.7.2 / 4.5 | Web app and import utility runtime |
| Search/Data | Azure.Search.Documents SDK | 11.1.1 | Query Azure Search indexes |
| Integration | BingGeocodingHelper | 1.1 | Convert zipcode to coordinates for distance filtering |

### Data Storage & External Services

The application does not use a relational database. It depends on Azure AI Search indexes (`nycjobs`, `zipcodes`) as its primary data store and uses Bing geocoding to support location-based filtering.

### Key Architectural Decisions

- The web app delegates all search behavior to a single `JobsSearch` service class used directly by `HomeController`.
- Search results are returned as JSON payloads to the browser while MVC views are used for initial page rendering.
- A separate console utility owns index re-creation and JSON-based bulk data import.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation
        HomeController["HomeController"]
        IndexView["Index.cshtml"]
        DetailView["JobDetails.cshtml"]
    end
    subgraph Business["Business Logic"]
        JobsSearchComp["JobsSearch"]
    end
    subgraph DataAccess["Data Access"]
        SearchClientComp["Azure SearchClient"]
        JobModel["NYCJob / NYCJobLookup"]
    end
    subgraph Infra["Infrastructure"]
        RouteConfigComp["RouteConfig"]
        GlobalApp["MvcApplication"]
        LoaderProgram["DataLoader Program"]
        SearchHelper["AzureSearchHelper"]
    end

    GlobalApp -->|"registers routes"| RouteConfigComp
    HomeController -->|"renders"| IndexView
    HomeController -->|"renders"| DetailView
    HomeController -->|"delegates search operations"| JobsSearchComp
    JobsSearchComp -->|"executes search and suggest"| SearchClientComp
    HomeController -->|"maps response"| JobModel
    LoaderProgram -->|"uses"| SearchHelper
    SearchHelper -->|"issues REST calls"| SearchClientComp
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| HomeController | Presentation | MVC Controller | Handles search, suggest, lookup, and view routes |
| JobsSearch | Business Logic | Service class | Builds Azure Search queries and filters |
| NYCJob / NYCJobLookup | Data Access | DTO model | Shapes API JSON payloads for UI consumption |
| RouteConfig | Infrastructure | Routing config | Maps default controller/action routes |
| MvcApplication | Infrastructure | App bootstrap | Registers areas/routes on startup |
| Program (DataLoader) | Infrastructure | Console entrypoint | Orchestrates index delete/create/import workflow |
| AzureSearchHelper | Infrastructure | HTTP helper | Sends Azure Search REST API requests |
