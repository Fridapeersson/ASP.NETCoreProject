using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CoursesEntity
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesinPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; } 
    public string? BackgroundImageName { get; set; }
    

    public int CategoryId { get; set; }
    public virtual CategoryEntity? Category { get; set; }



    public int AuthorId { get; set; }
    public virtual AuthorsEntity Author { get; set; } = null!;


    public ICollection<SavedCoursesEntity>? SavedCourses { get; set; }
}
