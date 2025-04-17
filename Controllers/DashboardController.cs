using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
