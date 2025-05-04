using MongoDB.Driver;
using RecipesharingService.Models;
using RecipesharingService.MongoDB;

namespace RecipesharingService.Repositories
{
    public class RecipeRepository
    {
        private readonly IMongoCollection<Recipe> _recipes;

        public RecipeRepository(MongoDbContext context)
        {
            _recipes = context.GetCollection<Recipe>("Recipes");
        }

        // Get all recipes
        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _recipes.Find(_ => true).ToListAsync();
        }

        // Get a recipe by ID
        public async Task<Recipe?> GetByIdAsync(Guid id)
        {
            return await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        // Add a new recipe
        public async Task AddAsync(Recipe recipe)
        {
            await _recipes.InsertOneAsync(recipe);
        }

        // Update an existing recipe
        public async Task UpdateAsync(Recipe recipe)
        {
            var result = await _recipes.ReplaceOneAsync(r => r.Id == recipe.Id, recipe);
            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Recipe with ID {recipe.Id} not found.");
            }
        }

        // Delete a recipe by ID
        public async Task DeleteAsync(Guid id)
        {
            var result = await _recipes.DeleteOneAsync(r => r.Id == id);
            if (result.DeletedCount == 0)
            {
                throw new KeyNotFoundException($"Recipe with ID {id} not found.");
            }
        }
    }
}