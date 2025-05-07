using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluentWork_Admin.Enums
{
    public class EnglishLevelsProvider
    {
        public static List<SelectListItem> GetLevels() => new()
        {
            new SelectListItem { Value = "Beginner", Text = "Beginner" },
            new SelectListItem { Value = "Intermediate", Text = "Intermediate" },
            new SelectListItem { Value = "Advanced", Text = "Advanced" }
        };
    }
}
