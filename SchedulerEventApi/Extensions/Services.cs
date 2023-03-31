using ScedulerEventDomain.Services;
using SchedulerEventRepositories.Repositories;

namespace SchedulerEventApi.Extensions
{
    public static class ServicesExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();

            services.AddTransient<IDeveloperService, DeveloperService>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }
    }
}