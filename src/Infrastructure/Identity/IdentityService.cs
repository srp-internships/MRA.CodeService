using Application.Identity;
using Application.Repositories;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        public const string API_KEY_NAME = "API_KEY";

        IApplicationDbContext _dbContext;
        public IdentityService(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public async Task<AuthenticationResponse> Authenticate(IDictionary<string, StringValues> headers)
        {
            AuthenticationResponse authenticationResponse = new();
            if (headers.TryGetValue(API_KEY_NAME, out StringValues secretKey))
            {
                var secretKeyFromDB = await _dbContext.GetApiKeyAsync(secretKey.ToString());
                if (secretKeyFromDB == null)
                {
                    authenticationResponse.Error = "Invalid api key";
                }
                else
                {
                    authenticationResponse.Success = true;
                }
            }
            else
            {
                authenticationResponse.Error = $"'{API_KEY_NAME}' is not provided";
            }
            return authenticationResponse;
        }
    }
}
