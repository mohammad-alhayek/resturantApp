using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class AdminBaseController : Controller
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly HttpClient client;

    public AdminBaseController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
       client = _httpClientFactory.CreateClient("ApiClient");
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var userRole = HttpContext.Session.GetString("UserRole");

        if (string.IsNullOrEmpty(userRole) || userRole != "Staff")
        {
            filterContext.Result = RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        base.OnActionExecuting(filterContext);
    }
}