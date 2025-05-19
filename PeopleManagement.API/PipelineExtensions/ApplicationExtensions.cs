using Microsoft.AspNetCore.Identity;
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            return services;
        }
    }
}
