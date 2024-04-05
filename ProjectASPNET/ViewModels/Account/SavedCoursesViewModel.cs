using Infrastructure.Models.Dtos;
using ProjectASPNET.ViewModels.Courses;

namespace ProjectASPNET.ViewModels.Account;

public class SavedCoursesViewModel
{
    public ProfileInfoViewModel? ProfileInfo { get; set; }
    public List<CourseViewModel>? SavedCourses { get; set; }
}
