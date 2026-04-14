using System.Text.Json.Serialization;

namespace DAL.Dtos;

public class RecentSecretDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string UserEmail { get; set; }
    public string? Category { get; set; }
    public string? Strength { get; set; }
    public DateTime CreatedAt { get; set; }
}
