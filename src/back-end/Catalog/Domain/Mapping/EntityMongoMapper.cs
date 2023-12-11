﻿using Catalog.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalog.Domain.Mapping;

public static class EntityMongoMapper
{
    public static void Map<TEntity, TIdentifier>()
        where TEntity : Entity<TIdentifier>, new()
    {
        BsonSerializer.RegisterSerializer(new DoubleSerializer(BsonType.Double));

        var mapper = BsonClassMap.RegisterClassMap<Entity<TIdentifier>>(map => {
            map.AutoMap();
            map.MapIdMember(m => m.Id);
        });

        var memberMap = mapper.MapIdMember(m => m.Id);

        if (typeof(TIdentifier) == typeof(string))
        {
            memberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
            memberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
        }
        else if (typeof(TIdentifier) == typeof(Guid))
        {
            memberMap.SetIdGenerator(GuidGenerator.Instance);
            memberMap.SetSerializer(new GuidSerializer(BsonType.ObjectId));
        }
        else if (typeof(TIdentifier) == typeof(ObjectId))
        {
            memberMap.SetIdGenerator(ObjectIdGenerator.Instance);
            memberMap.SetSerializer(new ObjectIdSerializer(BsonType.ObjectId));
        }

        BsonClassMap.RegisterClassMap<TEntity>();
    }
}
