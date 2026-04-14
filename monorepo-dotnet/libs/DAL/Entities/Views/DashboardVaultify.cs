using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using DAL.Dtos;

namespace DAL.Entities.Views;

public class DashboardVaultify
{
    public Guid UserId { get; set; }
    public long TotalSecrets { get; set; }
    public long NumberOfStrongSecrets { get; set; }
    public long NumberOfWeakSecrets { get; set; }
    public long SecurityScore { get; set; }

    [NotMapped]
    public List<RecentSecretDto> RecentSecrets { get; set; } = [];

    public string? RecentSecretsJson { get; set; }

    public void ParseRecentSecrets()
    {
        if (string.IsNullOrEmpty(RecentSecretsJson))
        {
            RecentSecrets = [];
            return;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            RecentSecrets = JsonSerializer.Deserialize<List<RecentSecretDto>>(RecentSecretsJson, options) ?? [];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao fazer parse de RecentSecrets: {ex.Message}");
            RecentSecrets = [];
        }
    }
}