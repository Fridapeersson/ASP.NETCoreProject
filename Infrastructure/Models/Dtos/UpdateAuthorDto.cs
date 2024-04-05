namespace Infrastructure.Models.Dtos;

public class UpdateAuthorDto
{
    public string? AuthorName { get; set; }
    public string? AuthorImageUrl { get; set; }
    public string? AuthorTitle { get; set; }
    public int YoutubeSubs { get; set; }
    public int FacebookSubs { get; set; }
    public string? AuthorDescritpion { get; set; }
}
