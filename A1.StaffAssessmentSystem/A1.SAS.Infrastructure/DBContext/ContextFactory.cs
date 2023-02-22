using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace A1.SAS.Infrastructure.DBContext
{
    public class ContextFactory : IContextFactory
    {
        private readonly IConfiguration _configuration;

        public ContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbContext DbContext
        {
            get
            {
                var contextOptionsBuilder = new DbContextOptionsBuilder<A1PlatformDbContext>();
                contextOptionsBuilder.UseMySql(_configuration.GetConnectionString("A1PlatformConnection"),
                           new MySqlServerVersion(new Version(8, 0, 28)),
                           options => options.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null)
                           )
                    .EnableSensitiveDataLogging();

                var ctx = new A1PlatformDbContext(contextOptionsBuilder.Options);
                return ctx;
            }
        }
    }
}
