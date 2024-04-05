using Infrastructure.Entities;
using System.Diagnostics;

namespace Infrastructure.Models.Courses;

public class CourseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesinPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string BackgroundImageName { get; set; } = null!;

    public CategoryModel Category { get; set; } = null!;
    public AuthorModel Author { get; set; } = null!;
    public bool IsSaved { get; set; }



    public static implicit operator CourseModel(CoursesEntity courseEntity)
    {
        try
        {
            return new CourseModel
            {
                Id = courseEntity.Id,
                Title = courseEntity.Title,
                Price = courseEntity.Price,
                DiscountPrice = courseEntity.DiscountPrice,
                HoursToComplete = courseEntity.HoursToComplete,
                LikesInNumbers = courseEntity.LikesInNumbers,
                LikesinPercent = courseEntity.LikesinPercent,
                IsBestSeller = courseEntity.IsBestSeller,
                BackgroundImageName = courseEntity.BackgroundImageName!,

                Category = new CategoryModel
                {
                    Id = courseEntity.Category!.Id,
                    CategoryName = courseEntity.Category.CategoryName,
                },

                Author = new AuthorModel
                {
                    Id = courseEntity.Author.Id,
                    AuthorTitle = courseEntity.Author.AuthorTitle,
                    AuthorName = courseEntity.Author.AuthorName,
                    AuthorDescription = courseEntity.Author.AuthorDescription,
                    AuthorImageUrl = courseEntity.Author.AuthorImageUrl,
                    FacebookSubs = courseEntity.Author.FacebookSubs,
                    YoutubeSubs = courseEntity.Author.YoutubeSubs,
                }

            };
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }
}
