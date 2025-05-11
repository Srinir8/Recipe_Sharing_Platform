using MediatR;
using RecipesharingService.Models;

namespace RecipesharingService.UpdateRecipes
{
    public record UpdateRecipeCommand(Recipe Recipe) : IRequest<Unit>;
}
