using Application.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class AdminApiKeyAttribute : ApiKeyAttribute, IAsyncActionFilter
    {
        protected override async Task OnAuthenticationActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identityService = context.HttpContext.RequestServices.GetRequiredService<IAdminIdentityService>();
            await OnAuthenticationActionExecutionAsync(identityService, context, next);
        }
    }
}
