using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class UpdateCourseDto
{
    [Required]
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesInPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string? BackgroundImageName { get; set; }
    //public string? CategoryName { get; set; }

    public UpdateCategoryDto? Category { get; set; }
    public UpdateAuthorDto? Author { get; set; }
}
