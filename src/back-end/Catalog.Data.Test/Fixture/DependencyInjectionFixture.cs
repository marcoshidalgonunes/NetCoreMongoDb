using System;
using System.IO;
using System.Text;
using Catalog.Data.MongoDb;
using Catalog.Domain.Entity;
using Catalog.Domain.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Catalog.Data.Test.Fixture;

public sealed class DependencyInjectionFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public DependencyInjectionFixture()
    {
        EntityMongoMapper.Map<Book, string>();

        var appsettings = "{ \"MongoDbSettings\": { \"CollectionName\": \"Books\", \"ConnectionString\": \"mongodb://localhost:27017\", \"DatabaseName\": \"BookstoreDb\" } }";

        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(appsettings)))
            .Build();

        var services = new ServiceCollection();

        services.Configure<MongoDbSettings>(
            config.GetSection(nameof(MongoDbSettings)));

        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}
