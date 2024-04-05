using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

//[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
[AttributeUsage(AttributeTargets.All)]
public class UseApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetService<IConfiguration>(); //IConfiguration kommer hämta in appsettings.json
        var secret = config?["ApiKey:Secret"]; //hämmtar in secret från appsettings

        if (!string.IsNullOrEmpty(secret) && context.HttpContext.Request.Query.TryGetValue("key", out var key))
        {
            //kollar så nyckeln vi fick in via queryn inte är tom och lika med secreten
            if (!string.IsNullOrEmpty(key) && secret == key)
            {
                await next();
                return;
            }
        }

        context.Result = new UnauthorizedResult();

        //await next(); //låter systemet gå vidare
    }
}
