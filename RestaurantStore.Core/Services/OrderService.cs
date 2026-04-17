using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantStore.Core.Data;
using RestaurantStore.Core.Interfaces;
using RestaurantStore.Core.Models;
using RestaurantStore.Shared.OrderDto;


namespace RestaurantStore.Core.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly IOrderNotificationService _notification;

        public OrderService(AppDbContext context, IOrderNotificationService notification)
        {
            _context = context;
            _notification = notification;
        }
        //add order
       public async Task<Order> AddOrderAsync(
       string userId,
       DateTime orderDate,
       string city,
       string state,
       string street,
       decimal totalPrice,
         string notes = null ,string DeliveryAddres=null)
        {
           //create order
            var order = new Order
            {
                UserId = userId,         // UserId
                OrderDate = orderDate,       // تاريخ الطلب
                Status = OrderStatus.Pending, // الحالة الابتدائية
                City = city,                 // عنوان التوصيل - المدينة
                State = state,               // عنوان التوصيل - المحافظة
                Street = street,             // عنوان التوصيل - الشارع
                TotalPrice = totalPrice,     // السعر الإجمالي
                Notes = notes   
                ,
                DeliveryAddress = DeliveryAddres
            };

            //add
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await _notification.NewOrderCreated(order);

            return order;
        }
        //add items to order
        public async Task AddItemToOrderAsync(int orderId, int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            _context.OrderItems.Add(orderItem);

         
            var order = await _context.Orders.FindAsync(orderId);
           //update price
            order.TotalPrice += product.Price * quantity;

            await _context.SaveChangesAsync();
            await _notification.NewOrderCreated(order);
        }
        //find order by orderId
        public async Task<Order>GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
        //finr orde by userName 
        public async Task<List<Order>> SearchOrdersAsync( string username = null, string phoneNumber = null)
        {
            
            var query = _context.Orders
                                .Include(o => o.User)       // جلب معلومات اليوزر المرتبط
                                .Include(o => o.OrderItems) // جلب كل الأوردر آيتمز
                                    .ThenInclude(oi => oi.Product) // ربط كل OrderItem بالمنتج نفسه
                                .AsQueryable();

           

            // فلترة حسب username لو موجود
            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(o => o.User.UserName.Contains(username));
            }

            // فلترة حسب phoneNumber لو موجود
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(o => o.User.PhoneNumber.Contains(phoneNumber));
            }

            // تنفيذ الاستعلام وإرجاع النتائج
            return await query.ToListAsync();
        }
        
        //.........................admin.......................................
        //get all order 
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                                 .Include(o => o.User)       // معلومات اليوزر
                                 .Include(o => o.OrderItems) // العناصر
                                 .ThenInclude(oi => oi.Product) // معلومات كل منتج
                                 .ToListAsync();
        }
        //edit status 
        public async Task<bool> EditOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
        //delete order
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderItems) // لازم نحذف العناصر المرتبطة
                                      .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return false;

            // حذف العناصر المرتبطة
            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
