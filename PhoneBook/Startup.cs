using System;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PhoneBook.Bll.Options;
using PhoneBook.Dal;
using PhoneBook.Dal.Migrations;
using Shared.ApiVersion;
using Shared.Dal;
using Shared.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
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

        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            //options
            services.AddOptions<FileOptions>();

            var mvcBuilder = services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // automapper
            services.AddAutoMapper(expression => expression.AddMaps(_entryAssembly));

            //EF
            services.AddDbContext<PhoneBookDbContext, IPhoneBookMigrationMarker>(Configuration.GetConnectionString("PhoneBookConnectionString"));

            // enable versioning API
            var defaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

            services.AddVersionedApiExplorer();
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.DefaultApiVersion = defaultApiVersion;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ApiVersionReader = ApiVersionReader.Combine(
                    // new HeaderApiVersionReader(VersionConstants.VersionHeaderName),
                    new QueryStringApiVersionReader("api-version"));
            });

            // configure versioning for swagger
            mvcBuilder.AddMvcOptions(x =>
            {
                var apiExplorerVersionConvention = new ApiExplorerVersionConvention(defaultApiVersion);

                x.Conventions.Add((IControllerModelConvention) apiExplorerVersionConvention);
                x.Conventions.Add((IActionModelConvention) apiExplorerVersionConvention);
            });

            // swagger
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(ConfigureSwagger);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();

            // check automapper config
            var provider = app.ApplicationServices.GetRequiredService<IConfigurationProvider>();
            provider.AssertConfigurationIsValid();

            //swagger
            var apiExplorer = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var item in apiExplorer.ApiVersionDescriptions)
                {
                    var version = $"{item.ApiVersion.MajorVersion}.{item.ApiVersion.MinorVersion}";
                    c.SwaggerEndpoint($"{version}/swagger.json", $"{GetType().Namespace} v{version}");
                    c.EnableDeepLinking();
                }
            });
        }

        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.EnableAnnotations();
            options.DocInclusionPredicate((version, desc) =>
            {
                // Игнорим методы не помеченные явно HTTP методом (GET, POST и тд).
                if (desc.HttpMethod == null)
                    return false;

                return desc.GetApiVersion().ToString() == version;
            });

            options.TagActionsBy(
                api =>
                {
                    if (api.GroupName != null && !Regex.IsMatch(api.GroupName, @"^\d"))
                    {
                        return new[] {api.GroupName};
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] {controllerActionDescriptor.ControllerName};
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
        }
    }
}