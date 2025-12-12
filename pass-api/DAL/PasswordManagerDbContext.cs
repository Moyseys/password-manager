using Microsoft.EntityFrameworkCore;
using PasswordManager.DAL.Entities;
using PasswordManager.Extensions;

namespace PasswordManager.DAL;

public class PasswordManagerDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<Secret> Secret { get; set; }
    public DbSet<SecretKey> SecretKey { get; set; } 

    //TODO indexar colunas pricipais
    public PasswordManagerDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach(var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            if(entityName != null) entity.SetTableName(entityName.ToSnakeCase());
            foreach(var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }
        }
    }
}