using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
