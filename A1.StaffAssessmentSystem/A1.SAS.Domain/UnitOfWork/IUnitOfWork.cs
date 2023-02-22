using A1.SAS.Domain.Entities;
using A1.SAS.Domain.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A1.SAS.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        int Commit();

        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        ChangeTracker ChangeTracker();
    }
}
