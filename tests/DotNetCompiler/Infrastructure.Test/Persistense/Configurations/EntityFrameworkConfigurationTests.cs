using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Text;

namespace Infrastructure.Test.Persistence.Configurations
{
    public class EntityFrameworkConfigurationTests
    {
        [Test]
        public void EntityFrameworkMapping_ShouldBuild_ValidModelTest()
        {
            var serviceProvider = CreateServiceProvider();
            var context = serviceProvider.GetService<ApplicationDbContext>();
            try
            {
                _ = context.Model;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Invalid model configuration. See error details:\n  {GetExceptionDetails(ex)}");
            }
        }

        ServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(op => op.UseInMemoryDatabase("TestingConfigureDB"));
            return services.BuildServiceProvider();
        }

        string GetExceptionDetails(Exception exception)
        {
            StringBuilder messageBuilder = new();
            while (exception != null)
            {
                messageBuilder.Append(exception.Message);
                exception = exception.InnerException;
            }
            return messageBuilder.ToString();
        }
    }
}
