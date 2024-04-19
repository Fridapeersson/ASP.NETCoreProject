using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public abstract class BaseRepo<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    protected BaseRepo(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Creates a new entity in the database async
    /// </summary>
    /// <param name="entity">The entity to be created</param>
    /// <returns>The created entity, or null if something went wrong</returns>
    public virtual async Task<TEntity> CreateOneAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Gets one entity based on the predicate async
    /// </summary>
    /// <param name="predicate">The predicate used to filter the entities </param>
    /// <returns>The entity that matches the predicarte, else null if not found</returns>
    public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return entity!;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Gets all entities from database async
    /// </summary>
    /// <returns>A list of entities, or null </returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var result = await _context.Set<TEntity>().ToListAsync();
            return result;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Updates an entity in database based on the provided predicate async
    /// </summary>
    /// <param name="predicate">The predicate used to filter the entity to be updated</param>
    /// <param name="newEntity">The updated entity</param>
    /// <returns>The updated entity, or null</returns>
    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity newEntity)
    {
        try
        {
            var existingEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if(existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                await _context.SaveChangesAsync();
                return existingEntity;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return null!;
    }

    /// <summary>
    ///     Deletes an entity based on the provided predicate async
    /// </summary>
    /// <param name="predicate">The predicate used to filter the entity to be deleted</param>
    /// <returns>True if the entity was deleted successfully, else false</returns>
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if(entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    /// <summary>
    ///     Checks if an entity already exists in database based on the provided predicate async
    /// </summary>
    /// <param name="predicate">The predicate used to chekc if the entity already exists in the database</param>
    /// <returns>True if the entity exists, else falsae</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var existing = await _context.Set<TEntity>().AnyAsync(predicate);
            return existing;
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }


    public virtual async Task<IEnumerable<TEntity>> GetAllPaginatedAsync(int page, int pageSize)
    {
        try
        {
            var entities = await _context.Set<TEntity>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new List<TEntity>(); 
        }
    }

    #region RESPONSE-BASE-REPO
    //public virtual async Task<ResponseResult> CreateOneAsync(TEntity entity)
    //{
    //    try
    //    {
    //        await _context.Set<TEntity>().AddAsync(entity);
    //        await _context.SaveChangesAsync();
    //        return ResponseFactory.OK(entity, "Created successfully");
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}

    //public virtual async Task<ResponseResult> GetAllAsync()
    //{
    //    try
    //    {
    //        IEnumerable<TEntity> result = await _context.Set<TEntity>().ToListAsync();
    //        return ResponseFactory.OK(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}

    //public virtual async Task<ResponseResult> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    //{
    //    try
    //    {
    //        var result = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    //        if (result == null)
    //        {
    //            return ResponseFactory.NOT_FOUND("Not found");
    //        }
    //        return ResponseFactory.OK(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}

    //public virtual async Task<ResponseResult> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity entityToUpdate)
    //{
    //    try
    //    {
    //        var result = _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    //        if (result != null)
    //        {
    //            _context.Entry(result).CurrentValues.SetValues(entityToUpdate);
    //            await _context.SaveChangesAsync();
    //            return ResponseFactory.OK();
    //        }
    //        return ResponseFactory.NOT_FOUND();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}


    //public virtual async Task<ResponseResult> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    //{
    //    try
    //    {
    //        var result = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    //        if (result != null)
    //        {
    //            _context.Set<TEntity>().Remove(result);
    //            await _context.SaveChangesAsync();

    //            ResponseFactory.OK("Deleted successfully");
    //        }
    //        return ResponseFactory.NOT_FOUND();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}

    //public virtual async Task<ResponseResult> AlreadyExistsAsync(Expression<Func<TEntity, bool>> predicate)
    //{
    //    try
    //    {
    //        var result = await _context.Set<TEntity>().AnyAsync(predicate);
    //        if (result)
    //        {
    //            return ResponseFactory.EXISTS();
    //        }
    //        return ResponseFactory.NOT_FOUND();
    //    }
    //    catch (Exception ex)
    //    {
    //        return ResponseFactory.ERROR(ex.Message);
    //    }
    //}
    #endregion
}
