namespace Infrastructure.Models.Courses;


public class PaginationModel
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public void UpdateTotalPages()
    {
        TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}

