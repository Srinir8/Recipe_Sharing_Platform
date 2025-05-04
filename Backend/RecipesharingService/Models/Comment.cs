using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonRepresentation(BsonType.String)]
    public Guid RecipeId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}