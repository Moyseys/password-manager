using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Repositories;

public class UserResitory
{
    private PasswordManagerDbContext context { get; set; }

    public UserResitory(PasswordManagerDbContext context)
    {
        this.context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.User
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmailWithMFAAsync(string email)
    {
        return await context.User
            .Include(u => u.MFASettings)
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
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

    public async Task AddWithSecretKeyAsync(User user)
    {
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserByIdAsync(Guid id)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Id.Equals(id));
        if (user != null)
        {
            context.User.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}