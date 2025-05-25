using Microsoft.IdentityModel.JsonWebTokens;

namespace FluentWork_Admin.Middlewares
{
    public class JwtRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            //Skip auth check for login, forgot password
            var excludedPaths = new[]
            {
                "/admin/login",
                "/account/login",
                "/admin/forgot-password",
                "/account/forgotpassword"
            };

            if (excludedPaths.Any(p => path!.StartsWith(p)))
            {
                await _next(context);
                return;
            }

            var token = context.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/admin/login");
                return;
            }

            var handler = new JsonWebTokenHandler();
            var jwt = handler.ReadJsonWebToken(token);
            var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (role != "Admin")
            {
                context.Response.Redirect("/admin/login");
                return;
            }

            await _next(context);
        }
    }
}
