using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Microsoft.Spatial;

namespace NYCJobsWeb;

public class JobsSearch
{
    private readonly SearchClient _indexClient;
    private readonly SearchClient _indexZipClient;

    public JobsSearch(IConfiguration configuration)
    {
        var endpoint = configuration["Search:Endpoint"];
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            endpoint = configuration["Searchendpoint"];
        }

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            var serviceName = configuration["Search:ServiceName"] ?? configuration["SearchServiceName"];
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                endpoint = $"https://{serviceName}.search.windows.net";
            }
        }

        var apiKey = configuration["Search:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            apiKey = configuration["SearchServiceApiKey"];
        }
        if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var searchEndpoint) ||
            string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "Configure Search:Endpoint (or Search:ServiceName) and Search:ApiKey in appsettings.json or environment variables.");
        }

        var indexClient = new SearchIndexClient(searchEndpoint, new AzureKeyCredential(apiKey));
        _indexClient = indexClient.GetSearchClient("nycjobs");
        _indexZipClient = indexClient.GetSearchClient("zipcodes");
    }

    public SearchResults<SearchDocument> Search(
        string searchText,
        string businessTitleFacet,
        string postingTypeFacet,
        string salaryRangeFacet,
        string sortType,
        double lat,
        double lon,
        int currentPage,
        int maxDistance,
        string maxDistanceLat,
        string maxDistanceLon)
    {
        var options = new SearchOptions
        {
            SearchMode = SearchMode.Any,
            Size = 10,
            Skip = Math.Max(0, currentPage - 1),
            IncludeTotalCount = true,
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>"
        };

        var select = new[]
        {
            "id", "agency", "posting_type", "num_of_positions", "business_title",
            "salary_range_from", "salary_range_to", "salary_frequency", "work_location",
            "job_description", "posting_date", "geo_location", "tags"
        };
        foreach (var field in select)
        {
            options.Select.Add(field);
        }

        options.Facets.Add("business_title");
        options.Facets.Add("posting_type");
        options.Facets.Add("level");
        options.Facets.Add("salary_range_from,interval:50000");
        options.HighlightFields.Add("job_description");

        if (sortType == "featured")
        {
            options.ScoringProfile = "jobsScoringFeatured";
            options.ScoringParameters.Add("featuredParam--featured");
            options.ScoringParameters.Add($"mapCenterParam--{GeographyPoint.Create(lon, lat)}");
        }
        else if (sortType == "salaryDesc")
        {
            options.OrderBy.Add("salary_range_from desc");
        }
        else if (sortType == "salaryIncr")
        {
            options.OrderBy.Add("salary_range_from");
        }
        else if (sortType == "mostRecent")
        {
            options.OrderBy.Add("posting_date desc");
        }

        var filters = new List<string>();
        if (!string.IsNullOrEmpty(businessTitleFacet))
        {
            filters.Add($"business_title eq '{EscapeFilterValue(businessTitleFacet)}'");
        }

        if (!string.IsNullOrEmpty(postingTypeFacet))
        {
            filters.Add($"posting_type eq '{EscapeFilterValue(postingTypeFacet)}'");
        }

        if (int.TryParse(salaryRangeFacet, out var salaryRange))
        {
            filters.Add($"salary_range_from ge {salaryRange} and salary_range_from lt {salaryRange + 50000}");
        }

        if (maxDistance > 0 &&
            double.TryParse(maxDistanceLat, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var latitude) &&
            double.TryParse(maxDistanceLon, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var longitude))
        {
            filters.Add($"geo.distance(geo_location, geography'POINT({longitude} {latitude})') le {maxDistance}");
        }

        options.Filter = filters.Count > 0 ? string.Join(" and ", filters) : null;
        return _indexClient.Search<SearchDocument>(searchText, options);
    }

    public SearchResults<SearchDocument> SearchZip(string zipCode)
    {
        return _indexZipClient.Search<SearchDocument>(
            zipCode,
            new SearchOptions { SearchMode = SearchMode.All, Size = 1 });
    }

    public SuggestResults<SearchDocument> Suggest(string searchText, bool fuzzy)
    {
        return _indexClient.Suggest<SearchDocument>(
            searchText,
            "sg",
            new SuggestOptions { UseFuzzyMatching = fuzzy, Size = 8 });
    }

    public SearchDocument LookUp(string id)
    {
        return _indexClient.GetDocument<SearchDocument>(id);
    }

    private static string EscapeFilterValue(string value) => value.Replace("'", "''", StringComparison.Ordinal);
}
