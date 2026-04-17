using Microsoft.AspNetCore.SignalR;
using RestaurantStore.Core.Interfaces;
using RestaurantStore.Shared.Dashboard;
using Microsoft.AspNetCore.SignalR;
using RestaurantStore.Core.Services;
using RestaurantStore.API.Hubs;
namespace RestaurantStore.API.Hubs
{
    public class DashboardHub : Hub
    {
        public async Task SendDashboardUpdate(DashboardDto data)
        {
            await Clients.All.SendAsync("ReceiveDashboardUpdate", data);
        }

    }
}

