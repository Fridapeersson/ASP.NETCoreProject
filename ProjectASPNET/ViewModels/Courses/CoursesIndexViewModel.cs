using Infrastructure.Entities;
using Infrastructure.Models.Courses;
using Infrastructure.Models.Dtos;
using ProjectASPNET.ViewModels.Account;


namespace ProjectASPNET.ViewModels.Courses;

public class CoursesIndexViewModel
{
    public IEnumerable<CourseViewModel> Courses { get; set; } = null!;
    public IEnumerable<CategoryViewModel> Categories { get; set; } = null!;
    public AuthorViewModel Author { get; set; } = null!;
    public AccountDetailsViewModel AccountDetails { get; set; } = null!;
    public string? SearchQuery { get; set; }

    public PaginationModel? Pagination { get; set; }

}


