using Infrastructure.Entities;
using Infrastructure.Models.Courses;
using Infrastructure.Models.Dtos;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services;

public class CoursesService
{
    private readonly CoursesRepository _coursesRepository;
    private readonly AuthorRepository _authorRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly SavedCoursesRepository _savedCourseRepository;

    private readonly HttpClient _http;
    private readonly IConfiguration _config;



    private readonly UserManager<UserEntity> _userManager;


    public CoursesService(CoursesRepository coursesRepository, AuthorRepository authorRepository, SavedCoursesRepository savedCourseRepository, UserManager<UserEntity> userManager, CategoryRepository categoryRepository, HttpClient http, IConfiguration config)
    {
        _coursesRepository = coursesRepository;
        _authorRepository = authorRepository;
        _savedCourseRepository = savedCourseRepository;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _http = http;
        _config = config;
    }


    public async Task<bool> CreateCourseAsync(CourseDto coursedto)
    {
        try
        {
            var courseEntity = new CoursesEntity
            {
                Title = coursedto.Title,
                Price = coursedto.Price,
                DiscountPrice = coursedto.DiscountPrice,
                HoursToComplete = coursedto.HoursToComplete,
                LikesInNumbers = coursedto.LikesInNumbers,
                LikesinPercent = coursedto.LikesInPercent,
                IsBestSeller = coursedto.IsBestSeller,
                BackgroundImageName = coursedto.BackgroundImageName,
            };

            var courseExists = await _coursesRepository.ExistsAsync(x => x.Title == coursedto.Title);
            if (courseExists)
            {
                return false;
            }

            // Kontrollera och hantera författare
            var authorExists = await _authorRepository.ExistsAsync(x => x.AuthorName == coursedto.Author.AuthorName);
            if (authorExists)
            {
                var existingAuthor = await _authorRepository.GetOneAsync(x => x.AuthorName == coursedto.Author.AuthorName);
                courseEntity.AuthorId = existingAuthor.Id; 
            }
            else
            {
                var newAuthor = new AuthorsEntity
                {
                    AuthorName = coursedto.Author.AuthorName,
                    AuthorTitle = coursedto.Author.AuthorTitle!,
                    AuthorDescription = coursedto.Author.AuthorDescritpion,
                    AuthorImageUrl = coursedto.Author.AuthorImageUrl,
                    FacebookSubs = coursedto.Author.FacebookSubs,
                    YoutubeSubs = coursedto.Author.YoutubeSubs,
                };
                var createdAuthor = await _authorRepository.CreateOneAsync(newAuthor);
                courseEntity.AuthorId = createdAuthor.Id; 
            }

            // Kontrollera och hantera kategori
            var categoryExists = await _categoryRepository.ExistsAsync(x => x.CategoryName == coursedto.Category.CategoryName);
            if (categoryExists)
            {
                var existingCategory = await _categoryRepository.GetOneAsync(x => x.CategoryName == coursedto.Category.CategoryName);
                courseEntity.CategoryId = existingCategory.Id;
            }
            else
            {
                var newCategory = new CategoryEntity
                {
                    CategoryName = coursedto.Category.CategoryName!
                };
                var createdCategory = await _categoryRepository.CreateOneAsync(newCategory);
                courseEntity.CategoryId = createdCategory.Id;
            }

            // Skapa kursen
            var createCourse = await _coursesRepository.CreateOneAsync(courseEntity);
            return createCourse != null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public async Task<CoursesEntity> GetOneCourseAsync(int id)
    {
        try
        {
            var courseEntity = await _coursesRepository.GetOneAsync(x => x.Id == id);
            if (courseEntity != null)
            {
                return courseEntity;
                //var result = ConvertEntityToDto(courseEntity);
                //if (result != null)
                //{
                //    return result;
                //}

            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    public async Task<IEnumerable<CoursesEntity>> GetAllAsync()
    {
        try
        {
            var courses = await _coursesRepository.GetAllAsync();
            if (courses != null)
            {
                return courses;
                //var courseDtos = courses.Select(x => ConvertEntityToDto(x)).ToList();
                //return courseDtos;
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return [];
    }






    public async Task<CourseResult> GetCoursesAsync(string category, string searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _coursesRepository.GetCoursesAsync();
            //var query = courses.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                query = query.Where(x => x.Category!.CategoryName.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                //if(searchQuery.Equals("undefined"))
                //{
                //    searchQuery = "";
                //}
                query = query.Where(x => x.Title.Contains(searchQuery) || x.Author.AuthorName!.Contains(searchQuery));
            }
            int totalItems = await query.CountAsync();
            List<CoursesEntity> courseList = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new CourseResult
            {
                Courses = courseList,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            //return await query.ToListAsync();


            //return await _coursesRepository.GetCoursesAsync(category, searchQuery, pageNumber, pageSize);
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }




    //public async Task<IEnumerable<CoursesEntity>> GetCoursesAsync(string category = "")
    //{
    //    try
    //    {
    //        var response = await _http.GetAsync($"{_config["ApiUris:Courses"]}?category={category}");
    //        if (response.IsSuccessStatusCode)
    //        {

    //            var courses = JsonConvert.DeserializeObject<IEnumerable<CoursesEntity>>(await response.Content.ReadAsStringAsync());

    //            if (courses != null)
    //            {
    //                return courses;
    //            }
    //        }
    //    }
    //    catch (Exception ex) { Debug.WriteLine(ex); }
    //    return null!;

    //}


    public async Task<bool> UpdateCourseAsync(int courseId, CoursesEntity courseToBeUpdated)
    {
        try
        {
            var courseEntity = await _coursesRepository.GetOneAsync(x => x.Id == courseId);
            if (courseEntity == null)
            {
                return false;
            }

            var existingCourseWithTitle = await _coursesRepository.GetOneAsync(x => x.Title == courseToBeUpdated.Title && x.Id != courseId);
            if (existingCourseWithTitle != null) return false;

            // author
            var existingAuthor = await _authorRepository.GetOneAsync(x => x.AuthorName == courseToBeUpdated.Author.AuthorName);
            if (existingAuthor != null)
            {
                courseEntity.AuthorId = existingAuthor.Id;
            }
            else
            {
                var newAuthor = new AuthorsEntity
                {
                    AuthorName = courseToBeUpdated.Author.AuthorName!,
                    AuthorDescription = courseToBeUpdated.Author.AuthorDescription,
                    AuthorTitle = courseToBeUpdated.Author.AuthorTitle!,
                    AuthorImageUrl = courseToBeUpdated.Author.AuthorImageUrl,
                    FacebookSubs = courseToBeUpdated.Author.FacebookSubs,
                    YoutubeSubs = courseToBeUpdated.Author.YoutubeSubs,
                };
                var createdAuthor = await _authorRepository.CreateOneAsync(newAuthor);
                courseEntity.AuthorId = createdAuthor.Id;
            }

            // Category
            var existingCategory = await _categoryRepository.GetOneAsync(x => x.CategoryName == courseToBeUpdated.Category!.CategoryName);
            if (existingCategory != null)
            {
                courseEntity.CategoryId = existingCategory.Id;
            }
            else
            {
                var newCategory = new CategoryEntity
                {
                    CategoryName = courseToBeUpdated.Category!.CategoryName
                };
                var createdCategory = await _categoryRepository.CreateOneAsync(newCategory);
                courseEntity.CategoryId = createdCategory.Id;
            }

            courseEntity.Title = courseToBeUpdated.Title;
            courseEntity.Price = courseToBeUpdated.Price;
            courseEntity.DiscountPrice = courseToBeUpdated.DiscountPrice;
            courseEntity.HoursToComplete = courseToBeUpdated.HoursToComplete;
            courseEntity.LikesInNumbers = courseToBeUpdated.LikesInNumbers;
            courseEntity.LikesinPercent = courseToBeUpdated.LikesinPercent;
            courseEntity.IsBestSeller = courseToBeUpdated.IsBestSeller;
            courseEntity.BackgroundImageName = courseToBeUpdated.BackgroundImageName;

            await _coursesRepository.UpdateAsync(x => x.Id == courseId, courseEntity);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> DeleteCourseAsync(Expression<Func<CoursesEntity, bool>> predicate)
    {
        try
        {
            var exists = await _coursesRepository.ExistsAsync(predicate);
            if (exists)
            {
                await _coursesRepository.DeleteAsync(predicate);
                return true;
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }



    public async Task<bool> SaveCourseToProfile(string userId, int courseId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var isCourseSaved = await _savedCourseRepository.ExistsAsync(x => x.UserId == userId && x.CourseId == courseId);
                if (isCourseSaved)
                {
                    return true;
                }
                else
                {
                    var savedCourseEntity = new SavedCoursesEntity
                    {
                        UserId = userId,
                        CourseId = courseId
                    };
                    var result = await _savedCourseRepository.CreateOneAsync(savedCourseEntity);
                    if (result != null)
                    {
                        return true;
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    public async Task<bool> RemoveCourseFromProfileAsync(string userId, int courseId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var isCourseSaved = await _savedCourseRepository.ExistsAsync(x => x.UserId == userId && x.CourseId == courseId);

                if (isCourseSaved)
                {
                    var result = await _savedCourseRepository.DeleteAsync(x => x.UserId == userId && x.CourseId == courseId);
                    if (result)
                    {
                        return true;
                    }
                }

            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    public async Task<bool> RemoveAllSavedCoursesAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var savedCourses = await _savedCourseRepository.GetAllAsync();

                if (savedCourses != null && savedCourses.Any())
                {
                    var userSavedCourses = savedCourses.Where(x => x.UserId == userId).ToList();
                    if (userSavedCourses.Any())
                    {
                        foreach (var course in userSavedCourses)
                        {
                            await _savedCourseRepository.DeleteAsync(x => x.UserId == course.UserId);
                        }
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return false;
    }

    public async Task<IEnumerable<SavedCoursesEntity>> GetAllSavedCourses(ClaimsPrincipal loggedInUser)
    {
        try
        {
            var user = await _userManager.GetUserAsync(loggedInUser);
            if (user != null)
            {
                var savedCoursesIds = await _savedCourseRepository.GetSavedCourseIdsForUserAsync(user.Id);
                if (savedCoursesIds != null)
                {
                    var savedCourses = new List<SavedCoursesEntity>();

                    foreach (var courseId in savedCoursesIds)
                    {
                        savedCourses.Add(new SavedCoursesEntity
                        {
                            UserId = user.Id,
                            CourseId = courseId
                        });
                    }
                    return savedCourses;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return new List<SavedCoursesEntity>();
    }
}