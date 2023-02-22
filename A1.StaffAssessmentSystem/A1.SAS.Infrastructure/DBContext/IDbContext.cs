using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A1.SAS.Infrastructure.DBContext
{
    public interface IDbContext
    {
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
        void Dispose();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        ChangeTracker ChangeTracking();
    }
}
