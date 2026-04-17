using RestaurantStore.Core.Data;
using RestaurantStore.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantStore.Core.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        //get all cat

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        //get cat by id
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        //.........................admin.......................................
        //add cat
        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

        }

        //update cat
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        //delete cat
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
