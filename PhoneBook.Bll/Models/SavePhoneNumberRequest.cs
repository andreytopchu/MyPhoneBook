namespace PhoneBook.Bll.Models;

public class SavePhoneNumberRequest
{
    public string PhoneNumber { get; set; }
    public Guid PhoneCategoryId { get; set; }
}