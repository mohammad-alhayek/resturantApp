using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using RestaurantStore.Shared.UserDto;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace RestaurantStore.Web.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginModel)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var response = await client.PostAsJsonAsync("api/User/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
               
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

              
                HttpContext.Session.SetString("AdminUser", result.Username);
                HttpContext.Session.SetString("UserRole", result.Role);

               
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                

              
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

       
       

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}