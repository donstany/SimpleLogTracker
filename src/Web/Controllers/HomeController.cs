using Microsoft.AspNetCore.Mvc;

namespace SimpleLogTracker.Web.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
