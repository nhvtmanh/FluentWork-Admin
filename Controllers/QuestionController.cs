using FluentWork_Admin.Enums;
using FluentWork_Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace FluentWork_Admin.Controllers
{
    public class QuestionController : Controller
    {
        List<M_Question> listQuestions;

        public QuestionController()
        {
            SeedData();
        }
        private void SeedData()
        {
            listQuestions = new List<M_Question>
            {
                new M_Question
                {
                    Id = 1,
                    Type = "Vocabulary",
                    VocabularyTopic = "IT",
                    GrammarTopic = null,
                    Level = "Beginner",
                    QuestionText = "What does 'HTML' stand for?",
                    Explanation = "HTML stands for HyperText Markup Language.",
                },
                new M_Question
                {
                    Id = 2,
                    Type = "Grammar",
                    VocabularyTopic = null,
                    GrammarTopic = "Tense",
                    Level = "Intermediate",
                    QuestionText = "What is the past tense of 'go'?",
                    Explanation = "The past tense of 'go' is 'went'.",
                },
                new M_Question
                {
                    Id = 3,
                    Type = "Vocabulary",
                    VocabularyTopic = "Business",
                    GrammarTopic = null,
                    Level = "Advanced",
                    QuestionText = "What does 'ROI' stand for?",
                    Explanation = "ROI stands for Return on Investment.",
                },
            };
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Question";
            ViewBag.QuestionTypes = QuestionTypesProvider.GetTopics();
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetList()
        {
            var res = new
            {
                data = listQuestions
            };
            return Json(res);
        }
        public IActionResult P_AddOrEdit()
        {
            ViewBag.GrammarTopics = GrammarTopicsProvider.GetTopics();
            ViewBag.VocabularyTopics = VocabularyTopicsProvider.GetTopics();
            return PartialView();
        }
        [HttpPost]
        public IActionResult P_AddOrEdit(EM_Question model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = 400, message = "Validation failed." });
            }

            // Save logic here...

            return Json(new { status = 200, message = "Question saved successfully." });
        }
    }
}
