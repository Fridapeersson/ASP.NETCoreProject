using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace ProjectASPNET.Helpers.Middlewares;

public class UserSessionValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserSessionValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    {
        if(context.User.Identity!.IsAuthenticated)
        {
            var user = await userManager.GetUserAsync(context.User);
            if(user == null)
            {
                await signInManager.SignOutAsync();

                if (!IsAjaxRequest(context.Request) && context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    var signInPath = "/signin";
                    context.Response.Redirect(signInPath);
                    return;
                }
            }
        }
        await _next(context); //kör man inte next kommer middleware fastna här
    }

    //ajax - asyncronous javascript and xml
    private static bool IsAjaxRequest (HttpRequest request)
    {
        return request.Headers.XRequestedWith == "XMLHttpRequest"; 
    }
}
