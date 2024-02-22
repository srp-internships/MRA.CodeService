using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using MRA.Configurations.Initializer.Azure.AppConfig;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CreateHostBuilder(args).Build();

        var webAppBuilder = WebApplication.CreateBuilder(args);

        if (webAppBuilder.Environment.IsProduction())
        {
            webAppBuilder.Configuration.ConfigureAzureKeyVault("MraCodeService");
            string appConfigConnectionString = webAppBuilder.Configuration["AppConfigConnectionString"];
            webAppBuilder.Configuration.AddAzureAppConfig(appConfigConnectionString);
            webAppBuilder.Logging.AddApiApplicationInsights(webAppBuilder.Configuration);
        }

        await builder.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            webBuilder.UseStartup<Startup>());
}