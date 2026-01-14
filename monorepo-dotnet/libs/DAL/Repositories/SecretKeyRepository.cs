using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Repositories;

public class SecretKeyRepository
{
    private PasswordManagerDbContext Context { get; set; }

    public SecretKeyRepository(PasswordManagerDbContext context)
    {
        this.Context = context;
    }

    public async Task AddAsync(SecretKey secretkey)
    {
        await Context.AddAsync(secretkey);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> ExistByUserId(Guid userId)
    {
        return await Context.SecretKey.AnyAsync(sk => sk.UserId == userId);
    }

    public async Task<T?> GetByUserId<T>(Guid userId, Expression<Func<SecretKey, T>> projection)
    {
        return await Context.SecretKey.Where((sk) => sk.UserId == userId).Select(projection).FirstOrDefaultAsync<T>();
    }

    public async Task<SecretKey?> GetSecretKeyByUserId(Guid userId)
    {
        return await Context.SecretKey
        .Include((sk) => sk.User)
        .Where((sk) => sk.UserId == userId)
        .FirstOrDefaultAsync();
    }
} 