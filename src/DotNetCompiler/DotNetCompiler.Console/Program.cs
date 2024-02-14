using Autofac;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Services.CSharp;
using DotNetCompiler.Console.Services.DotNetCompiler;
using DotNetCompiler.Console.Startup;
using DotNetCompiler.Console.Strategies;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            var container = ConfigureContainer();
            var application = container.Resolve<ApplicationStartup>();
            await application.Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Environment.Exit(1);
        }
    }

    private static IContainer ConfigureContainer()
    {
        var builder = new ContainerBuilder();

        AddApplicationStartupServices(builder);
        AddRepositories(builder);
        AddCompilerServices(builder);
        AddStrategyServices(builder);
        return builder.Build();
    }

    static void AddApplicationStartupServices(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationStartup>().AsSelf();
        builder.RegisterType<ApplicationParameterParser>().As<IApplicationParameterParser>().SingleInstance();
    }

    static void AddRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<DotNetDirectoryProvider>().As<IDirectoryProvider>().SingleInstance();
        builder.RegisterType<DotNetFrameworkProvider>().As<IDotNetFrameworkProvider>().SingleInstance();
    }

    static void AddCompilerServices(ContainerBuilder builder)
    {
        builder.RegisterType<CSharpCodeCompilerService>().As<IDotNetCompilerService>().SingleInstance();
        builder.RegisterType<CSharpCodeAnalyzerService>().As<IDotNetCodeAnalyzerConsoleService>().SingleInstance();
    }

    static void AddStrategyServices(ContainerBuilder builder)
    {
        builder.RegisterType<DotNetCompilerResolver>().As<IDotNetCompilerResolver>().SingleInstance();
        builder.RegisterType<DotNetCodeAnalyzerResolver>().As<IDotNetCodeAnalyzerResolver>().SingleInstance();
    }
}