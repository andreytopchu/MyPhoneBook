using System;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Dal;
using PhoneBook.Dal.Migrations;
using Shared.Dal;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PhoneBook
{
    public class Startup
    {
        private readonly Assembly _entryAssembly = Assembly.GetEntryAssembly()
                                                   ?? throw new InvalidOperationException("GetEntryAssembly is null");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            // automapper
            services.AddAutoMapper(expression => expression.AddMaps(_entryAssembly));

            //EF
            services.AddDbContext<PhoneBookDbContext, IPhoneBookMigrationMarker>(Configuration.GetConnectionString("PhoneBookConnectionString"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();

            // check automapper config
            var provider = app.ApplicationServices.GetRequiredService<IConfigurationProvider>();
            provider.AssertConfigurationIsValid();
        }
    }
}