using Microsoft.AspNetCore.Mvc;
using NYCJobsWeb.Models;

namespace NYCJobsWeb.Controllers;

public class HomeController : Controller
{
    private readonly JobsSearch _jobsSearch;

    public HomeController(JobsSearch jobsSearch)
    {
        _jobsSearch = jobsSearch;
    }

    public IActionResult Index() => View();

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
        int currentPage = 0,
        int zipCode = 10001,
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
            var location = _jobsSearch.GetZipLocation(zipCode.ToString());
            if (location is not null)
            {
                maxDistanceLat = location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                maxDistanceLon = location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        var response = _jobsSearch.Search(q, businessTitleFacet, postingTypeFacet, salaryRangeFacet, sortType, lat, lon, currentPage, maxDistance, maxDistanceLat, maxDistanceLon);

        return Json(response);
    }

    [HttpGet]
    public IActionResult Suggest(string term, bool fuzzy = true)
    {
        var uniqueItems = _jobsSearch.Suggest(term, fuzzy).Distinct().ToList();
        return Json(uniqueItems);
    }

    [HttpPost]
    public IActionResult LookUp(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        var response = _jobsSearch.LookUp(id);
        return Json(response ?? new NYCJobLookup());
    }
}
