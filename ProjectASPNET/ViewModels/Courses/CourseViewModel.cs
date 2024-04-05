namespace ProjectASPNET.ViewModels.Courses;

public class CourseViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesInPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string BackgroundImageName { get; set; } = null!;
    public AuthorViewModel Author { get; set; } = null!;
    public CategoryViewModel Category { get; set; } = null!;
    public bool IsSaved { get; set; }
}
