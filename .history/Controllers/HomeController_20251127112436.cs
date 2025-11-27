using Microsoft.AspNetCore.Mvc;

namespace HabilitationApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}