using Application;
using Application.Common;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MRA.Configurations.Initializer.Azure.AppConfig;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;
using WebApi;
using DependencyInjection = WebApi.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault("MRAIdentity");
    string appConfigConnectionString = builder.Configuration["AppConfigConnectionString"];
    builder.Configuration.AddAzureAppConfig(appConfigConnectionString);
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline.
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(DependencyInjection.POLICY_NAME);
// app.UseEndpoints(routeBuilder => { routeBuilder.MapControllers(); });

// Initialize and seed database
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IApplicationDbContextInitializer>();
    initializer.Initialize();
}

app.Run();