using DAL.Entities.Commons;
using DAL.Enums;

namespace DAL.Entities;

public class MFASettings : BaseEntity<Guid>
{
    public required MFAType Type { get; set; }
    public required MFASettingsState State { get; set; } = MFASettingsState.Inactive;
    public required Guid UserId { get; set; }
    public User? User { get; set; }

    public ICollection<MFAToken> Tokens { get; set; } = [];
}