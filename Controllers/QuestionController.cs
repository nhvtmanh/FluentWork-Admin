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
                    Topic = "IT",
                    Level = "Beginner",
                    QuestionText = "What does CPU stand for?"
                },
                new M_Question
                {
                    Id = 2,
                    Type = "Grammar",
                    Topic = "Tenses",
                    Level = "Intermediate",
                    QuestionText = "Choose the correct past tense form of the verb."
                },
                new M_Question
                {
                    Id = 3,
                    Type = "Vocabulary",
                    Topic = "Finance",
                    Level = "Advanced",
                    QuestionText = "What is the definition of 'liquidity' in finance?"
                }
            };
        }
        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Question";
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
        public IActionResult P_Add()
        {
            return PartialView();
        }
    }
}
