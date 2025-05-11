using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecipesharingService.Models
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
        public required string UserId { get; set; }
        public List<string> LikedBy { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        [BsonRepresentation(BsonType.Binary)]
        public byte[]? Picture { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
