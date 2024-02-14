using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Test.Identity
{
    public class AdminIdentityTests
    {
        Mock<IConfiguration> iConfigurationMock;
        AdminIdentityService identityService;

        [SetUp]
        public void SetUp()
        {
            iConfigurationMock = new Mock<IConfiguration>();
            identityService = new AdminIdentityService(iConfigurationMock.Object);
        }

        [Test]
        public async Task AdminIdentityService_ShouldReturnError_WhenApiKeyWasNotProvidedTest()
        {
            var response = await identityService.Authenticate(new Dictionary<string, StringValues>());
            Assert.That(response.Success, Is.False);
            Assert.That(response.Error, Is.EqualTo($"'{IdentityService.API_KEY_NAME}' is not provided"));
        }

        [Test]
        public async Task AdminIdentityService_ShouldReturnError_WhenApiKeyWasNotFoundInDataBaseTest()
        {
            var headers = new Dictionary<string, StringValues>();
            headers.Add(IdentityService.API_KEY_NAME, "SomeKey");
            var response = await identityService.Authenticate(headers);
            Assert.That(response.Success, Is.False);
            Assert.That(response.Error, Is.EqualTo("Invalid api key"));
        }
    }
}
