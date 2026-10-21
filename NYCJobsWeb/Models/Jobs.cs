using Azure.Search.Documents.Models;

namespace NYCJobsWeb.Models;

public class NYCJob
{
    public IDictionary<string, IList<FacetResult>> Facets { get; set; } = new Dictionary<string, IList<FacetResult>>();
    public IList<SearchResult<SearchDocument>> Results { get; set; } = new List<SearchResult<SearchDocument>>();
    public int? Count { get; set; }
}

public class NYCJobLookup
{
    public SearchDocument? Result { get; set; }
}
