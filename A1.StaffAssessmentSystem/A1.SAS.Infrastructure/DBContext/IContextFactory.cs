namespace A1.SAS.Infrastructure.DBContext
{
    public interface IContextFactory
    {
        IDbContext DbContext { get; }
    }
}
