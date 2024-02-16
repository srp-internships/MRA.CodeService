using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Identity
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> Authenticate(IDictionary<string, StringValues> headers);
    }
}
