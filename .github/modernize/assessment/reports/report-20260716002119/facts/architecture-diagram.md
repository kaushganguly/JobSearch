# Architecture Diagram

This repository contains a legacy ASP.NET MVC web application and a supporting .NET console utility that both interact with Azure Cognitive Search.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end

    subgraph App["Application Layer"]
        MVC["NYCJobsWeb ASP.NET MVC 5"]
        Ctrl["HomeController"]
        SearchSvc["JobsSearch Service"]
        Loader["DataLoader Console Utility"]
    end

    subgraph Data["Data Layer"]
        SearchIdx[("Azure Cognitive Search nycjobs index")]
        ZipIdx[("Azure Cognitive Search zipcodes index")]
    end

    subgraph External["External Services"]
        Bing["Bing Geocoding API"]
    end

    Browser -->|"HTTP requests"| MVC
    MVC -->|"routes requests"| Ctrl
    Ctrl -->|"query jobs"| SearchSvc
    SearchSvc -->|"search/suggest/lookup"| SearchIdx
    SearchSvc -->|"zip lookup"| ZipIdx
    Ctrl -->|"geocode zip"| Bing
    Loader -->|"backup/restore calls"| SearchIdx
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---|---|---|
| Presentation | ASP.NET MVC | 5.2.2 | Server-rendered web UI and JSON endpoints |
| Application | .NET Framework | 4.7.2 (web), 4.5 (loader) | Business logic and integration |
| Data Access | Azure.Search.Documents SDK | 11.1.1 | Query Azure Cognitive Search indexes |
| External Integration | BingGeocodingHelper | 1.1 | Resolve ZIP/geolocation inputs |

### Data Storage & External Services

The application relies on Azure Cognitive Search indexes (`nycjobs` and `zipcodes`) as its primary data source rather than a relational database. It also integrates with Bing geocoding to support location-aware filtering in search requests.

### Key Architectural Decisions

- Uses a thin MVC controller that delegates search operations to a dedicated `JobsSearch` service.
- Uses Azure Cognitive Search as the operational query store for job content and facets.
- Keeps backup/restore capabilities in a separate console application (`DataLoader`).

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation
        HomeCtrl["HomeController"]
        Views["Razor Views"]
    end

    subgraph Business["Business Logic"]
        JobsSearch["JobsSearch"]
    end

    subgraph DataAccess["Data Access"]
        SearchClient["SearchClient"]
        SearchIndexClient["SearchIndexClient"]
    end

    subgraph Infra["Infrastructure"]
        RouteConfig["RouteConfig"]
        AppStart["MvcApplication Application_Start"]
    end

    HomeCtrl -->|"returns"| Views
    HomeCtrl -->|"delegates"| JobsSearch
    JobsSearch -->|"creates"| SearchIndexClient
    SearchIndexClient -->|"creates clients"| SearchClient
    AppStart -->|"registers"| RouteConfig
    RouteConfig -->|"routes to"| HomeCtrl
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| HomeController | Presentation | MVC Controller | Handles search/suggest/lookup requests |
| Index.cshtml / JobDetails.cshtml | Presentation | Razor Views | Renders UI pages |
| JobsSearch | Business Logic | Service Class | Builds search options and executes queries |
| SearchIndexClient/SearchClient | Data Access | Azure SDK Clients | Communicate with search indexes |
| RouteConfig | Infrastructure | Route Registration | Defines MVC route template |
| MvcApplication | Infrastructure | Startup Class | Bootstraps MVC routing |
