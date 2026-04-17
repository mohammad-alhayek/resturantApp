using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantStore.Shared.Dashboard
{
    public class DashboardDto
    {
        public int TotalOrders { get; set; }

        public int TodayOrders { get; set; }

        public int PendingOrders { get; set; }

        public int TotalProducts { get; set; }

        public int TotalUsers { get; set; }

        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }
    public class RecentOrderDto
    {
        public int OrderId { get; set; }

        public string Username { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
