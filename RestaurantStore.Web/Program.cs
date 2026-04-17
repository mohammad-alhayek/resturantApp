using Microsoft.AspNetCore.Identity;
using RestaurantStore.Web.Areas.Admin.sercices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7269/");
});

// ✅ أضف هذا
builder.Services.AddHttpClient<ProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7269/");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddSession();

var app = builder.Build();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Admin/Account/Login");
    return Task.CompletedTask;
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();