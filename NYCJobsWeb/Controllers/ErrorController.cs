using Microsoft.AspNetCore.Mvc;

namespace NYCJobsWeb.Controllers;

public class ErrorController : Controller
{
    [Route("Home/Error")]
    public IActionResult Error() => Problem("An unexpected error occurred.");
}
