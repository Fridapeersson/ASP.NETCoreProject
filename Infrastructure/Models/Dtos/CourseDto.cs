using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class CourseDto
{
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesInPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string? BackgroundImageName { get; set; }
    //public string? CategoryName { get; set; }


    [Required]
    public AuthorDto Author { get; set; } = null!;

    [Required]
    public CategoryDto Category { get; set; } = null!;

    
}
