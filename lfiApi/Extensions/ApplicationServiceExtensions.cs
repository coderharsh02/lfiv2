using lfiApi.Data;
using lfiApi.Helpers;
using lfiApi.Interfaces;
using lfiApi.Services;
using lfiApi.SignalR;
using Microsoft.EntityFrameworkCore;

namespace lfiApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // creating interface for service is useful when we want to test the service and is a good practice.
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddScoped<IRepository, Repository>();

            // Add DbContext service to the container and configure it to use Sqlite so that it can be used thoroughout the program. 
            services.AddDbContext<DataContext>(options =>
            {
                Console.WriteLine(config.GetConnectionString("DefaultConnection"));
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}