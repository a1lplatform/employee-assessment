using A1.SAS.Api.Helpers;
using A1.SAS.Api.Services;
using A1.SAS.Api.Services.Implement;
using A1.SAS.Infrastructure.DBContext;
using Microsoft.Extensions.FileProviders;

namespace A1.SAS.Api.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServiceExtension(this IServiceCollection services)
        {
            services.AddTransient<IContextFactory, ContextFactory>();

            services.AddTransient(typeof(IAccountService), typeof(AccountService));
            services.AddTransient(typeof(IEmployeeService), typeof(EmployeeService));
            services.AddTransient(typeof(IRangeService), typeof(RangeService));
            services.AddTransient(typeof(IAssessmentService), typeof(AssessmentService));

            services.AddAutoMapper(typeof(AutoMapperProfiles));            

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            return services;
        }
    }
}
