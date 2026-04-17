using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using RestaurantStore.API.Hubs;
using RestaurantStore.Core.Interfaces;
using RestaurantStore.Core.Services;

public class SignalRNotificationService : IOrderNotificationService
{
    private readonly IHubContext<DashboardHub> _hub;
    private readonly IServiceScopeFactory _scopeFactory;

    public SignalRNotificationService(IHubContext<DashboardHub> hub, IServiceScopeFactory scopeFactory)
    {
        _hub = hub;
        _scopeFactory = scopeFactory;
    }

    public async Task NewOrderCreated(object order)
    {
        using var scope = _scopeFactory.CreateScope();
        var dashboardService = scope.ServiceProvider.GetRequiredService<DashboardService>();
        var dashboardData = await dashboardService.GetDashboardDataAsync();
        await _hub.Clients.All.SendAsync("ReceiveDashboardUpdate", dashboardData);
    }
}