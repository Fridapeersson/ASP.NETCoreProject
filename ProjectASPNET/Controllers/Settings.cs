using Microsoft.AspNetCore.Mvc;

namespace ProjectASPNET.Controllers;

public class Settings : Controller
{
    public IActionResult ChangeTheme(string theme)
    {
        //bake a cookie
        var cookieOption = new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMonths(1),
        };
        Response.Cookies.Append("ThemeMode", theme, cookieOption);

        return Ok();
    }
}
