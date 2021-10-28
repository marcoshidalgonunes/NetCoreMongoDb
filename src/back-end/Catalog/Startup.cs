using System;
using Catalog.Core;
using Catalog.Data;
using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;
using Catalog.Domain.Mapping;
using Catalog.Service;
using Catalog.Service.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Catalog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment() || _env.IsEnvironment("DockerCompose"))
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback);
                            builder.AllowAnyHeader();
                            builder.AllowAnyMethod();
                        });
                });
            }

            EntityMongoMapper.Map<Book, string>();

            // requires using Microsoft.Extensions.Options
            services.Configure<MongoDbSettings>(
                Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddSingleton<IMongoDbRepository<Book, string>, BookRepository>();
            services.AddSingleton<IMongoDbService<Book, string>, BookService>();

            services.AddControllers();

            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreWebAPIMongoDB", Version = "v1" });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreWebAPIMongoDB v1"));
            }

            app.UseExceptionMiddleware();

            // This will make the HTTP requests log as rich logs instead of plain text.
            app.UseSerilogRequestLogging();

            _ = app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
