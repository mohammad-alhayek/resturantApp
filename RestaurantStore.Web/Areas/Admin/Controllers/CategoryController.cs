using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Shared.CategoryDto;
using RestaurantStore.Shared.Dashboard;
using RestaurantStore.Shared.UserDto;

namespace RestaurantStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : AdminBaseController
    {
        public CategoryController(IHttpClientFactory httpClientFactory)
               : base(httpClientFactory)
        {
        }

        public async Task<IActionResult> Index()
        {
            try
            {


                var response = await client.GetFromJsonAsync< List<ResponseCategoryDto>>("api/Category");

                if (response == null)
                {
                    return View(new List< ResponseCategoryDto>()); 
                }

                return View(response);
            }
            catch (Exception ex)
            {
              
                ViewBag.Error = "no data avilable now";
                return View(new List< ResponseCategoryDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await client.DeleteAsync($"api/Category/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            var response = await client.GetFromJsonAsync<ResponseCategoryDto>($"api/Category/{id}");
            if (response == null)
                return View(new ResponseCategoryDto());
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditSave(ResponseCategoryDto model)
        {

            var response = await client.PutAsJsonAsync($"api/Category/{model.Id}", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ResponseCategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResponseCategoryDto dto)
        {
            var response = await client.PostAsJsonAsync("api/Category", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View(dto);
        }
    }
}
