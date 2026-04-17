using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestaurantStore.Shared.OrderDto
{
    public enum OrderStatus
    {
        Pending,
        Preparing,
        OutForDelivery,
        Completed
    }
    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }
    }
    public class AddOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class SearchOrderDto
    {
        public int? OrderId { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string DeliveryAddress { get; set; }
        public string? Notes { get; set; }
        public OrderStatus Status { get; set; }
    }
}
