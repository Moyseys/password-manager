using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using Core.Extensions;
using DAL.Entities.Views;
using DAL.Enums;

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
        // Dashboard View
        modelBuilder.Entity<DashboardVaultify>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("dashboard_vaultify", "public");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TotalSecrets).HasColumnName("total_secrets");
            entity.Property(e => e.NumberOfStrongSecrets).HasColumnName("number_of_strong_secrets");
            entity.Property(e => e.NumberOfWeakSecrets).HasColumnName("number_of_weak_secrets");
            entity.Property(e => e.SecurityScore).HasColumnName("security_score");
            entity.Property(e => e.RecentSecretsJson).HasColumnName("recent_secrets");
            entity.Ignore(e => e.RecentSecrets);
        });

        //Rename tables and columns to snake_case
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetViewName() is not null)
            {
                continue;
            }

            var entityName = entity.GetTableName();
            if (entityName != null) entity.SetTableName(entityName.ToSnakeCase());
            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (columnName != null)
                {
                    property.SetColumnName(columnName.ToSnakeCase());
                }
            }
        }

        //MFA Settings
        modelBuilder.Entity<MFASettings>()
            .Property(e => e.Type)
            .HasConversion<string>();

        modelBuilder.Entity<MFASettings>()
            .Property(e => e.State)
            .HasConversion<string>();

        modelBuilder.Entity<Secret>()
            .Property(s => s.Strength)
            .HasConversion<string>();
    }
}