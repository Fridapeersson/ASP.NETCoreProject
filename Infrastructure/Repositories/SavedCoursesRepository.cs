using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class SavedCoursesRepository : BaseRepo<SavedCoursesEntity>
{
    private readonly DataContext _context;

    public SavedCoursesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<int>> GetSavedCourseIdsForUserAsync(string userId)
    {
        try
        {
            var savedCourseIds = await _context.SavedCourses
                .Where(x => x.UserId == userId)
                .Select(x => x.CourseId)
                .ToListAsync();

            return savedCourseIds;
        }
        catch(Exception ex) { Debug.WriteLine(ex); }
        return [];
    }

    public async Task<bool> IsCourseSavedAsync(string userId, int courseId)
    {
        return await _context.SavedCourses.AnyAsync(x => x.UserId == userId && x.CourseId == courseId);
    }
}
