using MongoDB.Driver;
using RecipesharingService.Models;
using RecipesharingService.MongoDB;

namespace RecipesharingService.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.GetCollection<User>("Users");
        }

        // Get a user by Google ID
        public async Task<User?> GetByGoogleIdAsync(string googleId)
        {
            return await _users.Find(u => u.GoogleId == googleId).FirstOrDefaultAsync();
        }

        // Get a user by ID
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        // Add a new user
        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        // Update an existing user
        public async Task UpdateAsync(User user)
        {
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");
            }
        }
    }

}
