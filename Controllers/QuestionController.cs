using FluentWork_Admin.Enums;
using FluentWork_Admin.Models;
using FluentWork_Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FluentWork_Admin.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Question";
            ViewBag.QuestionTypes = EnglishTypesProvider.GetTopics();
            ViewBag.QuestionLevels = EnglishLevelsProvider.GetLevels();
            return View();
        }

        [HttpGet]
        public IActionResult GetTopicsByType(string type)
        {
            List<SelectListItem> topics = new List<SelectListItem>();

            if (type == EnglishType.VOCABULARY)
            {
                topics = VocabularyTopicsProvider.GetTopics();
            }
            else if (type == EnglishType.GRAMMAR)
            {
                topics = GrammarTopicsProvider.GetTopics();
            }

            return Json(topics);
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? type, string? vocabularyTopic, string? grammarTopic, string? level)
        {
            var res = await _questionService.GetList(type, vocabularyTopic, grammarTopic, level);
            return Json(res);
        }

        public async Task<IActionResult> P_AddOrEdit(int id)
        {
            ViewBag.GrammarTopics = GrammarTopicsProvider.GetTopics();
            ViewBag.VocabularyTopics = VocabularyTopicsProvider.GetTopics();
            ViewBag.EnglishLevels = EnglishLevelsProvider.GetLevels();

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
    }
}
