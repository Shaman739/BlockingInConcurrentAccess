using BlockingResourcesInConcurrentAccess.Core.Contract;
using BlockingResourcesInConcurrentAccess.Core.UserStories.PurchaceFigure;
using BlockingResourcesInConcurrentAccess.Infrastructure;
using BlockingResourcesInConcurrentAccess.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace BlockingResourcesInConcurrentAccess
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var muxer = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));

            services.AddSingleton<IConnectionMultiplexer>(muxer);

            services.AddSingleton<IFiguresStorage, RedisFiguresStorage>();
            services.AddSingleton<IOrderStorage, OrderStorage>();
            services.AddSingleton<PurchaseFigure>();

            services.AddControllers(options =>
            {
            }).AddNewtonsoftJson(o => { o.SerializerSettings.Converters.Add(new PositionJsonCreationConverter()); });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlockingResourcesInConcurrentAccess", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlockingResourcesInConcurrentAccess v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
