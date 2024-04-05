using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Dtos;

public class CourseDto
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal DiscountPrice { get; set; }
    public int HoursToComplete { get; set; }
    public decimal LikesInPercent { get; set; }
    public decimal LikesInNumbers { get; set; }
    public bool IsBestSeller { get; set; }
    public string? BackgroundImageName { get; set; }
    //public string? CategoryName { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }


    public AuthorDto Author { get; set; } = null!;


    public CategoryDto Category { get; set; } = null!;


    public static implicit operator CoursesEntity(CourseDto courseDto)
    {
        return new CoursesEntity
        {
            Title = courseDto.Title,
            Price = courseDto.Price,
            DiscountPrice = courseDto.DiscountPrice,
            HoursToComplete = courseDto.HoursToComplete,
            LikesinPercent = courseDto.LikesInPercent,
            LikesInNumbers = courseDto.LikesInNumbers,
            IsBestSeller = courseDto.IsBestSeller,
            BackgroundImageName = courseDto.BackgroundImageName,
            AuthorId = courseDto.AuthorId,
            CategoryId = courseDto.CategoryId,
            Author = new AuthorsEntity
            {
                AuthorName = courseDto.Author.AuthorName,
                AuthorTitle = courseDto.Author.AuthorTitle,
                AuthorDescription = courseDto.Author.AuthorDescritpion,
                AuthorImageUrl = courseDto.Author.AuthorImageUrl,
                FacebookSubs = courseDto.Author.FacebookSubs,
                YoutubeSubs = courseDto.Author.YoutubeSubs,
            },
            Category = new CategoryEntity
            {
                CategoryName = courseDto.Category!.CategoryName
            }
        };
    }
}
