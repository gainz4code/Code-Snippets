using Core.Infrastructure.MongoDb.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.Infrastructure.MongoDb.Implementation;

public class Document : IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = null!;
}

