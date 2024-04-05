using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class CategoryEntity
{
    [Key]
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;

    //public virtual ICollection<CoursesEntity> Courses { get; set; } = new List<CoursesEntity>();
}
