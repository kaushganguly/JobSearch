# Core Business Workflows

The application helps users search NYC job posting data, filter by facets and location, request suggestions, and view details for selected postings.

## Domain Entities

| Entity | Service / Bounded Context | Description | Key Relationships |
|---|---|---|---|
| Job Posting | NYCJobsWeb / Job Search | A searchable NYC job posting with agency, title, salary, location, and description information | Returned as Azure Search documents in search and lookup responses |
| Zip Code Location | NYCJobsWeb / Location Filtering | A zip-code lookup record used to translate a user-selected zip code into coordinates | Used to build distance filters for job searches |
| Search Result Set | NYCJobsWeb / Job Search | Aggregated search response containing matching jobs, facets, and total count | Composes job documents and facet counts |
| Suggestion | NYCJobsWeb / Search Assistance | Autocomplete text returned for a user-entered term | Derived from the Azure Search suggester |
| Search Index Schema | DataLoader / Index Provisioning | Definition of the Azure Search indexes loaded from schema files | Drives index creation before document import |

## Service-to-Domain Mapping

| Service | Domain Context | Owned Entities | External Dependencies |
|---|---|---|---|
| NYCJobsWeb | Job Search user experience | Search result set, lookup response, suggestion response | Azure AI Search `nycjobs` and `zipcodes` indexes |
| DataLoader | Search index provisioning | Search index schema and seed data batches | Azure AI Search REST API and local schema/data files |
| Azure AI Search | Searchable job and zip-code storage | Job posting documents and zip-code documents | Managed Azure service outside the repository |

## Primary Workflows

### Workflow 1: Search for Jobs

A user submits a search query from the web UI. If the query is blank, the controller treats it as a wildcard search. If a distance filter is requested, the controller first looks up the selected zip code to obtain coordinates, then calls `JobsSearch.Search` with text, facet, salary, sort, and distance parameters. Azure AI Search returns matching documents, facets, and a total count; the controller serializes these as `NYCJob` JSON.

### Workflow 2: Get Search Suggestions

A user types into the search box and the UI calls the suggestion action with a term and fuzzy-search flag. `JobsSearch.Suggest` calls the Azure Search suggester named `sg`, the controller removes duplicate suggestion text, and the response is returned as a JSON string list.

### Workflow 3: View Job Details

A user selects a job posting. The lookup action requires an id, calls `JobsSearch.LookUp`, and returns a `NYCJobLookup` JSON wrapper around the retrieved Azure Search document. If the id is missing, the action returns null rather than a typed error response.

### Workflow 4: Provision Search Indexes

An operator runs the DataLoader console application. The tool reads target search service settings, deletes existing `zipcodes` and `nycjobs` indexes, recreates each index from schema files, and uploads matching JSON data files in batches through the Azure Search REST API.

## Cross-Service Data Flows

The primary data composition flow is query-time enrichment inside the web application. A distance-filtered job search first reads the `zipcodes` index to obtain coordinates, then uses those coordinates to filter the `nycjobs` index. The data loader writes both indexes but does not communicate with the web process directly. No circuit breaker or fallback policy is implemented; if Azure Search calls fail, helper methods catch exceptions and return null or incomplete data, which may surface as missing results or runtime errors in controller actions.

## Business Workflow Sequence

```mermaid
sequenceDiagram
    participant User as "Job Seeker"
    participant UI as "Search Page"
    participant Controller as "HomeController"
    participant SearchSvc as "JobsSearch"
    participant ZipIndex as "Zip Code Index"
    participant JobIndex as "Job Posting Index"

    User->>UI: Enter search terms and filters
    UI->>Controller: Request job search
    alt Blank query
        Controller->>Controller: Treat query as wildcard
    else Query supplied
        Controller->>Controller: Use supplied query
    end
    alt Distance filter selected
        Controller->>SearchSvc: Resolve zip code
        SearchSvc->>ZipIndex: Find zip location
        ZipIndex-->>SearchSvc: Coordinates
        SearchSvc-->>Controller: Coordinates
    else No distance filter
        Controller->>Controller: Skip location enrichment
    end
    Controller->>SearchSvc: Search jobs with business filters
    SearchSvc->>JobIndex: Execute faceted search
    alt Search succeeds
        JobIndex-->>SearchSvc: Matching jobs and facets
        SearchSvc-->>Controller: Results
        Controller-->>UI: Search result JSON
        UI-->>User: Display jobs and facets
    else Search fails
        Note over Controller: No fallback policy; response may be empty or error
    end
```

## Business Rules & Decision Logic

- Blank search terms are converted to a wildcard query so users can browse all indexed postings.
- Distance filtering is conditional on `maxDistance` being greater than zero; when enabled, zip-code coordinates are used to build a geospatial search filter.
- Supported sort decisions include featured scoring, salary descending, salary increasing, and most recent posting date.
- Facet filters for business title, posting type, and salary range are combined with logical `and` conditions.
- Suggestion responses are de-duplicated before being returned to the browser.
- Index provisioning always deletes and recreates each target index before importing JSON documents, so running the loader is a destructive refresh workflow.
- No business-level authorization, audit trail, state machine, or transaction boundary was detected.
