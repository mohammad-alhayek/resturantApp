using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Shared.CategoryDto;
using RestaurantStore.Shared.ProductDto;
namespace RestaurantStore.Web.Areas.Admin.sercices;

[Area("Admin")]
public class ProductService
{

    private readonly HttpClient _client;

    public ProductService(HttpClient client)
    {
        _client = client;
    }

    // هاي function تجمع المنتجات مع أسماء الكاتيجوري
    public async Task<List<(ResponseProductDto Product, string CategoryName)>> GetProductsWithCategoryAsync()
    {
        var products = await _client.GetFromJsonAsync<List<ResponseProductDto>>("api/Product")
                       ?? new List<ResponseProductDto>();

        var categories = await _client.GetFromJsonAsync<List<ResponseCategoryDto>>("api/Category")
                         ?? new List<ResponseCategoryDto>();

        // نعمل Dictionary لتسريع البحث عن الاسم
        var catDict = categories.ToDictionary(c => c.Id, c => c.Name);

        // نرجع قائمة من Tuple: (Product, CategoryName)
        var result = products.Select(p => (Product: p, CategoryName: catDict.ContainsKey(p.CategoryId) ? catDict[p.CategoryId] : "-"))
                             .ToList();

        return result;
    }
}