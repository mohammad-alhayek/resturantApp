using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Shared.Dashboard;
using RestaurantStore.Shared.UserDto;

namespace RestaurantStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : AdminBaseController
    {

        public UsersController(IHttpClientFactory httpClientFactory)
       : base(httpClientFactory)
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {


                
                var response = await client.GetFromJsonAsync  < List <UserResponseDto>>("api/User/all-users");

                if (response == null)
                {
                    return View(new List<UserResponseDto>()); // إرجاع كائن فارغ لتجنب الـ Null في الـ View
                }

                return View(response);
            }
            catch (Exception ex)
            {
                // يمكنك إضافة سجل للأخطاء هنا
                ViewBag.Error = "no data avilable now";
                return View(new List< UserResponseDto>());
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var response = await client.GetFromJsonAsync<UserResponseDto>($"api/User/{id}");
            if (response == null)
                return View(new UserResponseDto());
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditSave(UserResponseDto model)
        {
      
            var response = await client.PutAsJsonAsync($"api/User/{model.Id}", model);
       
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await client.DeleteAsync($"api/User/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RegisterDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDto dto)
        {
            var response = await client.PostAsJsonAsync("api/User/register", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View(dto);
        }
    }
}
