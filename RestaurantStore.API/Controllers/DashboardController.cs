using Microsoft.AspNetCore.Mvc;
using RestaurantStore.Shared;
using RestaurantStore.Core.Services;
using RestaurantStore.Shared.Dashboard;


namespace RestaurantStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        { 
            _dashboardService = dashboardService;
        }

        // GET: api/dashboard
        [HttpGet]
        public async Task<ActionResult<DashboardDto>> GetDashboard()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();

            return Ok(dashboardData);
        }
    }
}