# Azure Search Index Restore

The purpose of this tool is to load content into an Azure AI Search index. In this sample, data for US Zip Codes as well as data from the NYC Jobs open data dataset is used to create two indexes in Azure AI Search.

## Configuration

Set `TargetSearchServiceName` and `TargetSearchServiceApiKey` in `DataLoader/DataLoader/appsettings.json`, or provide them via the `TARGET_SEARCH_SERVICE_NAME` and `TARGET_SEARCH_SERVICE_API_KEY` environment variables before running the loader.
