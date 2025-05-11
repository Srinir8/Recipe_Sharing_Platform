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

        public async Task<User?> GetByGoogleIdAsync(string googleId)
        {
            return await _users.Find(u => u.GoogleId == googleId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}
