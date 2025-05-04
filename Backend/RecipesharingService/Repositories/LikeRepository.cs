using MongoDB.Driver;
using RecipesharingService.Models;
using RecipesharingService.MongoDB;

namespace RecipesharingService.Repositories
{
    public class LikeRepository
    {
        private readonly IMongoCollection<Like> _likes;

        public LikeRepository(MongoDbContext context)
        {
            _likes = context.GetCollection<Like>("Likes");
        }

        public async Task<IEnumerable<Like>> GetByRecipeIdAsync(Guid recipeId)
        {
            return await _likes.Find(l => l.RecipeId == recipeId).ToListAsync();
        }

        public async Task AddAsync(Like like)
        {
            await _likes.InsertOneAsync(like);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _likes.DeleteOneAsync(l => l.Id == id);
        }
    }
}
