using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Components;
using ProjectASPNET.ViewModels.Home;
using System.Net;

namespace ProjectASPNET.Controllers;

public class HomeController : Controller
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public HomeController(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }


    [Route("/")]
    public IActionResult Index()
    {
        //if(HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        //{
            
        //}

        var viewModel = new HomeIndexViewModel
        {
            Showcase = new ShowcaseViewModel
            {
                Id = "showcase",
                Image = new()
                {
                    ImageUrl = "./images/showcase-taskmanager.svg",
                    AltText = "Showcase img"
                },
                Title = "Task Management Assistant You Gonna Love",
                Description = "We offer you a new generation of task management system. Plan, manage & track all your tasks in one flexible tool.",
                Link = new()
                {
                    ControllerName = "Downloads",
                    ActionName = "Index",
                    Text = "Get started for free"
                },
                BrandsText = "Largest companies use our tool to work efficiently",
                BrandImages =
                [
                    new() { ImageUrl = "./images/brands/brand_1.svg", AltText = "Brand 1" },
                    new() { ImageUrl = "./images/brands/brand_2.svg", AltText = "Brand 2" },
                    new() { ImageUrl = "./images/brands/brand_3.svg", AltText = "Brand 3" },
                    new() { ImageUrl = "./images/brands/brand_4.svg", AltText = "Brand 4" }
                ]
            },

            Features = new FeaturesViewModel
            {
                Id = "features",
                Title = "What Do You Get with Our Tool?",
                Description = "Make sure all your tasks are organized so you can set the priorities and focus on important.",
                FeaturesBox = new HashSet<FeaturesBoxViewModel>()
                {
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/chat.svg", AltText = "Chat-icon" },
                        FeaturesTitle = "Comments on Tasks",
                        FeaturesDescription = "Id mollis consectetur congue egestas egestas suspendisse blandit justo."
                    },
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/presentation.svg", AltText = "Presentation-icon" },
                        FeaturesTitle = "Tasks Analytics",
                        FeaturesDescription = "Non imperdiet facilisis nulla tellus Morbi scelerisque eget adipiscing vulputate."
                    },
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/add-group.svg", AltText = "add-group-icon" },
                        FeaturesTitle = "Multiple Assignees",
                        FeaturesDescription = "A elementum, imperdiet enim, pretium etiam facilisi in aenean quam mauris."
                    },
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/bell.svg", AltText = "Bell-icon" },
                        FeaturesTitle = "Notifications",
                        FeaturesDescription = "Diam, suspendisse velit cras ac. Lobortis diam volutpat, eget pellentesque viverra."
                    },
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/tests.svg", AltText = "test-icon" },
                        FeaturesTitle = "Sections & Subtasks",
                        FeaturesDescription = "Mi feugiat hac id in. Sit elit placerat lacus nibh lorem ridiculus lectus."
                    },
                    new()
                    {
                        FeaturesImg = new() { ImageUrl = "./images/Home/features/shield.svg", AltText = "shield-icon" },
                        FeaturesTitle = "Data Security",
                        FeaturesDescription = "Aliquam malesuada neque eget elit nulla vestibulum nunc cras."
                    },
                }
            },

            Subscriber = new SubscribeViewModel()

        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(SubscribeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(viewModel.SubscribeModel), System.Text.Encoding.UTF8, "application/json");
                
                var response = await _http.PostAsync($"https://localhost:7107/api/subscribers?key={_config["ApiKey:Secret"]}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Status"] = "Success";
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["Status"] = "AlreadyExists";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["Status"] = "Unauthorized";
                }
                else
                {
                    TempData["Status"] = "ConnectionFailed";
                }
            }
            catch
            {
                TempData["Status"] = "ConnectionFailed";
            }
        }
        else
        {
            TempData["Status"] = "Invalid";
        }

        return RedirectToAction("Index", "Home");
    }


    [Route("/error")]
    public IActionResult Error404(int statusCode) => View();


    [Route("/denied")]
    public IActionResult AccessDenied(int statusCode) => View();
}
