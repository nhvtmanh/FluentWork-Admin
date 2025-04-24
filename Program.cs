var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    name: "Admin register",
    pattern: "admin/register",
    defaults: new { controller = "Account", action = "Register" });

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
