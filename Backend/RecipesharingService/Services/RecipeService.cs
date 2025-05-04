using RecipesharingService.Contracts;
using RecipesharingService.Models;
using RecipesharingService.Repositories;

namespace RecipesharingService.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly RecipeRepository _repository;

        public RecipeService(RecipeRepository repository)
        {
            _repository = repository;
        }

        // Get all recipes
        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Get a recipe by ID
        public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Add a new recipe
        public async Task AddRecipeAsync(Recipe recipe)
        {
            await _repository.AddAsync(recipe);
        }

        // Update an existing recipe
        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            await _repository.UpdateAsync(recipe);
        }

        // Delete a recipe by ID
        public async Task DeleteRecipeAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}