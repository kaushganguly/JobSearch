# NYC Jobs Demo Data & Schema

This is the data used for the Azure AI Search Jobs demo website based on data from the NYC Open Data initiative. Jobs listed here should not be considered active or accurate.

## Configuration

Configure the web application with a valid Azure AI Search endpoint and query key by setting `Search:Endpoint`, `Search:ServiceName`, and `Search:ApiKey` in `NYCJobsWeb/appsettings.json`, or override them with environment variables supported by your host.

To import this data and schema manually, tools such as Fiddler or Postman are convenient for uploading the files. For more details on how to upload documents with the Azure AI Search REST API, see: https://learn.microsoft.com/azure/search/search-howto-reindex
