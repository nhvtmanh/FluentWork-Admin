using FluentWork_Admin.Extensions;
using FluentWork_Admin.Middlewares;
using FluentWork_Admin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiClient", httpClient =>
{
    string apiUrl = builder.Configuration.GetValue<string>("ApiConfig:ApiUrl")!;
    httpClient.BaseAddress = new Uri(apiUrl);
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseMiddleware<JwtRoleMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "Admin login",
    pattern: "admin/login",
    defaults: new { controller = "Account", action = "Login" });

app.MapControllerRoute(
    name: "Admin forgot password",
    pattern: "admin/forgot-password",
    defaults: new { controller = "Account", action = "ForgotPassword" });

app.MapControllerRoute(
    name: "Dashboard",
    pattern: "admin/dashboard",
    defaults: new { controller = "Dashboard", action = "Index" });

app.MapControllerRoute(
    name: "Manage lessons",
    pattern: "admin/manage-lessons",
    defaults: new { controller = "Lesson", action = "Index" });

app.MapControllerRoute(
    name: "Manage questions",
    pattern: "admin/manage-questions",
    defaults: new { controller = "Question", action = "Index" });

app.MapControllerRoute(
    name: "Manage tests",
    pattern: "admin/manage-tests",
    defaults: new { controller = "Test", action = "Index" });

app.MapControllerRoute(
    name: "Manage flashcards",
    pattern: "admin/manage-flashcards",
    defaults: new { controller = "Flashcard", action = "Index" });

app.MapControllerRoute(
    name: "Manage users",
    pattern: "admin/manage-users",
    defaults: new { controller = "User", action = "Index" });

app.Run();
