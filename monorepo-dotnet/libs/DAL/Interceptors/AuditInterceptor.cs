namespace DAL.Interceptors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Core.Contexts;
using DAL.Entities.Commons;

public class AuditInterceptor(UserContext userContext) : SaveChangesInterceptor
{
    private readonly UserContext _userContext = userContext;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData dbContextEventData,
        InterceptionResult<int> result,
        CancellationToken cancellation
    )
    {
        var context = dbContextEventData.Context;
        if(context == null) return base.SavingChangesAsync(dbContextEventData, result, cancellation); 
        
        var entries = context.ChangeTracker
            .Entries<BaseAuditEntity>() //! Aqui é retornado somente Entities que herdam de BaseAuditEntity
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach(var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = _userContext.UserId;
            }
            else if (entry.State == EntityState.Modified)
            {
                var deleteAtProp = entry.Property(nameof(BaseEntity<Guid>.Active));
                var isSoftDelete = deleteAtProp.IsModified 
                    && deleteAtProp.CurrentValue != null
                    && deleteAtProp.CurrentValue.Equals(false);
                if (isSoftDelete)
                {
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.DeletedBy = _userContext.UserId;
                }
                else
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _userContext.UserId;
                }
            }
        }

        return base.SavingChangesAsync(dbContextEventData, result, cancellation);
    }
}