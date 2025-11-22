
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PasswordManager.DAL.Entities;
using PasswordManager.SharedDtos;

namespace PasswordManager.DAL.Repositories;

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

    public async Task<List<T>> ListSecrets<T>(Guid userId, PaginationDto pagination, Expression<Func<Secret, T>> projection)
    {
        var totalItems = await _context.Secret.CountAsync((s) => s.UserId == userId);
        var secrets = await _context.Secret
            .Where(s => s.UserId == userId)
            .Select(projection)
            .Skip(pagination.Size * (pagination.Page - 1))
            .Take(pagination.Size)
            .ToListAsync();
       
        return secrets;
    }

    public async Task<T?> GetSecretById<T>(Guid secretId, Expression<Func<Secret, T>> projection)
    {
        return await _context.Secret.Where((s) => s.Id == secretId).Select(projection).FirstOrDefaultAsync();
    }
}