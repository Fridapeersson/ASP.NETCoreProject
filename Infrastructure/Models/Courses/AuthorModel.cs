namespace Infrastructure.Models.Courses;

public class AuthorModel
{
    public int Id { get; set; }
    public string AuthorTitle { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string? AuthorImageUrl { get; set; }
    public string? AuthorDescription { get; set; }
    public decimal YoutubeSubs { get; set; }
    public decimal FacebookSubs { get; set; }
}
