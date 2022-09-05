using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Bll.Models;

/// <summary>
/// Дто сохранения данных пользователя
/// </summary>
public class SaveUserRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Идентификаторы групп, к которым пользователь относится
    /// </summary>
    [Required]
    public Guid[] GroupIds { get; set; }

    /// <summary>
    /// Url фото
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Мобильные номера пользователя
    /// </summary>
    [Required]
    public SavePhoneNumberRequest[] PhoneNumbers { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public AddressDto Address { get; set; }
}