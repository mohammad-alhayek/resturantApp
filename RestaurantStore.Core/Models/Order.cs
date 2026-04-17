using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantStore.Shared.OrderDto;

namespace RestaurantStore.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } // رابط مع ApplicationUser
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string DeliveryAddress { get; set; } 
       public string City { get; set; }
       public string State { get; set; }
        public string Street { get; set; }
        public string? Notes { get; set; }

        public ApplicationUser User { get; set; } 
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
