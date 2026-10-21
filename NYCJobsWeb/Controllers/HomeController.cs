using Microsoft.AspNetCore.Mvc;
using NYCJobsWeb.Models;

namespace NYCJobsWeb.Controllers;

public class HomeController(JobsSearch jobsSearch) : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult JobDetails() => View();

    [HttpPost]
    public IActionResult Search(
        string q = "",
        string businessTitleFacet = "",
        string postingTypeFacet = "",
        string salaryRangeFacet = "",
        string sortType = "",
        double lat = 40.736224,
        double lon = -73.99251,
        int currentPage = 1,
        int zipCode = 10001,
        int maxDistance = 0)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            q = "*";
        }

        var maxDistanceLat = string.Empty;
        var maxDistanceLon = string.Empty;
        if (maxDistance > 0)
        {
            var zipResponse = jobsSearch.SearchZip(zipCode.ToString(System.Globalization.CultureInfo.InvariantCulture));
            var zipResult = zipResponse.GetResults().FirstOrDefault();
            if (zipResult?.Document.TryGetValue("geo_location", out var location) == true &&
                location is Microsoft.Spatial.GeographyPoint point)
            {
                maxDistanceLat = point.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                maxDistanceLon = point.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        var response = jobsSearch.Search(
            q, businessTitleFacet, postingTypeFacet, salaryRangeFacet,
            sortType, lat, lon, currentPage, maxDistance, maxDistanceLat, maxDistanceLon);

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
        var suggestions = jobsSearch.Suggest(term, fuzzy).Results
            .Select(result => result.Text)
            .Distinct()
            .ToList();

        return Json(suggestions);
    }

    [HttpPost]
    public IActionResult LookUp(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        var result = jobsSearch.LookUp(id);
        return Json(new NYCJobLookup { Result = result });
    }
}
