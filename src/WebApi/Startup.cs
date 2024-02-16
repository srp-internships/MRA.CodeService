using Application;
using Application.Common;
using Core.Filters;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            // Add services to the container.
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            AddSwaggerServices(services);
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllersWithViews(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>());
            EnableCorePolicies(services);
        }

        const string POLICY_NAME = "My policy";
        const string CORS_CONFIG_SECTION_NAME = "CORS";
        void EnableCorePolicies(IServiceCollection services)
        {
            var corsAllowedHosts = Configuration.GetSection(CORS_CONFIG_SECTION_NAME).Get<string[]>();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            if (env.IsDevelopment())
            {
                // Configure the HTTP request pipeline.
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCors(POLICY_NAME);
            app.UseEndpoints(routebuilder =>
            {
                routebuilder.MapControllers();
            });

            // Initialize and seed database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IApplicationDbContextInitializer>();
                initializer.Initialize();
            }
        }

        const string API_KEY = "API_KEY";
        void AddSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Dot Net compiler", Version = "v1" });
                setup.AddSecurityDefinition(API_KEY, new OpenApiSecurityScheme
                {
                    Description = $"JWT Authentication header using the API_KEY scheme. Enter '{API_KEY}' [space] and then your token in the text input below. Example: '12345abcdef'",
                    Name = API_KEY,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = API_KEY
                });
                var securityScheme = new OpenApiSecurityScheme()
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
}
