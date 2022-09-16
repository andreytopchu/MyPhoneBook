namespace PhoneBook.Bll.Models;

public class ExportUserDataDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string PhoneNumbers { get; set; }
    public string Address { get; set; }
    public string Groups { get; set; }
    public string Email { get; set; } = null!;
    public string Gender { get; set; } = null!;
}