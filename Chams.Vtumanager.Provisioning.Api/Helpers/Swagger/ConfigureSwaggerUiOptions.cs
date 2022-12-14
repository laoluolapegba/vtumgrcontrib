using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics;
using System.Linq;

namespace Chams.Vtumanager.Provisioning.Api.Helpers.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConfigureSwaggerUiOptions : IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;
        private readonly SwaggerSettings settings;

        /// <inheritdoc />
        public ConfigureSwaggerUiOptions(IApiVersionDescriptionProvider versionDescriptionProvider, IOptions<SwaggerSettings> settings)
        {
            Debug.Assert(versionDescriptionProvider != null, $"{nameof(versionDescriptionProvider)} != null");
            Debug.Assert(settings != null, $"{nameof(versionDescriptionProvider)} != null");

            this.provider = versionDescriptionProvider;
            this.settings = settings?.Value ?? new SwaggerSettings();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerUIOptions options)
        {
            provider
                .ApiVersionDescriptions
                .ToList()
                .ForEach(description =>
                {
                    options.SwaggerEndpoint(
                        $"/{settings.RoutePrefixWithSlash}{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());

                    options.RoutePrefix = settings.RoutePrefix;
                });
        }
    }
}
