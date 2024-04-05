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

    //lägga till/skapa ny db för review, ingress, coursedescription, includes, what you'll learn, programdetails
    

    //[InverseProperty("Author")]
    //public virtual ICollection<CoursesEntity> Courses { get; set; } = [];



}
