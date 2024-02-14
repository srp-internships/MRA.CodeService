using Application.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Identity
{
    public class AdminIdentityService : IAdminIdentityService
    {
        IConfiguration configurationService;
        public AdminIdentityService(IConfiguration configuration)
        {
            configurationService = configuration;
        }

        public Task<AuthenticationResponse> Authenticate(IDictionary<string, StringValues> headers)
        {
            AuthenticationResponse authenticationResponse = new AuthenticationResponse();
            if (headers.TryGetValue(IdentityService.API_KEY_NAME, out StringValues secretKey))
            {
                if (TryGetSecretKey(out string apiKey) && apiKey.Equals(secretKey))
                {
                    authenticationResponse.Success = true;
                }
                else
                {
                    authenticationResponse.Error = "Invalid api key";
                }
            }
            else
            {
                authenticationResponse.Error = $"'{IdentityService.API_KEY_NAME}' is not provided";
            }
            return Task.FromResult(authenticationResponse);
        }

        bool TryGetSecretKey(out string secretKey)
        {
            secretKey = string.Empty;
            try
            {
                secretKey = configurationService.GetValue<string>(IdentityService.API_KEY_NAME);
                return !string.IsNullOrEmpty(secretKey);
            }
            catch
            {
                return false;
            }
        }
    }
}
