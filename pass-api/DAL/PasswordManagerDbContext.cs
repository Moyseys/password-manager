using Microsoft.EntityFrameworkCore;
using PasswordManager.DAL.Entities;

namespace PasswordManager.DAL;

public class PasswordManagerDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Secret> Secret { get; set; }
    public DbSet<SecretKey> SecretKey { get; set; } 

    //TODO indexar colunas pricipais
    public PasswordManagerDbContext(DbContextOptions contextOptions) : base(contextOptions) { }
}