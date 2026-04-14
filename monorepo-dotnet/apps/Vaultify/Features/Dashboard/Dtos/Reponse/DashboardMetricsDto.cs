using DAL.Dtos;

namespace Vaultify.Features.Dtos.Reponse;

public record DashboardMetricsDto
{
    public long TotalSecrets { get; set; } = 0;
    public long NumberOfStrongSecrets { get; set; } = 0;
    public long NumberOfWeakSecrets { get; set; } = 0;
    public long SecurityScore { get; set; } = 0;
    public List<RecentSecretDto> RecentSecrets { get; set; } = [];
}