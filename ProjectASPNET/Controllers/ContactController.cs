using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Contact;
using System.Diagnostics;

namespace ProjectASPNET.Controllers;

public class ContactController : Controller
{
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    public ContactController(IConfiguration config, HttpClient http)
    {
        _config = config;
        _http = http;
    }


    [HttpGet]
    [Route("/Contact")]
    public IActionResult Index()
    {
        try
        {
            var viewModel = new ContactFormViewModel();

            return View(viewModel);
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactFormViewModel viewModel)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var contactDto = viewModel.Contact;
                var content = new StringContent(JsonConvert.SerializeObject(contactDto), System.Text.Encoding.UTF8, "application/json");
                var response = await _http.PostAsync($"https://localhost:7107/api/Contact?key={_config["ApiKey:Secret"]}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "Success";
                }                
                return View(viewModel);
            }
            else
            {
                TempData["Status"] = "Error";
            }
        }
        catch 
        { 
            TempData["Status"] = "ConnectionFailed";
        }
        return View(viewModel);
    }
}