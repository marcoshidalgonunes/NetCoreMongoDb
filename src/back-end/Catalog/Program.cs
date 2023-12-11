using System.Diagnostics;
using Catalog.Core;
using Catalog.Domain.Mapping;
using Catalog.Domain.Models;
using Catalog.Repositories;
using Catalog.Repositories.MongoDb;
using Catalog.Services;
using Catalog.Services.MongoDb;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    var environment = builder.Environment;

    EntityMongoMapper.Map<Book, string>();

    // requires using Microsoft.Extensions.Options
    services.Configure<MongoDbSettings>(
        builder.Configuration.GetSection(nameof(MongoDbSettings)));

    services.AddSingleton<IMongoDbSettings>(sp =>
        sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

    services.AddSingleton<IMongoDbRepository<Book, string>, BookRepository>();
    services.AddSingleton<IMongoDbService<Book, string>, BookService>();

    // Add services to the container.
    services.AddControllers()
        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreWebAPIMongoDB", Version = "v1" });
    });

    if (environment.IsDevelopment() || environment.IsEnvironment("DockerCompose"))
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

    // Configure Serilog
    builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
        loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
            .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
        // Used to filter out potentially bad data due debugging.
        // Very useful when doing Seq dashboards and want to remove logs under debugging session.
        loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionMiddleware();

    // This will make the HTTP requests log as rich logs instead of plain text.
    app.UseSerilogRequestLogging();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Terminanting");
    Log.CloseAndFlush();
}