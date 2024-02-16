using Application.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnAuthenticationActionExecutionAsync(context, next);
        }

        protected virtual async Task OnAuthenticationActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identityService = context.HttpContext.RequestServices.GetRequiredService<IIdentityService>();
            await OnAuthenticationActionExecutionAsync(identityService, context, next);
        }

        protected async Task OnAuthenticationActionExecutionAsync(IIdentityService identityService, ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authenticateResponse = await identityService.Authenticate(context.HttpContext.Request.Headers);
            if (!authenticateResponse.Success)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = JsonConvert.SerializeObject(authenticateResponse, Formatting.Indented),
                    ContentType = "application/json"
                };
                return;
            }
            await next();
        }
    }
}
