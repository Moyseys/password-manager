namespace PasswordManager.SharedDtos;

public class PaginationDto
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string Sort { get; set; }
}