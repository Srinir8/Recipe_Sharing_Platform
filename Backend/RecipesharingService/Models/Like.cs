using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecipesharingService.Models
{
    public class Like
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonRepresentation(BsonType.String)] 
        public Guid RecipeId { get; set; } 

        [BsonRepresentation(BsonType.String)] 
        public Guid UserId { get; set; } 

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}