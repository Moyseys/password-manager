namespace DAL.Dtos;

public class PageableDto<T>
{
    public required List<T> Items { get; set; }
    public required int Page { get; set; }
    public required int Size { get; set; }
    public required int TotalItems { get; set; }
    public required int TotalPages { get; set; }
}
