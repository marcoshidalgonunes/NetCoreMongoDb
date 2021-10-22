namespace Catalog.Core
{
    public sealed class CatalogDatabaseSettings : ICatalogDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
