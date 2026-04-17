using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantStore.Shared.CategoryDto;
using RestaurantStore.Shared.ProductDto;
using RestaurantStore.Web.Areas.Admin.sercices;

namespace RestaurantStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : AdminBaseController
    {
        private readonly ProductService _productService;

        public ProductController(IHttpClientFactory httpClientFactory, ProductService productService)
               : base(httpClientFactory)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                // نجيب المنتجات مع أسماء الكاتيجوري من الـ Service مباشرة
                var productsWithCategory = await _productService.GetProductsWithCategoryAsync();

                // لو ما فيه بيانات نرسل قائمة فاضية
                if (productsWithCategory == null || !productsWithCategory.Any())
                {
                    ViewBag.Error = "No data available now";
                    return View(new List<(ResponseProductDto Product, string CategoryName)>());
                }

                return View(productsWithCategory);
            }
            catch (Exception)
            {
                ViewBag.Error = "No data available now";
                return View(new List<(ResponseProductDto Product, string CategoryName)>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await client.DeleteAsync($"api/Product/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            var product = await client.GetFromJsonAsync<ResponseProductDto>($"api/Product/{id}");

            if (product == null)
                return View(new ResponseProductDto());

            // جيب كل الكاتيجوريز
            var categories = await client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/Category");

            // حولهم لـ SelectList
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> EditSave(ResponseProductDto model, IFormFile? image)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(model.Id.ToString()), "Id");
            form.Add(new StringContent(model.Name ?? ""), "Name");
            form.Add(new StringContent(model.Description ?? ""), "Description");
            form.Add(new StringContent(model.Price.ToString()), "Price");
            form.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(model.ImageUrl ?? ""), "ImageUrl"); // ✅ الصورة القديمة

            if (image != null && image.Length > 0)
            {
                var stream = image.OpenReadStream();
                form.Add(new StreamContent(stream), "image", image.FileName);
            }

            var response = await client.PutAsync($"api/Product/{model.Id}", form);
            Console.WriteLine($">>> Status: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($">>> Response: {content}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            var categories = await client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/Category");
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View("Edit", model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // جيب كل الكاتيجوريز
            var categories = await client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/Category");

            // هون ما في selected value
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(new ResponseProductDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ResponseProductDto dto, IFormFile? image)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(dto.Name), "Name");
            form.Add(new StringContent(dto.Description ?? ""), "Description");
            form.Add(new StringContent(dto.Price.ToString()), "Price");
            form.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");

            if (image != null && image.Length > 0)
            {
                var stream = image.OpenReadStream();
                form.Add(new StreamContent(stream), "ImageUrl", image.FileName);
            }

            var response = await client.PostAsync("api/Product", form);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = await response.Content.ReadAsStringAsync();
            return View(dto);
        }
    }
}