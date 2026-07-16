using Microsoft.AspNetCore.Mvc;
using NYCJobsWeb.Models;
using System.Globalization;
using System.Text.Json;

namespace NYCJobsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobsSearch _jobsSearch;

        public HomeController(JobsSearch jobsSearch)
        {
            _jobsSearch = jobsSearch;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JobDetails()
        {
            return View();
        }

        public IActionResult Search(string q = "", string businessTitleFacet = "", string postingTypeFacet = "", string salaryRangeFacet = "",
            string sortType = "", double lat = 40.736224, double lon = -73.99251, int currentPage = 0, int zipCode = 10001,
            int maxDistance = 0)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                q = "*";
            }

            string maxDistanceLat = string.Empty;
            string maxDistanceLon = string.Empty;

            if (maxDistance > 0)
            {
                var zipReponse = _jobsSearch.SearchZip(zipCode.ToString());
                if (zipReponse is not null)
                {
                    foreach (var result in zipReponse.GetResults())
                    {
                        var doc = result.Document;
                        if (doc.TryGetValue("geo_location", out var geoLocation))
                        {
                            TryReadGeoCoordinates(geoLocation, out maxDistanceLat, out maxDistanceLon);
                        }
                    }
                }
            }

            var response = _jobsSearch.Search(q, businessTitleFacet, postingTypeFacet, salaryRangeFacet, sortType, lat, lon, currentPage, maxDistance, maxDistanceLat, maxDistanceLon);
            if (response is null)
            {
                return Json(new NYCJob { Results = new List<Azure.Search.Documents.Models.SearchResult<Azure.Search.Documents.Models.SearchDocument>>(), Facets = new Dictionary<string, IList<Azure.Search.Documents.Models.FacetResult>>(), Count = 0 });
            }

            return Json(new NYCJob
            {
                Results = response.GetResults().ToList(),
                Facets = response.Facets,
                Count = Convert.ToInt32(response.TotalCount)
            });
        }

        [HttpGet]
        public IActionResult Suggest(string term, bool fuzzy = true)
        {
            var response = _jobsSearch.Suggest(term, fuzzy);
            List<string> suggestions = new List<string>();

            if (response is not null)
            {
                foreach (var result in response.Results)
                {
                    suggestions.Add(result.Text);
                }
            }

            return Json(suggestions.Distinct().ToList());
        }

        public IActionResult LookUp(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Job ID is required.");
            }

            var response = _jobsSearch.LookUp(id);
            return Json(new NYCJobLookup { Result = response });
        }

        private static void TryReadGeoCoordinates(object geoLocation, out string latitude, out string longitude)
        {
            latitude = string.Empty;
            longitude = string.Empty;

            if (geoLocation is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("coordinates", out var coordinates) && coordinates.ValueKind == JsonValueKind.Array && coordinates.GetArrayLength() >= 2)
                {
                    longitude = coordinates[0].GetDouble().ToString(CultureInfo.InvariantCulture);
                    latitude = coordinates[1].GetDouble().ToString(CultureInfo.InvariantCulture);
                    return;
                }

                if (element.ValueKind == JsonValueKind.Object
                    && element.TryGetProperty("Latitude", out var latProperty)
                    && element.TryGetProperty("Longitude", out var lonProperty))
                {
                    latitude = latProperty.GetDouble().ToString(CultureInfo.InvariantCulture);
                    longitude = lonProperty.GetDouble().ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
