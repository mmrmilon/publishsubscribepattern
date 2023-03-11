using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pub.ResourceEngagement.Options;
using Pub.ResourceEngagement.Persistence;
using Pub.ResourceEngagement.Services;

namespace Pub.ResourceEngagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("PubSubPatternDb");
                });
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            // Add Options
            builder.Services.Configure<PublisherOptions>(builder.Configuration.GetSection("PublisherSettings"));

            // Add services to the container.
            builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            builder.Services.AddTransient<IEngagementOrderService, EngagementOrderService>();
            builder.Services.AddTransient<IPublisherService, PublisherService>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}