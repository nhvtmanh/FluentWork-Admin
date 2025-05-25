using FluentWork_Admin.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FluentWork_Admin.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(token);
                var username = jwt.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                ViewBag.Username = username;
            }

            base.OnActionExecuting(context);
        }

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
