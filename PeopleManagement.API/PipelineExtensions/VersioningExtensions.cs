using Microsoft.AspNetCore.Mvc.Versioning;

namespace PeopleManagement.API.PipelineExtensions
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IHostApplicationBuilder builder)
        {
            return builder.Services.AddApiVersioning(static config =>
            {
                config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
    }
}