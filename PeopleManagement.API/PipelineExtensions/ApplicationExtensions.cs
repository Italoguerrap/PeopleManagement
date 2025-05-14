using PeopleManagement.Application.Interfaces;
using PeopleManagement.Application.Services;

namespace PeopleManagement.API.PipelineExtensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IPeopleService, PeopleService>();
            return services;
        }
    }
}
