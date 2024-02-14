using Application.Common;
using Application.DotNetCodeAnalyzer.Services;
using Application.Identity;
using Application.Repositories;
using DotNetCompiler.Console.Services;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        const string USE_MEMORY_DB = "UseInMemoryDatabase";
        const string ONLINE_PLATFORM_DB = "CSharpOnlinePlatformDb";
        const string DEFAULT_CONNECTION = "DefaultConnection";
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDbContext(configuration);
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAdminIdentityService, AdminIdentityService>();
            services.AddScoped<IDotNetCodeAnalyzerService, DotNetCodeAnalyzerService>();
            return services;
        }

        static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>(USE_MEMORY_DB))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(ONLINE_PLATFORM_DB));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString(DEFAULT_CONNECTION),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IApplicationDbContextInitializer, ApplicationDbContextInitializer>();

            return services;
        }
    }
}
