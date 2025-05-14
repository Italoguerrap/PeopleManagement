using Microsoft.EntityFrameworkCore;
using PeopleManagement.Application.Interfaces;
using PeopleManagement.Infrastructure.Context;
using PeopleManagement.Infrastructure.Repositories;

namespace PeopleManagement.API.PipelineExtensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=databse.dat"));
            services.AddScoped<IPersonRepository, PersonRepository>();
            return services;
        }
    }
}