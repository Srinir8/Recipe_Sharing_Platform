using MongoDB.Driver;
using RecipesharingService.Models;
using RecipesharingService.MongoDB;

namespace RecipesharingService.Repositories
{
    public class CommentRepository
    {
        private readonly IMongoCollection<Comment> _comments;

        public CommentRepository(MongoDbContext context)
        {
            _comments = context.GetCollection<Comment>("Comments");
        }

        public async Task<IEnumerable<Comment>> GetByRecipeIdAsync(Guid recipeId)
        {
            return await _comments.Find(c => c.RecipeId == recipeId).ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _comments.InsertOneAsync(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _comments.DeleteOneAsync(c => c.Id == id);
        }
    }

}
