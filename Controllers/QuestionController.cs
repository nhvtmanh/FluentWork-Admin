using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Question";
            GetTypeTopicLevelDropdown();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? type, string? vocabularyTopic, string? grammarTopic, string? level)
        {
            var res = await _questionService.GetList(type, vocabularyTopic, grammarTopic, level);
            return Json(res);
        }

        public async Task<IActionResult> P_AddOrEdit(int id)
        {
            GetTypeTopicLevelDropdown();

            if (id > 0) //Show edit modal
            {
                var res = await _questionService.GetById(id);

                if (res.StatusCode == StatusCodes.Status200OK)
                {
                    var question = res.Data;
                    return PartialView(question);
                }
            }

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> P_AddOrEdit(M_Question model)
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
                var resUpdate = await _questionService.Update(model);

                if (resUpdate.StatusCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(resUpdate);
                }

                return Json(resUpdate);
            }

            var resCreate = await _questionService.Create(model);

            if (resCreate.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(resCreate);
            }

            return Json(resCreate);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _questionService.Delete(id);

            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }
    }
}
