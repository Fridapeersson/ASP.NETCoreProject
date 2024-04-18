using Azure;
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
    private readonly CoursesService _coursesService;

    public ContactController(IConfiguration config, HttpClient http, CoursesService coursesService)
    {
        _config = config;
        _http = http;
        _coursesService = coursesService;
    }


    [HttpGet]
    [Route("/Contact")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = new ContactFormViewModel();

            //viewModel.Contact = new();
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
                //else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                //{
                //    TempData["Status"] = "AlreadyExists";
                //}
                //else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    TempData["Status"] = "Unauthorized";
                //}
                
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
        //return RedirectToAction("Contact");
        return View(viewModel);
    }
}
//[HttpPost]
//public async Task<IActionResult> Subscribe(SubscribeViewModel viewModel)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            var content = new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json");
//            var response = await _http.PostAsync($"https://localhost:7107/api/subscribers?key={_config["ApiKey:Secret"]}", content);

//            if (response.IsSuccessStatusCode)
//            {
//                TempData["Status"] = "Success";
//            }
//            else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
//            {
//                TempData["Status"] = "AlreadyExists";
//            }
//            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
//            {
//                TempData["Status"] = "Unauthorized";
//            }
//        }
//        catch
//        {
//            TempData["Status"] = "ConnectionFailed";
//        }
//    }
//    else
//    {
//        TempData["Status"] = "Invalid";
//    }
//    //aint pretty
//    return RedirectToAction("Index", "Home");
//    //return View(viewModel);
//}