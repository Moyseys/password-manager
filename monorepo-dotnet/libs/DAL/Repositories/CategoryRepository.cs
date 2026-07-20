using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace DAL.Repositories;

public class CategoryRepository
{
    private readonly PasswordManagerDbContext _context;

    public CategoryRepository(PasswordManagerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        await _context.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> ListByUserId<T>(Guid userId, Expression<Func<Category, T>> projection)
    {
        return await _context.Category
            .AsNoTracking()
            .Where(c => c.UserId == userId && c.Active)
            .OrderBy(c => c.Name)
            .Select(projection)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAndUserId(Guid id, Guid userId)
    {
        return await _context.Category
            .Where(c => c.Id == id && c.UserId == userId && c.Active)
            .FirstOrDefaultAsync();
    }

    public async Task<Category?> GetByNameAndUserId(string name, Guid userId)
    {
        return await _context.Category
            .Where(c => c.UserId == userId && c.Active && c.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByNameAndUserId(string name, Guid userId, Guid? excludeId = null)
    {
        return await _context.Category
            .AnyAsync(c => c.UserId == userId
                && c.Active
                && c.Name.ToLower() == name.ToLower()
                && (excludeId == null || c.Id != excludeId));
    }

    public async Task<bool> ExistsByIdAndUserId(Guid id, Guid userId)
    {
        return await _context.Category.AnyAsync(c => c.Id == id && c.UserId == userId && c.Active);
    }

    public async Task<bool> HasActiveSecrets(Guid categoryId)
    {
        return await _context.Secret.AnyAsync(s => s.CategoryId == categoryId && s.Active);
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Category.Update(category);
        await _context.SaveChangesAsync();
    }
}
