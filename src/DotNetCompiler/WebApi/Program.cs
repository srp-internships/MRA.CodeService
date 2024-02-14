using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using MRA.Jobs.Web.AzureKeyVault;

namespace WebApi;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = CreateHostBuilder(args).Build();

        var webAppBuilder = WebApplication.CreateBuilder(args);

        webAppBuilder.ConfigureAzureKeyVault();

        await builder.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureLogging((context, builder) =>
        {
            if (!context.HostingEnvironment.IsDevelopment())
            {
                builder.AddApplicationInsights(
              configureTelemetryConfiguration: (config) =>
              {
                  config.ConnectionString = context.Configuration["Logging:ApplicationInsights:ConnectionString"];
              },
              configureApplicationInsightsLoggerOptions: (options) => { });
            }
        })
        .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>());
}