using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class MFASettingsRepository(PasswordManagerDbContext context)
{
    private readonly PasswordManagerDbContext _context = context;

    public IQueryable<MFASettings> QueryByUserId(Guid userId)
    {
        return _context.MFASettings
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt);
    }

    public async Task<bool> ExistsByUserIdAndTypeAsync(Guid userId, MFAType type)
    {
        return await _context.MFASettings.AnyAsync(m => m.UserId == userId && m.Type == type);
    }

    public async Task<MFASettings> AddAsync(MFASettings settings)
    {
        var entityEntry = await _context.MFASettings.AddAsync(settings);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task<MFASettings?> GetByIdAndUserIdAsync(Guid id, Guid userId)
    {
        return await _context.MFASettings
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
    }

    public async Task UpdateStateAsync(Guid id, MFASettingsState state)
    {
        var mfaSettings = await _context.MFASettings.FirstOrDefaultAsync(m => m.Id == id)
            ?? throw new InvalidOperationException("MFA settings not found.");

        mfaSettings.State = state;
        _context.MFASettings.Update(mfaSettings);
        await _context.SaveChangesAsync();
    }
}