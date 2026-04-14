using Microsoft.EntityFrameworkCore;
using DAL.Entities.Views;

namespace DAL.Repositories;

public class DashboardRepository
{
    private PasswordManagerDbContext context { get; set; }

    public DashboardRepository(PasswordManagerDbContext context)
    {
        this.context = context;
    }

    public async Task<DashboardVaultify?> GetMetricsByUserIdAsync(Guid userId)
    {
        var result = await context.DashboardVaultify
            .Where(dv => dv.UserId == userId)
            .FirstOrDefaultAsync();

        result?.ParseRecentSecrets();
        return result;
    }
}