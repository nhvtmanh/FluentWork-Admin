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
            ViewBag.QuestionTypes = QuestionTypesProvider.GetTopics();
            ViewBag.QuestionLevels = EnglishLevelsProvider.GetLevels();
            return View();
        }

        [HttpGet]
        public IActionResult GetTopicsByType(string type)
        {
            List<SelectListItem> topics = new List<SelectListItem>();

            if (type == QuestionType.VOCABULARY)
            {
                topics = VocabularyTopicsProvider.GetTopics();
            }
            else if (type == QuestionType.GRAMMAR)
            {
                topics = GrammarTopicsProvider.GetTopics();
            }

            return Json(topics);
        }

        [HttpGet]
        public async Task<JsonResult> GetList(string? topic, string? vocabularyTopic, string? grammarTopic, string? level)
        {
            var res = await _questionService.GetList(topic, vocabularyTopic, grammarTopic, level);
            return Json(res);
        }

        public IActionResult P_AddOrEdit()
        {
            ViewBag.GrammarTopics = GrammarTopicsProvider.GetTopics();
            ViewBag.VocabularyTopics = VocabularyTopicsProvider.GetTopics();
            ViewBag.EnglishLevels = EnglishLevelsProvider.GetLevels();
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

            var res = await _questionService.Create(model);
            
            if (res.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(res);
            }

            return Json(res);
        }
    }
}
