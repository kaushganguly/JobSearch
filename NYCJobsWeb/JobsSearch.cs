using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace NYCJobsWeb
{
    public class JobsSearch
    {
        private readonly SearchClient _indexClient;
        private readonly SearchClient _indexZipClient;

        private const string IndexName = "nycjobs";
        private const string IndexZipCodes = "zipcodes";

        public JobsSearch(IConfiguration configuration)
        {
            var endpoint = configuration["Searchendpoint"];
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                var searchServiceName = configuration["SearchServiceName"];
                if (!string.IsNullOrWhiteSpace(searchServiceName))
                {
                    endpoint = $"https://{searchServiceName}.search.windows.net";
                }
            }

            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var endpointUri))
            {
                return;
            }

            var apiKey = configuration["SearchServiceApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return;
            }

            var searchIndexClient = new SearchIndexClient(endpointUri, new AzureKeyCredential(apiKey));
            _indexClient = searchIndexClient.GetSearchClient(IndexName);
            _indexZipClient = searchIndexClient.GetSearchClient(IndexZipCodes);
        }

        public SearchResults<SearchDocument> Search(string searchText, string businessTitleFacet, string postingTypeFacet, string salaryRangeFacet,
            string sortType, double lat, double lon, int currentPage, int maxDistance, string maxDistanceLat, string maxDistanceLon)
        {
            try
            {
                if (_indexClient is null)
                {
                    return null;
                }

                SearchOptions sp = new SearchOptions
                {
                    SearchMode = SearchMode.Any,
                    Size = 10,
                    Skip = Math.Max(currentPage - 1, 0),
                    IncludeTotalCount = true,
                    HighlightPreTag = "<b>",
                    HighlightPostTag = "</b>"
                };

                List<string> select = new List<string> { "id", "agency", "posting_type", "num_of_positions", "business_title",
                        "salary_range_from", "salary_range_to", "salary_frequency", "work_location", "job_description",
                        "posting_date", "geo_location", "tags" };
                List<string> facets = new List<string> { "business_title", "posting_type", "level", "salary_range_from,interval:50000" };
                AddList(sp.Select, select);
                AddList(sp.Facets, facets);
                sp.HighlightFields.Add("job_description");

                if (sortType == "featured")
                {
                    sp.ScoringProfile = "jobsScoringFeatured";
                    sp.ScoringParameters.Add("featuredParam--featured");
                    sp.ScoringParameters.Add($"mapCenterParam--geography'POINT({lon.ToString(CultureInfo.InvariantCulture)} {lat.ToString(CultureInfo.InvariantCulture)})'");
                }
                else if (sortType == "salaryDesc")
                {
                    sp.OrderBy.Add("salary_range_from desc");
                }
                else if (sortType == "salaryIncr")
                {
                    sp.OrderBy.Add("salary_range_from");
                }
                else if (sortType == "mostRecent")
                {
                    sp.OrderBy.Add("posting_date desc");
                }

                string filter = null;
                if (!string.IsNullOrWhiteSpace(businessTitleFacet))
                {
                    filter = "business_title eq '" + businessTitleFacet + "'";
                }

                if (!string.IsNullOrWhiteSpace(postingTypeFacet))
                {
                    if (filter is not null)
                    {
                        filter += " and ";
                    }
                    filter += "posting_type eq '" + postingTypeFacet + "'";
                }

                if (!string.IsNullOrWhiteSpace(salaryRangeFacet))
                {
                    if (filter is not null)
                    {
                        filter += " and ";
                    }
                    filter += "salary_range_from ge " + salaryRangeFacet + " and salary_range_from lt " + (Convert.ToInt32(salaryRangeFacet) + 50000).ToString();
                }

                if (maxDistance > 0 && !string.IsNullOrWhiteSpace(maxDistanceLon) && !string.IsNullOrWhiteSpace(maxDistanceLat))
                {
                    if (filter is not null)
                    {
                        filter += " and ";
                    }
                    filter += "geo.distance(geo_location, geography'POINT(" + maxDistanceLon + " " + maxDistanceLat + ")') le " + maxDistance;
                }

                sp.Filter = filter;

                return _indexClient.Search<SearchDocument>(searchText, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message);
            }
            return null;
        }

        public SearchResults<SearchDocument> SearchZip(string zipCode)
        {
            try
            {
                if (_indexZipClient is null)
                {
                    return null;
                }

                SearchOptions sp = new SearchOptions
                {
                    SearchMode = SearchMode.All,
                    Size = 1
                };
                return _indexZipClient.Search<SearchDocument>(zipCode, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message);
            }
            return null;
        }

        public SuggestResults<SearchDocument> Suggest(string searchText, bool fuzzy)
        {
            try
            {
                if (_indexClient is null)
                {
                    return null;
                }

                SuggestOptions sp = new SuggestOptions
                {
                    UseFuzzyMatching = fuzzy,
                    Size = 8
                };

                return _indexClient.Suggest<SearchDocument>(searchText, "sg", sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message);
            }
            return null;
        }

        public SearchDocument LookUp(string id)
        {
            try
            {
                if (_indexClient is null)
                {
                    return null;
                }

                return _indexClient.GetDocument<SearchDocument>(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message);
            }
            return null;
        }

        public void AddList(IList<string> list1, List<string> list2)
        {
            foreach (string element in list2)
            {
                list1.Add(element);
            }
        }
    }
}
