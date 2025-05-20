using Microsoft.EntityFrameworkCore;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Infrastructure.Auth;
using PeopleManagement.Infrastructure.Context;
using PeopleManagement.Infrastructure.Repositories;

namespace PeopleManagement.API.PipelineExtensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration["ConnectionStrings:database"];
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connString));
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}