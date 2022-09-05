using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dex.Ef.Contracts.Entities;

namespace PhoneBook.Dal.Models;

[Table("user")]
[Microsoft.EntityFrameworkCore.Index(nameof(CreatedUtc))]
[Microsoft.EntityFrameworkCore.Index(nameof(DeletedUtc))]
public class UserDb : ICreatedUtc, IDeletable, IUpdatedUtc
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_utc")]
    public DateTime CreatedUtc { get; set; }

    [Column("deleted_utc")]
    public DateTime? DeletedUtc { get; set; }

    [Column("updated_utc")]
    public DateTime UpdatedUtc { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    public string LastName { get; set; } = null!;

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("image_url")]
    public string ImageUrl { get; set; }

    public AddressDb Address { get; set; }
    public ICollection<GroupDb> Groups { get; set; }
    public ICollection<PhoneDataDb> Phones { get; set; }
}