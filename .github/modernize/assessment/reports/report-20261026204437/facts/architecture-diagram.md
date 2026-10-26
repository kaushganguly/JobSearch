# Architecture Diagram

The repository contains a public job-search web application and a separate console utility for loading sample data. Both use Azure AI Search indexes as their shared data boundary.

## Application Architecture

```mermaid
flowchart TD
    subgraph Client["Client Layer"]
        Browser["Web Browser"]
    end
    subgraph WebApp["Web Application - ASP.NET MVC 5"]
        Views["Razor Views and JavaScript"]
        Controller["HomeController"]
        SearchLogic["JobsSearch"]
    end
    subgraph DataLayer["Search Data"]
        JobsIndex[("Azure AI Search - nycjobs")]
        ZipIndex[("Azure AI Search - zipcodes")]
    end
    subgraph Importer["Data Loading Utility"]
        Loader["DataLoader Console Application"]
        SampleFiles["Local JSON and schema files"]
    end

    Browser -->|"HTTP requests and JSON responses"| Views
    Views -->|"AJAX search and lookup requests"| Controller
    Controller -->|"search, suggest, and lookup calls"| SearchLogic
    SearchLogic -->|"job search and document lookup"| JobsIndex
    SearchLogic -->|"postal code search"| ZipIndex
    SampleFiles -->|"read and upload"| Loader
    Loader -->|"create indexes and upload documents"| JobsIndex
    Loader -->|"create indexes and upload documents"| ZipIndex
```

### Technology Stack Summary

| Layer | Technology | Version | Purpose |
|---|---|---:|---|
| Web presentation | ASP.NET MVC / Razor | MVC 5.2.2 | Renders job search and detail pages |
| Web runtime | .NET Framework | 4.7.2 | Hosts the MVC application |
| Search access | Azure.Search.Documents | 11.1.1 | Queries Azure AI Search |
| Data loading | .NET Framework console application | 4.5 | Creates and populates search indexes |
| Browser interface | JavaScript, jQuery, Bootstrap | jQuery 3.1.1; Bootstrap 3.4.1 | Search interaction and page presentation |

### Data Storage & External Services

Azure AI Search stores job postings and postal-code location records in the `nycjobs` and `zipcodes` indexes. The web application reads those indexes, while DataLoader reads repository JSON/schema files and sends index-management and document-upload requests. No relational database, message broker, or cache is configured.

### Key Architectural Decisions

- The web application and data loader are separate projects that share Azure Search indexes rather than a relational database.
- Job search, suggestions, and document lookup are handled by one MVC controller and a dedicated search wrapper.
- Sample records and index schemas are repository files used by the standalone loader.

## Component Relationships

```mermaid
flowchart LR
    subgraph Presentation["Presentation"]
        SearchView["Search and detail Razor views"]
        HomeController["HomeController"]
    end
    subgraph Application["Application Logic"]
        JobsSearch["JobsSearch"]
    end
    subgraph Import["Data Import"]
        LoaderProgram["DataLoader Program"]
        SearchHelper["AzureSearchHelper"]
        SampleData["Repository sample data"]
    end

    SearchView -->|"AJAX actions"| HomeController
    HomeController -->|"delegates search operations"| JobsSearch
    LoaderProgram -->|"sends REST requests"| SearchHelper
    LoaderProgram -->|"reads schemas and JSON"| SampleData
```

### Component Inventory

| Component | Layer | Type | Responsibility |
|---|---|---|---|
| Search and detail Razor views | Presentation | MVC views | Accept search input and render results and job details |
| HomeController | Presentation | MVC controller | Exposes search, suggestion, detail, and page actions |
| JobsSearch | Application Logic | Search client wrapper | Builds Azure Search queries and retrieves documents |
| DataLoader Program | Data Import | Console entry point | Deletes/recreates indexes and uploads sample records |
| AzureSearchHelper | Data Import | HTTP helper | Sends REST requests and serializes JSON payloads |
| Repository sample data | Data Import | JSON/schema files | Provides index schemas and sample job/postal-code records |
