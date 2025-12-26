using Microsoft.EntityFrameworkCore;
using PasswordManager.DAL.Entities;

namespace PasswordManager.DAL.Repositories;

public class UserResitory
{
    private PasswordManagerDbContext context { get; set; }

    public UserResitory(PasswordManagerDbContext context)
    {
        this.context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Id.Equals(id));
    } 

    public async Task AddAsync(User u)
    {
        await context.User.AddAsync(u);
        await context.SaveChangesAsync();
    }

    public async Task AddWithSecretKeyAsync(User user, SecretKey secretKey)
    {
        await context.User.AddAsync(user);
        await context.SecretKey.AddAsync(secretKey);
        await context.SaveChangesAsync();
    }
}