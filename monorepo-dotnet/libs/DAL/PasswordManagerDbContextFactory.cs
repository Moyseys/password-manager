using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

/// <summary>
/// Factory for creating instances of PasswordManagerDbContext at design time.
/// Necessary for EF Core commands like 'dotnet ef migrations add' to work.
/// </summary>
public class PasswordManagerDbContextFactory : IDesignTimeDbContextFactory<PasswordManagerDbContext>
{
    public PasswordManagerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PasswordManagerDbContext>();
        
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") 
            ?? "Host=localhost;Port=5432;Database=password_manager;Username=postgres;Password=postgres";
        
        optionsBuilder.UseNpgsql(connectionString);
        
        return new PasswordManagerDbContext(optionsBuilder.Options);
    }
}
