using Microsoft.EntityFrameworkCore;
using Pub.ResourceEngagement.Options;
using Pub.ResourceEngagement.Persistence;
using Pub.ResourceEngagement.Services;
using System.Reflection;

namespace Pub.ResourceEngagement.Extensions
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("PubSubPatternDb");
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            // Add Options
            services.Configure<PublisherOptions>(configuration.GetSection("PublisherSettings"));

            // Add services to the container.
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IEngagementOrderService, EngagementOrderService>();
            services.AddTransient<IPublisherService, PublisherService>();

            return services;
        }
    }
}
