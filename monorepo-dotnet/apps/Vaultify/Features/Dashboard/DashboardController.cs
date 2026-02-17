using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vaultify.Features.Dtos.Reponse;

namespace Vaultify.Features.Dashboard;

[ApiController]
[Route("api/v1/dashboard")]
[Authorize]
public class VaultifyController(
    DashboardService dashboardService
) : ControllerBase
{
    private readonly DashboardService dashboardService = dashboardService;

    [HttpGet()]
    public async Task<DashboardMetricsDto> GetMetrics()
    {
        return await dashboardService.GetMetrics();
    }
}