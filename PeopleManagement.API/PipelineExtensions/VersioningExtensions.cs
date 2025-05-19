using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace PeopleManagement.API.PipelineExtensions
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddVersioning(this IHostApplicationBuilder builder)
        {
            return builder.Services.AddApiVersioning(static config =>
            {
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }
    }
}