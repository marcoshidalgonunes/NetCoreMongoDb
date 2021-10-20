using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Models
{
    public class Book : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }
    }
}
