namespace Infrastructure.Models.Dtos;

public class CourseDtoWithId
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesInPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string? BackgroundImageName { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }

    public AuthorDtoWithId? Author { get; set; }
    public CategoryDtoWithId? Category { get; set; }
    public bool IsSaved { get; set; } 

}
