using Infrastructure.Entities;

namespace Infrastructure.Models.Courses;

public class CourseResult
{
    public IEnumerable<CoursesEntity>? Courses { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
