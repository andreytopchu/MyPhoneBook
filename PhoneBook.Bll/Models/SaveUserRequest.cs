using System.ComponentModel.DataAnnotations;
using PhoneBook.Dal.Enums;

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

    /// <summary>
    /// Пол
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }
}