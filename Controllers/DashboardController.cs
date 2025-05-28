using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class DashboardController : BaseController<DashboardController>
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Dashboard";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var res = await _dashboardService.GetSummary();

            if (res.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(res);
            }
            else
            {
                return Json(res);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLearnersDaily()
        {
            var res = await _dashboardService.GetLearnersDaily();

            if (res.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(res);
            }
            else
            {
                return Json(res);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var res = await _dashboardService.GetRoles();

            if (res.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(res);
            }
            else
            {
                return Json(res);
            }
        }
    }
}
