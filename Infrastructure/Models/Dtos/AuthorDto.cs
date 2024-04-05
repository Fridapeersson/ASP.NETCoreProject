namespace Infrastructure.Models.Dtos;

public class AuthorDto
{
    public string? AuthorName { get; set; }
    public string? AuthorImageUrl { get; set; }
    public string? AuthorTitle { get; set; } 
    public decimal YoutubeSubs { get; set; }
    public decimal FacebookSubs { get; set; }
    public string? AuthorDescritpion { get; set; }
}
