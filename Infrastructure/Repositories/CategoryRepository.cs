using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class CategoryRepository : BaseRepo<CategoryEntity>
{
    private readonly DataContext _context;
    public CategoryRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<CategoryEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Categories
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    public override async Task<CategoryEntity> GetOneAsync(Expression<Func<CategoryEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Categories
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
