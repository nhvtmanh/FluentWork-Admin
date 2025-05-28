using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class FlashcardController : BaseController<FlashcardController>
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Flashcard";
            GetTypeTopicLevelDropdown();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? topic)
        {
            var res = await _flashcardService.GetList(topic);
            return Json(res);
        }

        public async Task<IActionResult> P_AddOrEdit(int id)
        {
            GetTypeTopicLevelDropdown();

            if (id > 0) //Show edit modal
            {
                var res = await _flashcardService.GetById(id);

                if (res.StatusCode == StatusCodes.Status200OK)
                {
                    var flashcard = res.Data;
                    return PartialView(flashcard);
                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> P_AddOrEdit(M_Flashcard model)
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

            if (model.Id > 0) //Update
            {
                var resUpdate = await _flashcardService.Update(model);

                if (resUpdate.StatusCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(resUpdate);
                }

                return Json(resUpdate);
            }

            var resCreate = await _flashcardService.Create(model);

            if (resCreate.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(resCreate);
            }

            return Json(resCreate);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _flashcardService.Delete(id);

            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }
    }
}
