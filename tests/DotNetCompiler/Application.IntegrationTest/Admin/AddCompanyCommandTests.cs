using Application.DotNetCodeAnalyzer.Commands;
using Core.Exceptions;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin
{
    public class AddCompanyCommandTests
    {
        [Test]
        public void AddCompanyCommand_ShouldReturnError_WhenCompanyNameIsEmptyTest()
        {
            AddCompanyCommand addCompanyCommand = new() { Host = "srp.tj" };

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(addCompanyCommand));

            var companyNameIsEmptyErrorWasShown = IsErrorExists("CompanyName", "'Company Name' must not be empty.", validationError);

            Assert.That(companyNameIsEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void AddCompanyCommand_ShouldReturnError_WhenHostIsEmptyTest()
        {
            AddCompanyCommand addCompanyCommand = new() { CompanyName = "SRP" };

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(addCompanyCommand));

            var hostIsEmptyErrorWasShown = IsErrorExists("Host", "'Host' must not be empty.", validationError);
            Assert.That(hostIsEmptyErrorWasShown, Is.True);
        }
    }
}
