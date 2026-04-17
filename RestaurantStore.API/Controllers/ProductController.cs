using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Core.Models;
using RestaurantStore.Core.Services;
using RestaurantStore.Shared;
using RestaurantStore.Shared.ProductDto;

namespace RestaurantStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // ===============================
        // 1️⃣ Get All Products
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();

            var result = products.Select(p => new 
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
            });

            return Ok(result);
        }

        // ===============================
        // 2️⃣ Get Product By Id
        // ===============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var result = new ResponseProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };

            return Ok(result);
        }

        // ===============================
        // 3️⃣ Get Products By Category
        // ===============================
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);

            var result = products.Select(p => new ResponseProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
            });

            return Ok(result);
        }

        // ===============================
        // 4️⃣ Get Trending Products
        // ===============================
        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingProducts()
        {
            var products = await _productService.GetTrendProductsAsync();

            var result = products.Select(p => new ResponseProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
            });

            return Ok(result);
        }

        // ===============================
        // 🔐 ADMIN OPERATIONS
        // ===============================
        //add aitem
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ResponseProductDto model, IFormFile? image)
        {
            string? imagePath = null;

            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var savePath = Path.Combine("wwwroot", "images", "products", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                using var stream = new FileStream(savePath, FileMode.Create);
                await image.CopyToAsync(stream);
                imagePath = $"/images/products/{fileName}";
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                ImageUrl = imagePath
            };

            await _productService.AddProductAsync(product);
            return Ok("Product added successfully");
        }
        //edit item
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ResponseProductDto model, IFormFile? image)
        {
            Console.WriteLine($">>> ImageUrl من الـ form: {model.ImageUrl}");
            Console.WriteLine($">>> image file: {image?.FileName ?? "null"}");
            if (id != model.Id)
                return BadRequest("Id mismatch");

            string? imagePath = model.ImageUrl;

            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                using var stream = new FileStream(savePath, FileMode.Create);
                await image.CopyToAsync(stream);
                imagePath = $"/images/products/{fileName}";
            }

            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CategoryId = model.CategoryId,
                ImageUrl = imagePath
            };

            await _productService.UpdateProductAsync(product);
            return Ok("Product updated");
        }
        // Delete Product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok("Product deleted");
        }
    }
}