using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecipesharingService.Models
{
    public class User
    {
        [BsonId] // Maps this property to MongoDB's _id field
        [BsonRepresentation(BsonType.String)] // Ensures the _id is stored as a string (Guid)
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique user ID
        public required string GoogleId { get; set; } // Unique ID from Google
        public required string Email { get; set; } // User's email address
        public string? Name { get; set; } // User's name (optional)
        public string? ProfilePictureUrl { get; set; } // URL to the user's profile picture
        public string Role { get; set; } = "User"; // Roles: "User", "Admin"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Account creation timestamp
    }
}
