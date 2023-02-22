using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using A1.SAS.Api.Extension;
using A1.SAS.Api.Middleware;
using A1.SAS.Domain.UnitOfWork;
using A1.SAS.Infrastructure.DBContext;
using A1.SAS.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Inject all services
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);
builder.Services.AddServiceExtension();
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database
builder.Services.AddScoped<IContextFactory, ContextFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<A1PlatformDbContext>(
               x => x.UseMySql(builder.Configuration.GetConnectionString("A1PlatformConnection"),
               new MySqlServerVersion(new Version(8, 0, 28)),
               options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
               ), ServiceLifetime.Transient);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Configure all middlewares
var app = builder.Build();



app.UseMiddleware<ExceptionMiddleware>();

//if (env.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocExpansion(DocExpansion.None);
});
//}

app.UseCors();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.RunAsync();