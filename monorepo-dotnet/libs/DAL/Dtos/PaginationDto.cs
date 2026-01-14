namespace DAL.Dtos;

public class PaginationDto
{
    public required int Page { get; set; }
    public required int Size { get; set; }
    public string Sort { get; set; } = string.Empty;
}