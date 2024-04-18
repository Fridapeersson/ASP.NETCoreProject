using Infrastructure.Models.Courses;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectASPNET.ViewModels.Courses;
using System.Diagnostics;
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

    public CoursesController(HttpClient http, IConfiguration config, CategoryService categoryService, CoursesService coursesService)
    {
        _http = http;
        _config = config;
        _categoryService = categoryService;
        _coursesService = coursesService;
    }

    [HttpGet]
    [Route("/courses")]
    public async Task<IActionResult> Courses(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 9)
    {
        var courseApiUrl = $"https://localhost:7107/api/courses?key={_config["ApiKey:Secret"]}&category={HttpUtility.UrlEncode(category)}&searchQuery={HttpUtility.UrlEncode(searchQuery)}&pageNumber={pageNumber}&pageSize={pageSize}";

        try
        {
            if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var courseResponse = await _http.GetAsync(courseApiUrl);
            if (courseResponse.IsSuccessStatusCode)
            {
                var savedCourses = await _coursesService.GetAllSavedCourses(User);
                var savedCoursesIds = savedCourses.Select(x => x.CourseId).ToList();

                var jsonCourses = await courseResponse.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<CourseResult>(jsonCourses);

                var coursesViewModel = courses!.Courses!.Select(x => new CourseViewModel
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
                        AuthorDescription = x.Author.AuthorDescription,
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
                        TotalPages = courses.TotalPages,
                        PageSize = pageSize,
                        TotalItems = courses.TotalItems,

                    }
                };

                return View(viewModel);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }

        return null!;
    }

    [HttpGet]
    public async Task<IActionResult> CourseDetails(int id)
    {
        var response = await _http.GetAsync($"https://localhost:7107/api/courses/{id}?key={_config["ApiKey:Secret"]}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var viewModel = JsonConvert.DeserializeObject<CourseViewModel>(json);

            return View(viewModel);
        }
        if (HttpContext.Request.Cookies.TryGetValue("AccessToken", out var token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }
        return View();
    }
}