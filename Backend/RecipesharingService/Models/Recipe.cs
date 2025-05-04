using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecipesharingService.Models
{
    public class Recipe
    {
        [BsonId] // Maps this property to MongoDB's _id field
        [BsonRepresentation(BsonType.String)] // Ensures the _id is stored as a string (Guid)
        public Guid Id { get; set; } = Guid.NewGuid(); // Generate a new GUID by default
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
        public required string UserId { get; set; } // ID of the user who created the recipe
        public List<string> LikedBy { get; set; } = new(); // List of user IDs who liked the recipe
        public List<Comment> Comments { get; set; } = new(); // List of comments
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
