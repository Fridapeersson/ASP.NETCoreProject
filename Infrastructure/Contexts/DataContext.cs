using Infrastructure.Entities;
using Infrastructure.Entities.Contact;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class DataContext : IdentityDbContext<UserEntity>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<SubscribersEntity> Subscribers { get; set; }
    public DbSet<SavedCoursesEntity> SavedCourses { get; set; }
    public DbSet<AuthorsEntity> Authors { get; set; }
    public DbSet<CoursesEntity> Courses { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ContactUsEntity> ContactUs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // definerar sammansatt nyckel
        builder.Entity<SavedCoursesEntity>()
            .HasKey(x => new { x.UserId, x.CourseId });

        builder.Entity<SavedCoursesEntity>()
            .HasOne<UserEntity>()
            .WithMany(x => x.SavedCourses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SavedCoursesEntity>()
            .HasOne<CoursesEntity>()
            .WithMany(x => x.SavedCourses)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
