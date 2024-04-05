using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models;

public class CoursesModel
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesinPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; } = false;
    public string? BackgroundImageName { get; set; }

    public AuthorsModel Authors { get; set; } = null!;

}



//public int? AuthorId { get; set; }
//public int Id { get; set; }
