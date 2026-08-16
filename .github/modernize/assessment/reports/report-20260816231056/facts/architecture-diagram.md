# Architecture Diagram

The repository contains a legacy ASP.NET MVC web application for searching NYC job postings and a companion console data loader for provisioning Azure AI Search indexes.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end
    subgraph WebApp["NYCJobsWeb - ASP.NET MVC 5 on .NET Framework 4.7.2"]
        Views["Razor Views and Static Assets"]
        HomeController["HomeController"]
        JobsSearch["JobsSearch Service"]
        Models["NYCJob View Models"]
    end
    subgraph Loader["DataLoader - .NET Framework 4.5 Console"]
        Program["Import Orchestrator"]
        SearchHelper["AzureSearchHelper"]
        SchemaData["Schema and JSON Data Files"]
    end
    subgraph Azure["Azure Services"]
        Search[("Azure AI Search nycjobs index")]
        ZipSearch[("Azure AI Search zipcodes index")]
        Bing["Bing Geocoding Helper"]
    end

    Browser -->|"MVC pages and AJAX JSON requests"| Views
    Views -->|"routes to actions"| HomeController
    HomeController -->|"search suggest lookup"| JobsSearch
    JobsSearch -->|"typed SDK queries"| Search
    JobsSearch -->|"zip proximity lookup"| ZipSearch
    JobsSearch -->|"geocoding package available"| Bing
    HomeController -->|"serializes"| Models
    Program -->|"reads"| SchemaData
    Program -->|"create and upload indexes"| SearchHelper
    SearchHelper -->|"REST requests"| Search
    SearchHelper -->|"REST requests"| ZipSearch
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---:|---:|---|
| Presentation | ASP.NET MVC | 5.2.2 | Server-side MVC routing, controllers, and Razor views |
| Runtime | .NET Framework | 4.7.2 | Web application target framework |
| Runtime | .NET Framework | 4.5 | Console data loader target framework |
| Search Integration | Azure.Search.Documents | 11.1.1 | Query Azure AI Search indexes from the web app |
| Search Integration | Azure Search REST API | 2015-02-28-Preview | Data loader index creation and document upload |
| Serialization | Newtonsoft.Json | 10.0.3 / 9.0.1 | JSON serialization in web and loader projects |
| Client UI | Bootstrap, jQuery, Modernizr | 3.4.1, 3.1.1, 2.8.3 | Browser styling and interactivity |

### Data Storage & External Services

The application does not use a relational database or ORM. Persistent searchable data is stored externally in Azure AI Search indexes named `nycjobs` and `zipcodes`; the loader recreates these indexes from schema and JSON seed files under `NYCJobsWeb/Schema_and_Data`.

### Key Architectural Decisions

- The web application uses a simple MVC controller plus helper-service pattern rather than dependency injection.
- Search is delegated to Azure AI Search, so filtering, faceting, suggestions, and document lookup are handled by the external search service.
- The data loader is a separate console utility that provisions the same external indexes used by the web application.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation["Presentation"]
        RouteConfig["RouteConfig"]
        HomeController2["HomeController"]
        RazorViews["Razor Views"]
    end
    subgraph Business["Business Logic"]
        JobsSearch2["JobsSearch"]
        LoaderProgram["DataLoader Program"]
    end
    subgraph Contracts["Contracts"]
        NYCJob["NYCJob"]
        NYCJobLookup["NYCJobLookup"]
        SearchDocument["SearchDocument"]
    end
    subgraph Infrastructure["Infrastructure"]
        Config["Web.config and App.config"]
        AzureSdk["Azure Search SDK"]
        RestHelper["AzureSearchHelper"]
        JsonFiles["Schema and Data Files"]
    end
    subgraph External["External Services"]
        AzureSearch["Azure AI Search"]
    end

    RouteConfig -->|"maps default route"| HomeController2
    RazorViews -->|"AJAX requests"| HomeController2
    HomeController2 -->|"delegates queries"| JobsSearch2
    HomeController2 -->|"returns JSON"| NYCJob
    HomeController2 -->|"returns JSON"| NYCJobLookup
    JobsSearch2 -->|"uses"| Config
    JobsSearch2 -->|"uses"| AzureSdk
    AzureSdk -->|"queries"| AzureSearch
    LoaderProgram -->|"reads settings"| Config
    LoaderProgram -->|"reads schemas and documents"| JsonFiles
    LoaderProgram -->|"sends REST calls"| RestHelper
    RestHelper -->|"calls"| AzureSearch
    NYCJob -->|"contains"| SearchDocument
    NYCJobLookup -->|"contains"| SearchDocument
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| `HomeController` | Presentation | MVC Controller | Serves home and details views; exposes search, suggest, and lookup JSON actions |
| `RouteConfig` | Presentation | MVC Route Configuration | Registers the default `{controller}/{action}/{id}` route |
| Razor views and static assets | Presentation | Views and client assets | Render the search UI and invoke controller actions |
| `JobsSearch` | Business Logic | Search service helper | Builds Azure AI Search options, filters, facets, sorting, suggestions, and document lookups |
| `NYCJob` | Contracts | Response model | Wraps search results, facets, and total count for JSON responses |
| `NYCJobLookup` | Contracts | Response model | Wraps a single Azure Search document for details lookup |
| `DataLoader.Program` | Business Logic | Console orchestrator | Deletes, creates, and populates Azure Search indexes from local JSON files |
| `AzureSearchHelper` | Infrastructure | REST helper | Adds API version query parameters and sends Azure Search REST requests |
| `Web.config` / `App.config` | Infrastructure | Configuration | Stores service names and API-key references for web and loader projects |
