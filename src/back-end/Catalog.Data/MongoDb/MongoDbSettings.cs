namespace Catalog.Data.MongoDb
{
    public sealed class MongoDbSettings : IMongoDbSettings
    {
        public string CollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
