﻿namespace PhoneBook.Bll.Models
{
    /// <summary>
    /// Дто для получения данных пользователя
    /// </summary>
    public class UserData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public PhoneData[] Phones { get; set; }
        public AddressDto Address { get; set; }
        public GroupDto[] Groups { get; set; }
        public string? ImageUrl { get; set; }
    }
}