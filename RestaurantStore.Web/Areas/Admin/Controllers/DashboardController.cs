using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Shared.Dashboard;
using System.Net.Http.Json; // مهم جداً لاستخدام GetFromJsonAsync

namespace RestaurantStore.Web.Controllers.Admin
{
    [Area("Admin")]
    public class DashboardController : AdminBaseController
    {
       
            public DashboardController(IHttpClientFactory httpClientFactory)
                : base(httpClientFactory)
            {
            }

        

        // GET: Admin/Dashboard/Index
        public async Task<IActionResult> Index()
        {
            try
            {
               

                var response = await client.GetFromJsonAsync<DashboardDto>("api/Dashboard");

                if (response == null)
                {
                    return View(new DashboardDto()); 
                }

                return View(response);
            }
            catch (Exception ex)
            {
               
                ViewBag.Error = "no data avilable now";
                return View(new DashboardDto());
            }
        }
    }
}