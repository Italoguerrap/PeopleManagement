namespace PeopleManagement.API.PipelineExtensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhostFrontend", policy =>
                {
                    string frontendUrl = configuration["Cors:FrontendUrl"];

                    policy
                        .WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}