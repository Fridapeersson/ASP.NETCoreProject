namespace Infrastructure.Models;

public class AuthorsModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string? AuthorImageUrl { get; set; }
    public string? YoutubeSubs { get; set; }
    public string? FacebookSubs { get; set; }


}

