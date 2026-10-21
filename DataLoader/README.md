# Azure Search Index Restore

The purpose of this tool is to load contend into an Azure Search index.  In this sample, data for US Zip Codes as well as data from the NYC Jobs opendata dataset is used to create two indexes in Azure Search.  Please note, the data in the either datasets should not be considered up-to-date.

The .NET 10 data loader reads its target service from the `TARGET_SEARCH_SERVICE_NAME`
and `TARGET_SEARCH_SERVICE_API_KEY` environment variables. Run it from the repository
or from a build output beneath the repository so it can locate `NYCJobsWeb/Schema_and_Data`.