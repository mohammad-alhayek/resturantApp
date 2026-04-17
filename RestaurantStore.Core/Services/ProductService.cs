using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantStore.Core.Data;
using RestaurantStore.Core.Models;

namespace RestaurantStore.Core.Services
{

    public class ProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        // get all produvt
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        //get product by id
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        //get product by cat
        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                                 .Where(p => p.CategoryId == categoryId)
                                 .ToListAsync();
        }

        //trend product
        public async Task<List<Product>> GetTrendProductsAsync(int minOrders = 5, int defaultCount = 5)
        {
            //last 10 trend product
            var trendingProducts = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Where(g => g.Count() >= minOrders) 
                .Select(g => g.Key)
                .Join(_context.Products,
                      productId => productId,
                      p => p.Id,
                      (productId, product) => product)
                .ToListAsync();
             
            //if no trending product 
            if (!trendingProducts.Any())
            {
                trendingProducts = await _context.Products
                                        .Take(defaultCount)
                                        .ToListAsync();
            }

            return trendingProducts;
        }

        //.....................................admin.................................................
        //add product
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        //update product
        
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        //delete product
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }


    }
}
