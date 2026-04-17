using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantStore.Core.Data;
using RestaurantStore.Shared.Dashboard;
using RestaurantStore.Shared.OrderDto;

namespace RestaurantStore.Core.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;
        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var today=DateTime.Today;
            var totalOrder=await _context.Orders.CountAsync();
            var todayOrders =await _context.Orders.Where(o=>o.OrderDate==today).CountAsync();
            var pendingOrders = await _context.Orders
                .Where(o => o.Status == OrderStatus.Pending)
                .CountAsync();
            var totalProducts = await _context.Products.CountAsync();

            var totalUsers = await _context.Users.CountAsync();

            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentOrderDto
                {
                    OrderId = o.Id,
                    Username = o.User.UserName,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status.ToString(),
                    OrderDate = o.OrderDate
                })
                .ToListAsync();

            var dashboard = new DashboardDto
            {
                TotalOrders = totalOrder,
                TodayOrders = todayOrders,
                PendingOrders = pendingOrders,
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,
                RecentOrders = recentOrders
            };

            return dashboard;


        }
    }
}
