using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class AuthorRepository : BaseRepo<AuthorsEntity>
{
    private readonly DataContext _context;
    public AuthorRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<AuthorsEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Authors
                //.Include(i => i.Courses)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    public override async Task<AuthorsEntity> GetOneAsync(Expression<Func<AuthorsEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Authors
                //.Include(i => i.Courses)
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
