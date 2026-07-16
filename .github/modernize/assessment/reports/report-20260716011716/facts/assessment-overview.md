# Assessment Overview

This directory contains supplementary architectural and technical analysis documents generated as part of the application assessment for the **NYC Jobs Search** solution. Use the links below to navigate to each document.

## Supplementary Documents

| Document | Description |
|----------|-------------|
| [Architecture Diagram](./architecture-diagram.md) | Two-layer visual diagram of the application architecture (technology stack, data flows) and component relationships (controller, service, data-access layer interactions) |
| [Dependency Map](./dependency-map.md) | Visual map of all external NuGet package dependencies grouped by functional category (Web Frameworks, Azure/Search SDK, Geolocation, UI, Serialization, Runtime polyfills), with version risk analysis and test dependency inventory |
| [API & Service Contracts](./api-service-contracts.md) | Catalog of all HTTP endpoints, request/response types, service communication patterns, DTOs, security posture, and a sequence diagram of the primary search request flow |
| [Data Architecture](./data-architecture.md) | Azure Cognitive Search index schemas (nycjobs, zipcodes), data ownership boundaries, key query methods, caching strategy, and data classification/sensitivity analysis |
| [Configuration Inventory](./configuration-inventory.md) | Inventory of all configuration sources (Web.config, app.config), build profiles (Debug/Release), runtime settings, secrets handling, startup dependency chain, and framework version catalog |
| [Business Workflows](./business-workflows.md) | Documentation of core business workflows (job search, proximity filtering, autocomplete, job detail lookup, index seeding), domain entities, business rules, decision logic, and error handling patterns |
