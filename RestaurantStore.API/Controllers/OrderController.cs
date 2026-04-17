using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestaurantStore.Core.Models;
using RestaurantStore.Core.Services;
using RestaurantStore.Shared;
using RestaurantStore.Shared.OrderDto;

namespace RestaurantStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // ===============================
        // 1️⃣ Create Order
        // ===============================
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto model)
        {
            var order = await _orderService.AddOrderAsync(
                model.UserId,
                DateTime.Now,
                model.City,
                model.State,
                model.Street,
                model.TotalPrice,
                model.Notes,
                model.DeliveryAddress
            );

            var response = new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,

                TotalPrice = order.TotalPrice,
                City = order.City,
                State = order.State,
                Street = order.Street,
                Notes = order.Notes,
                Status = order.Status,
                DeliveryAddress = order.DeliveryAddress
              
            };

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, response);

        }

        // ===============================
        // 2️⃣ Get Order By Id
        // ===============================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            
            if (order == null)
                return NotFound();

            var response = new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                
                TotalPrice = order.TotalPrice,
                City = order.City,
                State = order.State,
                Street = order.Street,
                Notes = order.Notes,
                Status = order.Status
            };

            return Ok(response);
        }

        // ===============================
        // 3️⃣ Add Item To Order
        // ===============================
        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddItemToOrder(int orderId, [FromBody] AddOrderItemDto model)
        {
            await _orderService.AddItemToOrderAsync(orderId, model.ProductId, model.Quantity);
            return Ok("Item added to order");
        }

        // ===============================
        // 4️⃣ Search Orders
        // ===============================
        [HttpPost("search")]
        public async Task<IActionResult> SearchOrders([FromBody] SearchOrderDto model)
        {
            var orders = await _orderService.SearchOrdersAsync(
                
                model.Username,
                model.PhoneNumber
            );

            if (orders == null || orders.Count == 0)
                return NotFound("No orders found");

            return Ok(orders);
        }

        // ===============================
        // 5️⃣ Get All Orders (Admin)
        // ===============================
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // ===============================
        // 6️⃣ Update Order Status
        // ===============================
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> EditOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto model)
        {
            var updated = await _orderService.EditOrderStatusAsync(orderId, model.Status);

            if (!updated)
                return NotFound("Order not found");

            return Ok("Order status updated");
        }

        // ===============================
        // 7️⃣ Delete Order
        // ===============================
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var deleted = await _orderService.DeleteOrderAsync(orderId);

            if (!deleted)
                return NotFound("Order not found");

            return Ok("Order deleted");
        }
    }
}