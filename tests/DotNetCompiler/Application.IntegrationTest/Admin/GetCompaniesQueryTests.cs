using Application.Admin.Queries;
using Domain.Entities;

namespace Application.IntegrationTest.Admin
{
    public class GetCompaniesQueryTests
    {
        [Test]
        public async Task GetCompanies_ShouldReturnCreatedCompanyTest()
        {
            var newCompanyWithKey = new Company()
            {
                Name = "TEST",
                Host = "TEST",
                ApiKey = new ApiKey
                {
                    SecretKey = Guid.NewGuid().ToString()
                }
            };
            await TestHelper.AddAsync(newCompanyWithKey);

            var companies = await TestHelper.SendAsync(new GetCompaniesQuery());
            Assert.That(companies.Any(), Is.True);
            Assert.That(companies.Any(s => s.Apikey == newCompanyWithKey.ApiKey.SecretKey), Is.True);
        }
    }
}
