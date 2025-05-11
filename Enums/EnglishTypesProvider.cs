using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluentWork_Admin.Enums
{
    public static class EnglishTypesProvider
    {
        public static List<SelectListItem> GetTopics() => new()
        {
            new SelectListItem { Value = "Vocabulary", Text = "Vocabulary" },
            new SelectListItem { Value = "Grammar", Text = "Grammar" }
        };
    }
}
