using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using A1.SAS.Domain.Entities;

namespace A1.SAS.Infrastructure.DBContext
{
    public class A1PlatformDbContext : DbContext, IDbContext
    {
        public A1PlatformDbContext(DbContextOptions<A1PlatformDbContext> options) : base(options)
        {

        }
        public DbSet<TblEmployee> Employees { get; set; }
        public DbSet<TblRange> Ranges { get; set; }
        public DbSet<TblAssessment> Assessments { get; set; }
        public DbSet<TblAccount> Accounts { get; set; }
        public DbSet<TblImages> Images { get; set; }
        public DbSet<TblHistory> Histories { get; set; }
        
        public ChangeTracker ChangeTracking()
        {
            return ChangeTracker;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = string.IsNullOrEmpty(entry.Entity.CreatedBy) ? "admin" : entry.Entity.CreatedBy;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = string.IsNullOrEmpty(entry.Entity.UpdatedBy) ? "admin" : entry.Entity.UpdatedBy;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
