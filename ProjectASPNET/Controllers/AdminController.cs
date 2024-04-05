using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectASPNET.Controllers;


[Authorize(Policy = "Admins")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Policy = "CIO")] //vilka som har tillågng till settings, anges i program-delen
    public IActionResult Settings()
    {
        return View();
    }
}
