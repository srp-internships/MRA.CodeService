using Application.Repositories;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Test.Identity
{
    public class IdentityTests
    {
        Mock<IApplicationDbContext> applicationDbContextMock;
        IdentityService identityService;

        [SetUp]
        public void SetUp()
        {
            applicationDbContextMock = new Mock<IApplicationDbContext>();
            identityService = new IdentityService(applicationDbContextMock.Object);
        }

        [Test]
        public async Task IdentityService_ShouldReturnError_WhenApiKeyWasNotProvidedTest()
        {
            var response = await identityService.Authenticate(new Dictionary<string, StringValues>());
            Assert.That(response.Success, Is.False);
            Assert.That(response.Error, Is.EqualTo($"'{IdentityService.API_KEY_NAME}' is not provided"));
        }

        [Test]
        public async Task IdentityService_ShouldReturnError_WhenApiKeyWasNotFoundInDataBaseTest()
        {
            var headers = new Dictionary<string, StringValues>();
            headers.Add(IdentityService.API_KEY_NAME, "SomeKey");
            var response = await identityService.Authenticate(headers);
            Assert.That(response.Success, Is.False);
            Assert.That(response.Error, Is.EqualTo("Invalid api key"));
        }

        [Test]
        public async Task IdentityService_ShouldReturnSuccess_WhenApiKeyWasFoundInDataBaseTest()
        {
            const string existingKeyInDataBase = "ExistingKey";
            applicationDbContextMock.Setup(s => s.GetApiKeyAsync(existingKeyInDataBase)).Returns(Task.FromResult(new ApiKey() { SecretKey = existingKeyInDataBase }));

            var headers = new Dictionary<string, StringValues>();
            headers.Add(IdentityService.API_KEY_NAME, existingKeyInDataBase);
            var response = await identityService.Authenticate(headers);
            Assert.That(response.Success, Is.True);
        }
    }
}
