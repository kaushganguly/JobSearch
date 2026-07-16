using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using NYCJobsWeb.Models;
using System.Globalization;
using System.Text.Json;

namespace NYCJobsWeb;

public class JobsSearch
{
    private static readonly string[] SelectFields =
    {
        "id", "agency", "posting_type", "num_of_positions", "business_title", "salary_range_from",
        "salary_range_to", "salary_frequency", "work_location", "job_description", "posting_date",
        "geo_location", "tags", "job_id", "civil_service_title", "division_work_unit",
        "hours_per_shift", "level", "minimum_qual_requirements", "preferred_skills",
        "residency_requirement", "to_apply"
    };

    private static readonly string[] FacetFields =
    {
        "business_title", "posting_type", "level", "salary_range_from,interval:50000"
    };

    private readonly SearchClient? _jobsClient;
    private readonly SearchClient? _zipClient;

    public JobsSearch(IConfiguration configuration)
    {
        var endpoint = configuration["Search:Endpoint"];
        var serviceName = configuration["Search:ServiceName"] ?? configuration["SearchServiceName"];
        var apiKey = configuration["Search:ApiKey"] ?? configuration["SearchServiceApiKey"];

        if (string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(serviceName))
        {
            endpoint = $"https://{serviceName}.search.windows.net";
        }

        if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var serviceEndpoint))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Contains('<', StringComparison.Ordinal))
        {
            return;
        }

        var credential = new AzureKeyCredential(apiKey);
        _jobsClient = new SearchClient(serviceEndpoint, "nycjobs", credential);
        _zipClient = new SearchClient(serviceEndpoint, "zipcodes", credential);
    }

    public NYCJob Search(string searchText, string businessTitleFacet, string postingTypeFacet, string salaryRangeFacet,
        string sortType, double lat, double lon, int currentPage, int maxDistance, string maxDistanceLat, string maxDistanceLon)
    {
        if (_jobsClient is null)
        {
            return new NYCJob();
        }

        var searchOptions = new SearchOptions
        {
            SearchMode = SearchMode.Any,
            Size = 10,
            Skip = Math.Max(0, currentPage - 1) * 10,
            IncludeTotalCount = true,
            HighlightPreTag = "<b>",
            HighlightPostTag = "</b>",
        };

        AddList(searchOptions.Select, SelectFields);
        AddList(searchOptions.Facets, FacetFields);
        searchOptions.HighlightFields.Add("job_description");

        if (sortType == "featured")
        {
            searchOptions.ScoringProfile = "jobsScoringFeatured";
            searchOptions.ScoringParameters.Add("featuredParam--featured");
            searchOptions.ScoringParameters.Add($"mapCenterParam--{lon.ToString(CultureInfo.InvariantCulture)},{lat.ToString(CultureInfo.InvariantCulture)}");
        }
        else if (sortType == "salaryDesc")
        {
            searchOptions.OrderBy.Add("salary_range_from desc");
        }
        else if (sortType == "salaryIncr")
        {
            searchOptions.OrderBy.Add("salary_range_from");
        }
        else if (sortType == "mostRecent")
        {
            searchOptions.OrderBy.Add("posting_date desc");
        }

        var filters = new List<string>();
        if (!string.IsNullOrWhiteSpace(businessTitleFacet))
        {
            filters.Add($"business_title eq '{businessTitleFacet.Replace("'", "''")}'");
        }

        if (!string.IsNullOrWhiteSpace(postingTypeFacet))
        {
            filters.Add($"posting_type eq '{postingTypeFacet.Replace("'", "''")}'");
        }

        if (!string.IsNullOrWhiteSpace(salaryRangeFacet) && int.TryParse(salaryRangeFacet, out var salaryRange))
        {
            filters.Add($"salary_range_from ge {salaryRange} and salary_range_from lt {salaryRange + 50000}");
        }

        if (maxDistance > 0 && !string.IsNullOrWhiteSpace(maxDistanceLat) && !string.IsNullOrWhiteSpace(maxDistanceLon))
        {
            filters.Add($"geo.distance(geo_location, geography'POINT({maxDistanceLon} {maxDistanceLat})') le {maxDistance}");
        }

        if (filters.Count > 0)
        {
            searchOptions.Filter = string.Join(" and ", filters);
        }

        var response = _jobsClient.Search<SearchDocument>(searchText, searchOptions);
        return new NYCJob
        {
            Count = Convert.ToInt32(response.Value.TotalCount ?? 0),
            Facets = NormalizeFacets(response.Value.Facets),
            Results = response.Value.GetResults().Select(NormalizeSearchResult).ToList()
        };
    }

    public GeoLocation? GetZipLocation(string zipCode)
    {
        if (_zipClient is null)
        {
            return null;
        }

        var searchOptions = new SearchOptions
        {
            SearchMode = SearchMode.All,
            Size = 1
        };

        var result = _zipClient.Search<SearchDocument>(zipCode, searchOptions).Value.GetResults().FirstOrDefault();
        return result is null ? null : TryGetGeoLocation(result.Document, "geo_location");
    }

    public IReadOnlyList<string> Suggest(string searchText, bool fuzzy)
    {
        if (_jobsClient is null || string.IsNullOrWhiteSpace(searchText))
        {
            return Array.Empty<string>();
        }

        var suggestOptions = new SuggestOptions
        {
            UseFuzzyMatching = fuzzy,
            Size = 8
        };

        return _jobsClient.Suggest<SearchDocument>(searchText, "sg", suggestOptions)
            .Value
            .Results
            .Select(result => result.Text)
            .ToList();
    }

    public NYCJobLookup? LookUp(string id)
    {
        if (_jobsClient is null)
        {
            return new NYCJobLookup();
        }

        var response = _jobsClient.GetDocument<SearchDocument>(id);
        return new NYCJobLookup { Result = NormalizeDocument(response.Value) };
    }

    private static SearchResultItem NormalizeSearchResult(SearchResult<SearchDocument> result)
    {
        return new SearchResultItem
        {
            Document = NormalizeDocument(result.Document),
            Highlights = result.Highlights?.ToDictionary(pair => pair.Key, pair => string.Join(" ", pair.Value))
        };
    }

    private static Dictionary<string, IList<FacetValue>> NormalizeFacets(IDictionary<string, IList<FacetResult>>? facets)
    {
        if (facets is null)
        {
            return new Dictionary<string, IList<FacetValue>>();
        }

        return facets.ToDictionary(
            facet => facet.Key,
            facet => (IList<FacetValue>)facet.Value.Select(value => new FacetValue
            {
                Value = NormalizeScalarValue(value.Value),
                Count = value.Count
            }).ToList());
    }

    private static Dictionary<string, object?> NormalizeDocument(SearchDocument document)
    {
        var normalized = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in document)
        {
            normalized[pair.Key] = NormalizeFieldValue(pair.Key, pair.Value);
        }

        return normalized;
    }

    private static object? NormalizeFieldValue(string key, object? value)
    {
        if (string.Equals(key, "geo_location", StringComparison.OrdinalIgnoreCase))
        {
            return TryGetGeoLocation(value, key) ?? value;
        }

        return NormalizeScalarValue(value, key);
    }

    private static object? NormalizeScalarValue(object? value, string? key = null)
    {
        return value switch
        {
            null => null,
            DateTimeOffset dateTimeOffset => ToLegacyDate(dateTimeOffset),
            DateTime dateTime => ToLegacyDate(new DateTimeOffset(dateTime)),
            JsonElement jsonElement => NormalizeJsonElement(jsonElement, key),
            IEnumerable<object?> list when value is not string => list.Select(item => NormalizeScalarValue(item)).ToList(),
            string text when IsDateField(key) && DateTimeOffset.TryParse(text, out var parsed) => ToLegacyDate(parsed),
            _ => value
        };
    }

    private static object? NormalizeJsonElement(JsonElement element, string? key = null)
    {
        if (string.Equals(key, "geo_location", StringComparison.OrdinalIgnoreCase))
        {
            return TryGetGeoLocation(element, key);
        }

        return element.ValueKind switch
        {
            JsonValueKind.String when IsDateField(key) && DateTimeOffset.TryParse(element.GetString(), out var parsed) => ToLegacyDate(parsed),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt64(out var number) => number,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Array => element.EnumerateArray().Select(item => NormalizeJsonElement(item)).ToList(),
            JsonValueKind.Object => element.EnumerateObject().ToDictionary(property => property.Name, property => NormalizeJsonElement(property.Value, property.Name)),
            JsonValueKind.Null => null,
            _ => element.ToString()
        };
    }

    private static GeoLocation? TryGetGeoLocation(object? value, string? key)
    {
        if (!string.Equals(key, "geo_location", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (value is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            if (element.TryGetProperty("coordinates", out var coordinates) && coordinates.GetArrayLength() >= 2)
            {
                return new GeoLocation
                {
                    Longitude = coordinates[0].GetDouble(),
                    Latitude = coordinates[1].GetDouble()
                };
            }
        }

        if (value is IDictionary<string, object?> dictionary && dictionary.TryGetValue("coordinates", out var rawCoordinates) && rawCoordinates is IEnumerable<object?> coordinatesList)
        {
            var coordinates = coordinatesList.OfType<IConvertible>().Select(item => Convert.ToDouble(item, CultureInfo.InvariantCulture)).ToArray();
            if (coordinates.Length >= 2)
            {
                return new GeoLocation { Longitude = coordinates[0], Latitude = coordinates[1] };
            }
        }

        var valueType = value?.GetType();
        if (valueType is not null)
        {
            var latitudeProperty = valueType.GetProperty("Latitude");
            var longitudeProperty = valueType.GetProperty("Longitude");
            if (latitudeProperty?.GetValue(value) is IConvertible latitude && longitudeProperty?.GetValue(value) is IConvertible longitude)
            {
                return new GeoLocation
                {
                    Latitude = Convert.ToDouble(latitude, CultureInfo.InvariantCulture),
                    Longitude = Convert.ToDouble(longitude, CultureInfo.InvariantCulture)
                };
            }
        }

        return null;
    }

    private static bool IsDateField(string? key)
        => key is "posting_date" or "post_until" or "posting_updated" or "process_date";

    private static string ToLegacyDate(DateTimeOffset value)
        => $"/Date({value.ToUnixTimeMilliseconds()})/";

    private static void AddList(ICollection<string> destination, IEnumerable<string> source)
    {
        foreach (var item in source)
        {
            destination.Add(item);
        }
    }
}
