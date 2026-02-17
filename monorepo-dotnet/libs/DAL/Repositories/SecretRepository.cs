
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Dtos;
using DAL.Extensions;

namespace DAL.Repositories;

public class SecretRepository
{
    private PasswordManagerDbContext _context { get; set; }

    public SecretRepository(PasswordManagerDbContext context)
    {
        this._context = context;
    }

    public async Task AddAsync(Secret secret)
    {
        await _context.AddAsync(secret);
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetSecretById<T>(Guid secretId, Expression<Func<Secret, T>>? projection)
    {
        return await _context.Secret.Where((s) => s.Id == secretId).Select(projection).FirstOrDefaultAsync();
    }

    public async Task<Secret?> GetSecretById(Guid secretId)
    {
        return await _context.Secret.Where((s) => s.Id == secretId).FirstOrDefaultAsync();
    }

    public async Task<Secret?> GetSecretByIdAndUserId(Guid userId, Guid secretId)
    {
        return await _context.Secret.Where(s => s.UserId.Equals(userId) && s.Id.Equals(secretId)).FirstOrDefaultAsync();
    }

    public async Task<PageableDto<T>> GetSecretsPagedByUserId<T>(Guid userId, Expression<Func<Secret, T>> projection, PaginationDto pagination, string? search = null)
    {
        IQueryable<Secret> query = _context.Secret
            .AsNoTracking()
            .Where(s => s.UserId == userId && s.Active)
            .OrderByDescending(s => s.CreatedAt);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Title.ToLower().Contains(search.ToLower()));

        return await query.WithPagination(projection, pagination);
    }

    public async Task UpdateSecretAsync(Secret secret)
    {
        _context.Secret.Update(secret);
        await _context.SaveChangesAsync();
    }
}