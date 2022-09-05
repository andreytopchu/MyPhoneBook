﻿using Dex.Ef.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Npgsql;
using PhoneBook.Dal.Models;
using Shared.Dal;

namespace PhoneBook.Dal
{
    public class PhoneBookDbContext : BaseDbContext<PhoneBookDbContext>
    {
        public PhoneBookDbContext(ISystemClock clock, IModelStore modelStore, INpgsqlNameTranslator nameTranslator, DbContextOptions<PhoneBookDbContext> options) : base(clock, modelStore, nameTranslator, options)
        {
        }

        public DbSet<UserDb> Users => Set<UserDb>();
        public DbSet<AddressDb> Addresses => Set<AddressDb>();
        public DbSet<GroupDb> Groups => Set<GroupDb>();
        public DbSet<PhoneCategoryDb> PhoneCategories => Set<PhoneCategoryDb>();
        public DbSet<PhoneDataDb> Phones => Set<PhoneDataDb>();
    }
}