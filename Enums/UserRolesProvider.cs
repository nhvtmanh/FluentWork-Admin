using Microsoft.AspNetCore.Mvc.Rendering;

namespace FluentWork_Admin.Enums
{
    public static class UserRolesProvider
    {
        public static List<SelectListItem> GetRoles() => new()
        {
            new SelectListItem { Value = "Admin", Text = "Admin" },
            new SelectListItem { Value = "Learner", Text = "Learner" },
            new SelectListItem { Value = "Instructor", Text = "Instructor" }
        };
    }
}
