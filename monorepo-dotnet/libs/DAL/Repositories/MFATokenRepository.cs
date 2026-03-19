using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class MFATokenRepository(PasswordManagerDbContext context)
{
    private readonly PasswordManagerDbContext _context = context;

    public async Task<MFAToken?> GetLatestUnusedTokenAsync(Guid mfaSettingsId)
    {
        return await _context.MFATokens
            .Where(t =>
                t.MFASettingsId == mfaSettingsId &&
                !t.IsUsed)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsValidTokenAsync(Guid mfaSettingsId)
    {
        return await _context.MFATokens
            .AnyAsync(t =>
                t.MFASettingsId == mfaSettingsId &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow);
    }

    public async Task MarkTokenAsUsedAsync(MFAToken token)
    {
        token.IsUsed = true;
        _context.MFATokens.Update(token);
        await _context.SaveChangesAsync();
    }

    public async Task InvalidateActiveTokensAsync(Guid mfaSettingsId)
    {
        var activeTokens = await _context.MFATokens
            .Where(t =>
                t.MFASettingsId == mfaSettingsId &&
                !t.IsUsed &&
                t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

        if (activeTokens.Count == 0)
        {
            return;
        }

        foreach (var token in activeTokens)
        {
            token.IsUsed = true;
        }

        _context.MFATokens.UpdateRange(activeTokens);
        await _context.SaveChangesAsync();
    }

    public async Task<MFAToken> AddAsync(MFAToken token)
    {
        var entityEntry = await _context.MFATokens.AddAsync(token);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(MFAToken token)
    {
        _context.MFATokens.Update(token);
        await _context.SaveChangesAsync();
    }
}
