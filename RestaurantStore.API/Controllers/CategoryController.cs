using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Core;
using RestaurantStore.Core.Models;
using RestaurantStore.Core.Services;
using RestaurantStore.Shared.CategoryDto;

namespace RestaurantStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<ResponseCategoryDto>>> GetAll()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseCategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(ResponseCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
               Description = dto.Description
            };

            await _categoryService.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ResponseCategoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var category = new Category
            {
                Id = dto.Id,
                Name = dto.Name,
              Description=dto.Description
            };

            await _categoryService.UpdateCategoryAsync(category);

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}