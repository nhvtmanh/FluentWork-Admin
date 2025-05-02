using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluentWork_Admin.Enums
{
    public static class VocabularyTopicsProvider
    {
        public static List<SelectListItem> GetTopics() => new()
        {
            new SelectListItem { Value = "Information Technology", Text = "Information Technology" },
            new SelectListItem { Value = "Business", Text = "Business" },
            new SelectListItem { Value = "Finance", Text = "Finance" }
        };
    }
}
