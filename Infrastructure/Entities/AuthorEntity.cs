using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class AuthorsEntity
{
    [Key]
    public int Id { get; set; }
    public string AuthorTitle { get; set; } = null!;
    public string? AuthorName { get; set; }
    public string? AuthorImageUrl { get; set; }
    public string? AuthorDescription { get; set; }
    public decimal YoutubeSubs { get; set; }
    public decimal FacebookSubs { get; set; }
}
