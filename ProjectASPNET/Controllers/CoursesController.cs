using Infrastructure.Entities;
using Infrastructure.Models.Courses;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Courses;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;


namespace ProjectASPNET.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly CategoryService _categoryService;
    private readonly CoursesService _coursesService;

    //private readonly string _apiKey = "?key=JDJ5JDEwJGkzNzU5S0YvQnp3bTBzQ09rV2JMOC40UVdZWHZ6NkFvZm05ckFUS05mMXpPamc3T1M0NVlH";

    public CoursesController(HttpClient http, IConfiguration config, CategoryService categoryService, CoursesService coursesService)
    {
        _http = http;
        _config = config;
        _categoryService = categoryService;
        _coursesService = coursesService;
    }

    //[Route("/courses")]
    //public async Task<IActionResult> Courses(string category = "")
    //{
    //    var courseApiUrl = $"https://localhost:7107/api/courses?key={_config["ApiKey:Secret"]}&category={HttpUtility.UrlEncode(category)}";

    //    if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
    //    {
    //        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //        var courseResponse = await _http.GetAsync(courseApiUrl);
    //        if (courseResponse.IsSuccessStatusCode)
    //        {
    //            //var jsonCourses = await courseResponse.Content.ReadAsStringAsync();
    //            //var courses = JsonConvert.DeserializeObject<List<CourseViewModel>>(jsonCourses);
    //            var courses = await _coursesService.GetAllAsync();
    //            if (courses != null)
    //            {
    //                var savedCourses = await _coursesService.GetAllSavedCourses(User);
    //                var savedCoursesIds = savedCourses.Select(x => x.CourseId);

    //                var coursesViewModel = courses.Select(x => new CourseViewModel
    //                {
    //                    Id = x.Id,
    //                    Title = x.Title,
    //                    Price = x.Price,
    //                    DiscountPrice = x.DiscountPrice,
    //                    BackgroundImageName = x.BackgroundImageName!,
    //                    LikesInNumbers = x.LikesInNumbers,
    //                    LikesInPercent = x.LikesinPercent,
    //                    IsBestSeller = x.IsBestSeller,
    //                    HoursToComplete = x.HoursToComplete,
    //                    IsSaved = savedCoursesIds.Contains(x.Id),
    //                    Author = new AuthorViewModel
    //                    {
    //                        Id = x.Id,
    //                        AuthorTitle = x.Author.AuthorTitle,
    //                        AuthorName = x.Author.AuthorName,
    //                        AuthorDescritpion = x.Author.AuthorDescription,
    //                        AuthorImageUrl = x.Author.AuthorImageUrl,
    //                        FacebookSubs = x.Author.FacebookSubs,
    //                        YoutubeSubs = x.Author.YoutubeSubs,
    //                    }
    //                }).ToList();

    //                //foreach (var course in courses)
    //                //{
    //                //    course.IsSaved = savedCoursesIds.Contains(course.Id);
    //                //}


    //                var categoryEntities = await _categoryService.GetCategoriesAsync();
    //                var categoryViewModel = categoryEntities.Select(x => new CategoryViewModel
    //                {
    //                    Id = x.Id,
    //                    CategoryName = x.CategoryName,
    //                });

    //                var viewModel = new CoursesIndexViewModel
    //                {
    //                    Categories = categoryViewModel,
    //                    Courses = coursesViewModel,
    //                };

    //                return View(viewModel);
    //            }
    //        }
    //    }
    //    return View(new CoursesIndexViewModel());
    //}


    [Route("/courses")]
    public async Task<IActionResult> Courses(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 9)
    {
        var savedCourses = await _coursesService.GetAllSavedCourses(User);
        var savedCoursesIds = savedCourses.Select(x => x.CourseId).ToList();

        var courseResult = await _coursesService.GetCoursesAsync(category, searchQuery, pageNumber, pageSize);

        var coursesViewModel = courseResult.Courses!.Select(x => new CourseViewModel
        {
            Id = x.Id,
            Title = x.Title,
            Price = x.Price,
            DiscountPrice = x.DiscountPrice,
            BackgroundImageName = x.BackgroundImageName!,
            LikesInNumbers = x.LikesInNumbers,
            LikesInPercent = x.LikesinPercent,
            IsBestSeller = x.IsBestSeller,
            HoursToComplete = x.HoursToComplete,
            IsSaved = savedCoursesIds.Contains(x.Id), 
            Author = new AuthorViewModel
            {
                Id = x.Id,
                AuthorTitle = x.Author.AuthorTitle,
                AuthorName = x.Author.AuthorName,
                AuthorDescritpion = x.Author.AuthorDescription,
                AuthorImageUrl = x.Author.AuthorImageUrl,
                FacebookSubs = x.Author.FacebookSubs,
                YoutubeSubs = x.Author.YoutubeSubs,
            }
        }).ToList();

        var categoryEntities = await _categoryService.GetCategoriesAsync();
        var categoryViewModel = categoryEntities.Select(x => new CategoryViewModel
        {
            Id = x.Id,
            CategoryName = x.CategoryName,
        });


        var viewModel = new CoursesIndexViewModel
        {
            Categories = categoryViewModel,
            Courses = coursesViewModel,
            SearchQuery = searchQuery,
            Pagination = new PaginationModel
            {
                CurrentPage = pageNumber,
                TotalPages = courseResult.TotalPages,
                PageSize = pageSize,
                TotalItems = courseResult.TotalItems,
            }
            //CurrentPage = pageNumber,
            //PageSize = pageSize,
            //TotalItems = courses.TotalItems,
            //TotalPages = courses.TotalPages,

};

        return View(viewModel);
    }

    //[Route("/courses")]
    //public async Task<IActionResult> Courses(/*string searchQuery, int? categoryId*/)
    //{
    //    var courseApiUrl = $"https://localhost:7107/api/courses?key={_config["ApiKey:Secret"]}";

    //    if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
    //    {
    //        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    //        var courseResponse = await _http.GetAsync(courseApiUrl);
    //        if (courseResponse.IsSuccessStatusCode)
    //        {
    //            var jsonCourses = await courseResponse.Content.ReadAsStringAsync();
    //            var coursesList = JsonConvert.DeserializeObject<List<CourseViewModel>>(jsonCourses);
    //            if (coursesList != null)
    //            {
    //                // Ingen kontroll av sparade kurser, visa bara listan av alla kurser som de är
    //                var viewModel = new CoursesIndexViewModel
    //                {
    //                    Courses = coursesList,
    //                };

    //                return View(viewModel);
    //            }
    //        }
    //    }
    //    return View(new CoursesIndexViewModel()); // Returnera en tom modell om inga kurser hämtades eller om ett fel uppstod
    //}


    public async Task<IActionResult> CourseDetails(int id)
    {
        if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync($"https://localhost:7107/api/courses/{id}?key={_config["ApiKey:Secret"]}"); /*{_apiKey}*/

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<CourseViewModel>(json);


                return View(viewModel);
            }
        }
        return View();
    }





}