using Infrastructure.Entities;
using Infrastructure.Models.Dtos;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CoursesService _coursesService;
    private readonly CoursesRepository _coursesRepository;
    private readonly UserManager<UserEntity> _userManager;

    public CoursesController(CoursesService coursesService, UserManager<UserEntity> userManager, CoursesRepository coursesRepository)
    {
        _coursesService = coursesService;
        _userManager = userManager;
        _coursesRepository = coursesRepository;
    }


    #region CREATE COURSE
    [HttpPost]
    public async Task<IActionResult> CreateCourse(CoursesEntity courseEntity)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var result = await _coursesService.CreateCourseAsync(courseEntity);

                if (result)
                {
                    return Created("created", null);
                }
                else
                {
                    return Conflict("Course already exists");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            // Log the exception as needed.
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    #endregion

    #region GET ONE COURSE
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneCourse(int id)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var courseDto = await _coursesRepository.GetOneAsync(x => x.Id == id);
                if (courseDto != null)
                {
                    return Ok(courseDto);
                }
            }
            return NotFound();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
    #endregion


    #region GET ALL COURSES
    [HttpGet]
    public async Task<IActionResult> GetAllCourses(string category = "", string searchQuery = "")
    {
        try
        {
            if (ModelState.IsValid)
            {
                var courses = await _coursesRepository.GetCoursesAsync(category, searchQuery);
                var query = courses.AsQueryable();

                if (!string.IsNullOrEmpty(category) && category != "all")
                {
                    query = query.Where(x => x.Category!.CategoryName == category);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(x => x.Title.Contains(searchQuery) || x.Author.AuthorName.Contains(searchQuery));
                }

                query = query.OrderByDescending(x => x.Id);

                if (courses != null)
                {
                    return Ok(courses);
                }
                return NotFound();
            }

        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return BadRequest();
    }
    #endregion




    #region UPDATE COURSE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, CoursesEntity courseEntity)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _coursesService.UpdateCourseAsync(id, courseEntity);
            if (course)
            {
                return Ok("Course updated successfully.");
            }
            else
            {
                return NotFound("Course not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Problem("An error occurred while updating the course. Please try again later.");
        }
    }
    #endregion

    #region DELETE COURSE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var courseEntity = await _coursesRepository.GetOneAsync(x => x.Id == id);
                if (courseEntity != null)
                {
                    var result = await _coursesRepository.DeleteAsync(x => x.Id == courseEntity.Id);
                    if (result)
                    {
                        return Ok("Deleted course successfully");
                    }
                }

                return NotFound(courseEntity);
            }
            return BadRequest();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return Problem();
    }
    #endregion


    //#region REMOVE ALL SAVED COURSES
    //[HttpDelete("RemoveAllSavedCourses/{userId}")]
    //public async Task<IActionResult> RemoveAllSavedCourses(string userId)
    //{
    //    try
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var result = await _coursesService.RemoveAllSavedCoursesAsync(userId);
    //            if (result)
    //            {
    //                return Ok("All saved courses removed successfully");
    //            }
    //            else
    //            {
    //                return NotFound("No saved courses found for the specified user.");
    //            }
    //        }
    //        return BadRequest("Invalid model state");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex);
    //        return Problem("An error occurred while removing saved courses. Please try again later.");
    //    }
    //}
    //#endregion
}
//try
//{

//}
//catch (Exception ex) { Debug.WriteLine(ex); }