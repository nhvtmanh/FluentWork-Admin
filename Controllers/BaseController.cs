using FluentWork_Admin.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace FluentWork_Admin.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        protected void GetTypeTopicLevelDropdown()
        {
            ViewBag.EnglishTypes = EnglishTypesProvider.GetTopics();
            ViewBag.VocabularyTopics = VocabularyTopicsProvider.GetTopics();
            ViewBag.GrammarTopics = GrammarTopicsProvider.GetTopics();
            ViewBag.EnglishLevels = EnglishLevelsProvider.GetLevels();
        }

        protected void GetUserRoleDropdown()
        {
            ViewBag.UserRoles = UserRolesProvider.GetRoles();
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
    }
}
