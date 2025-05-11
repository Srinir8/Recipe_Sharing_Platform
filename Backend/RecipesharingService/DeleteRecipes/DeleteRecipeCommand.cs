using MediatR;

namespace RecipesharingService.DeleteRecipes
{
    public record DeleteRecipeCommand(Guid RecipeId) : IRequest<Unit>;
}
