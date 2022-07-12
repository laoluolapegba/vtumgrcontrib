using Microsoft.OpenApi.Models;

namespace Chams.Vtumanager.Web.Api.Helpers.Swagger
{
    /// <summary>
    /// Swagger Configuration
    /// </summary>
    public class SwaggerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerSettings"/> class.
        /// </summary>
        public SwaggerSettings()
        {
            Name = "Chams.Vtumanager.Provisioning API";
            Info = new OpenApiInfo
            {
                Title = "Chams Vtumanager Provisioning",
                Description = "Accepts VTU Topup requests"
            };
        }

        /// <summary>
        /// Gets or sets document Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets swagger Info.
        /// </summary>
        public OpenApiInfo Info { get; set; }

        /// <summary>
        /// Gets or sets RoutePrefix.
        /// </summary>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// Gets Route Prefix with tailing slash.
        /// </summary>
        public string RoutePrefixWithSlash =>
            string.IsNullOrWhiteSpace(RoutePrefix)
                ? string.Empty
                : RoutePrefix + "/";
    }
}
