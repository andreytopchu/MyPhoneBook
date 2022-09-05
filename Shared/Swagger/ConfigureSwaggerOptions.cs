using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiExplorer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _apiExplorer = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<SetDefaultParameterVersionOperationFilter>();
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.UseAllOfToExtendReferenceSchemas();

            foreach (var item in _apiExplorer.ApiVersionDescriptions)
            {
                options.SwaggerDoc(item.GroupName, new OpenApiInfo
                {
                    Title = $"{Assembly.GetEntryAssembly()!.GetName().Name} service, v. " + item.GroupName,
                    Version = $"{item.ApiVersion.MajorVersion}.{item.ApiVersion.MinorVersion}"
                });
            }

            foreach (var fileName in Directory.GetFiles(AppContext.BaseDirectory, "*.xml"))
            {
                options.IncludeXmlComments(fileName, includeControllerXmlComments: true);
            }
        }
    }
}