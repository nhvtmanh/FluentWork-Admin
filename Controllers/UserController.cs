using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "User";
            GetUserRoleDropdown();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? role)
        {
            var res = await _userService.GetList(role);
            return Json(res);
        }

        public async Task<IActionResult> P_AddOrEdit(int id)
        {
            GetUserRoleDropdown();

            if (id > 0) //Show edit modal
            {
                var res = await _userService.GetById(id);

                if (res.StatusCode == StatusCodes.Status200OK)
                {
                    var user = res.Data;
                    return PartialView(user);
                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> P_AddOrEdit(M_User model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value!.Errors.Any())
                    .SelectMany(x => x.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { message = errors });
            }

            var res = await _userService.Update(model);

            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _userService.Delete(id);

            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }
    }
}
