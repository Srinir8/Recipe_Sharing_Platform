using RecipesharingService.Models;

namespace RecipesharingService.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();

        Task<Recipe> GetRecipeByIdAsync(Guid id);

        Task AddRecipeAsync(Recipe recipe);

        Task UpdateRecipeAsync(Recipe recipe);

        Task DeleteRecipeAsync(Guid id);
    }
}
