using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Models.Courses;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CoursesRepository : BaseRepo<CoursesEntity>
{
    private readonly DataContext _context;
    public CoursesRepository(DataContext context) : base(context)
    {
        _context = context;
    }



    public override async Task<IEnumerable<CoursesEntity>> GetAllAsync()
    {
        try
        {
            List<CoursesEntity> result = await _context.Courses
                        .Include(i => i.Author)
                        .Include(i => i.Category)
                        .ToListAsync();

            return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }




    public IQueryable<CoursesEntity> GetCoursesAsync(/*string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 9*/)
    {
        try
        {
            return _context.Courses
                     .Include(i => i.Author)
                     .Include(i => i.Category)
                     .AsQueryable();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null!;
    }

    public override async Task<CoursesEntity> GetOneAsync(Expression<Func<CoursesEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Courses
                .Include(i => i.Author)
                .Include(i => i.Category)
                .FirstOrDefaultAsync(predicate);

            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }
}
