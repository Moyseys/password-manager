using Core.Contexts;
using DAL.Repositories;
using Vaultify.Features.Dtos.Reponse;
using Vaultify.Core.Mappers;

namespace Vaultify.Features.Dashboard;

public class DashboardService(
    UserContext userContext,
    DashboardRepository dashboardRepository
)
{
    private readonly UserContext userContext = userContext;
    private readonly DashboardRepository dashboardRepository = dashboardRepository;

    public async Task<DashboardMetricsDto> GetMetrics()
    {
        var userId = userContext.GetUserIdOrThrow();
        var dv = await dashboardRepository.GetMetricsByUserIdAsync(userId);

        return dv?.ToDashboardMetricsDto() ?? new DashboardMetricsDto();
    }
}