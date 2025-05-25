using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FluentWork_Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(M_Account_Login account)
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

            var res = await _authService.Login(account);

            if (res.StatusCode == StatusCodes.Status201Created)
            {
                return Ok(res);
            }
            else if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }
            else if (res.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(res);
            }
            else if (res.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(res);
            }
            else
            {
                return Json(res);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var res = _authService.Logout();
            return Ok(res);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(M_Account_ForgotPassword account)
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

            var res = await _authService.ForgotPassword(account);

            if (res.StatusCode == StatusCodes.Status201Created)
            {
                return Ok(res);
            }
            else if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }
            else if (res.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(res);
            }
            else
            {
                return Json(res);
            }
        }
    }
}
