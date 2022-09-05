using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Internal;
using Npgsql;
using Shared.Dal;

namespace PhoneBook.Dal;

public class PhoneBookDbContextFactory : IDesignTimeDbContextFactory<PhoneBookDbContext>
{
    public PhoneBookDbContext CreateDbContext(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var optionsBuilder = new DbContextOptionsBuilder<PhoneBookDbContext>();
        optionsBuilder.UseNpgsql();

        return new PhoneBookDbContext(new SystemClock(), new ModelStore<PhoneBookDbContext>(),
            NpgsqlConnection.GlobalTypeMapper.DefaultNameTranslator, optionsBuilder.Options);
    }
}