namespace NYCJobsWeb.Models;

public class NYCJob
{
    public IDictionary<string, IList<FacetValue>> Facets { get; set; } = new Dictionary<string, IList<FacetValue>>();

    public IList<SearchResultItem> Results { get; set; } = new List<SearchResultItem>();

    public int Count { get; set; }
}

public class NYCJobLookup
{
    public IDictionary<string, object?>? Result { get; set; }
}

public class SearchResultItem
{
    public IDictionary<string, object?> Document { get; set; } = new Dictionary<string, object?>();

    public IDictionary<string, string>? Highlights { get; set; }
}

public class FacetValue
{
    public object? Value { get; set; }

    public long? Count { get; set; }
}

public class GeoLocation
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
