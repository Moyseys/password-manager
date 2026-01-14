
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

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

    public async Task <Secret?> GetSecretByIdAndUserId(Guid userId, Guid secretId)
    {
        return await _context.Secret.Where(s => s.UserId.Equals(userId) && s.Id.Equals(secretId)).FirstAsync();
    }
}