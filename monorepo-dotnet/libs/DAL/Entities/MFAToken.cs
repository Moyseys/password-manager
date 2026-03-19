using System.ComponentModel.DataAnnotations;
using DAL.Entities.Commons;

namespace DAL.Entities;

public class MFAToken : BaseEntity<Guid>
{
    public required string TokenHash { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required bool IsUsed { get; set; }
    public int AttemptCount { get; set; } = 0;

    public required Guid MFASettingsId { get; set; }
    public MFASettings? MFASettings { get; set; }
}