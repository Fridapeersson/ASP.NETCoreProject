﻿using Infrastructure.Contexts;
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
            ////IQueryable<CoursesEntity> query = _context.Courses
            ////                    .Include(i => i.Author)
            ////                    .Include(i => i.Category)
            ////                    .AsQueryable();

            ////if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            ////{
            ////    query = query.Where(x => x.Category!.CategoryName.ToLower() == category.ToLower());
            ////}

            ////if (!string.IsNullOrEmpty(searchQuery))
            ////{
            ////    //if(searchQuery.Equals("undefined"))
            ////    //{
            ////    //    searchQuery = "";
            ////    //}
            ////    query = query.Where(x => x.Title.Contains(searchQuery) || x.Author.AuthorName!.Contains(searchQuery));
            ////}
            ////int totalItems = await query.CountAsync();
            ////List<CoursesEntity> courses = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            ////int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ////return new CourseResult
            ////{
            ////    Courses = courses,
            ////    TotalItems = totalItems,
            ////    TotalPages = totalPages
            ////};

            //////return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        return null!;
    }

    //public override async Task<IEnumerable<CoursesEntity>> GetAllPaginatedAsync(int page, int pageSize)
    //{
    //    try
    //    {
    //        var entities = await _context.Courses.Include(x => x.Author).OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    //        return entities;
    //    }
    //    catch(Exception ex) { Debug.WriteLine(ex); }
    //    return new List<CoursesEntity>();
    //}

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


    //public async Task<IEnumerable<CoursesEntity>> GetAllFilteredASync(string category, string searchWord)
    //{
    //    try
    //    {
    //        var text = _context.Courses.AsQueryable();
    //        if (!string.IsNullOrEmpty(searchWord))
    //        {
    //            text = text.Where(x => x.Title.Contains(searchWord));
    //        }

    //        if (!string.IsNullOrEmpty(category) && category != "All Categories")
    //        {
    //            text = text.Where(x => x.CategoryName == category);
    //        }

    //        return await text.Include(x => x.Author).ToListAsync();
    //    }
    //    catch (Exception ex) { Debug.WriteLine(ex); }
    //    return null!;
    //}
}