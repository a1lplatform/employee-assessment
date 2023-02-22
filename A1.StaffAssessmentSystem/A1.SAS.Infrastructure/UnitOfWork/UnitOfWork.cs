using A1.SAS.Domain.Entities;
using A1.SAS.Domain.Repositories;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.DBContext;
using A1.SAS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace A1.SAS.Infrastructure.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private IDbContext dbContext;

        /// <summary>
        /// The repositories
        /// </summary>
        private Dictionary<Type, object> repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        /// <param name="contextFactory">The context factory.</param>
        public UnitOfWork(IContextFactory contextFactory)
        {
            dbContext = contextFactory.DbContext;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity
        {
            repositories ??= new Dictionary<Type, object>();

            var type = typeof(TEntity);

            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(dbContext);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            // Save changes with the default options
            return dbContext.SaveChanges();
        }

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
            => dbContext.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
            GC.SuppressFinalize(obj: this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext?.Dispose();
            }
        }

        public ChangeTracker ChangeTracker()
        {
            return dbContext.ChangeTracking();
        }
    }
}
