using FluentWork_Admin.Enums;
using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class LessonController : BaseController<LessonController>
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _lessonService = lessonService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Lesson";
            GetTypeTopicLevelDropdown();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? type, string? vocabularyTopic, string? grammarTopic, string? level)
        {
            var res = await _lessonService.GetList(type, vocabularyTopic, grammarTopic, level);
            return Json(res);
        }

        public async Task<IActionResult> P_AddOrEdit(int id)
        {
            GetTypeTopicLevelDropdown();

            if (id > 0) //Show edit modal
            {
                var res = await _lessonService.GetById(id);

                if (res.StatusCode == StatusCodes.Status200OK)
                {
                    var lesson = res.Data;
                    return PartialView(lesson);
                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> P_AddOrEdit(M_Lesson model)
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
                var resUpdate = await _lessonService.Update(model);

                if (resUpdate.StatusCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(resUpdate);
                }

                return Json(resUpdate);
            }

            var resCreate = await _lessonService.Create(model);

            if (resCreate.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(resCreate);
            }

            return Json(resCreate);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _lessonService.Delete(id);

            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }
    }
}
