using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Core.Extensions;
using DAL.Entities.Views;

namespace DAL;

public class PasswordManagerDbContext : DbContext
{
    public DbSet<User> User { get; set; }
    public DbSet<MFASettings> MFASettings { get; set; }
    public DbSet<MFAToken> MFATokens { get; set; }
    public DbSet<Secret> Secret { get; set; }
    public DbSet<SecretKey> SecretKey { get; set; }
    public DbSet<DashboardVaultify> DashboardVaultify { get; set; }

    public PasswordManagerDbContext(DbContextOptions contextOptions) : base(contextOptions) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.GetTableName();
            if (entityName != null) entity.SetTableName(entityName.ToSnakeCase());
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }
        }

        //MFA Settings
        modelBuilder.Entity<MFASettings>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<MFASettings>()
            .Property(e => e.State)
            .HasConversion<string>();
        //Dashboard View    
        modelBuilder.Entity<DashboardVaultify>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("dashboard_vaultify");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TotalSecrets).HasColumnName("total_secrets");
        });
    }
}