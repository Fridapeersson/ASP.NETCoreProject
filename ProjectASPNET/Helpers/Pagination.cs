namespace ProjectASPNET.Helpers;

public class Pagination<CourseEntity>
{
    public IEnumerable<CourseEntity> Items { get; set; }
    public int TotalCount { get; set; }


    public Pagination(IEnumerable<CourseEntity> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}
