using System.Collections.Generic;
using Core.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace WebApi;

public static class DependencyInjection
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();

        services.AddSwaggerServices();
        services.AddControllers(s => s.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.EnableCorePolicies(configuration);
    }

    public const string POLICY_NAME = "My policy";
    private const string CORS_CONFIG_SECTION_NAME = "CORS";

    private static void EnableCorePolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var corsAllowedHosts = configuration.GetSection(CORS_CONFIG_SECTION_NAME).Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy(POLICY_NAME, policyConfig =>
            {
                policyConfig.WithOrigins(corsAllowedHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    const string API_KEY = "API_KEY";

    private static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Dot Net compiler", Version = "v1" });
            setup.AddSecurityDefinition(API_KEY, new OpenApiSecurityScheme
            {
                Description =
                    $"JWT Authentication header using the API_KEY scheme. Enter '{API_KEY}' [space] and then your token in the text input below. Example: '12345abcdef'",
                Name = API_KEY,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = API_KEY
            });
            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = API_KEY
                },
                In = ParameterLocation.Header,
            };
            var requirement = new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            };
            setup.AddSecurityRequirement(requirement);
        });
    }
}