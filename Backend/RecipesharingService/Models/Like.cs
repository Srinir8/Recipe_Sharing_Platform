using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecipesharingService.Models
{
    public class Like
    {
        [BsonId] // Maps this property to MongoDB's _id field
        [BsonRepresentation(BsonType.String)] // Ensures the _id is stored as a string (Guid)
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique like ID

        [BsonRepresentation(BsonType.String)] // Ensures RecipeId is stored as a string
        public Guid RecipeId { get; set; } // ID of the liked recipe

        [BsonRepresentation(BsonType.String)] // Ensures UserId is stored as a string
        public Guid UserId { get; set; } // ID of the user who liked the recipe

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}