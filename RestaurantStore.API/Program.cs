using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestaurantStore.API.Hubs;
using RestaurantStore.Core.Data;
using RestaurantStore.Core.Interfaces;
using RestaurantStore.Core.Models;
using RestaurantStore.Core.Services;

namespace RestaurantStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // ✅ CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("WebPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7154") // ← بورت الـ Web
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // مهم جداً لـ SignalR
                });
            });

            // Services
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddSingleton<IOrderNotificationService>(sp =>
            {
                var hub = sp.GetRequiredService<IHubContext<DashboardHub>>();
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new SignalRNotificationService(hub, scopeFactory);
            }); builder.Services.AddScoped<DashboardService>();
            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ✅ CORS لازم يجي قبل Authentication
            app.UseCors("WebPolicy");

            app.UseAuthentication();
            app.UseStaticFiles(); // ← لازم يكون موجود
            app.UseAuthorization();

            app.MapControllers();

            // ✅ Hub في الأخير
            app.MapHub<DashboardHub>("/dashboardHub");

            app.Run();
        }
    }
}