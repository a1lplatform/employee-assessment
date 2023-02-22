using A1.SAS.Infrastructure.DBContext;
using Microsoft.Extensions.FileProviders;

namespace A1.SAS.Api.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServiceExtension(this IServiceCollection services)
        {
            services.AddTransient<IContextFactory, ContextFactory>();

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            return services;
        }
    }
}
