using MediatR;
using RecipesharingService.Models;

namespace RecipesharingService.AddRecipes
{
    public record AddRecipeCommand(Recipe Recipe) : IRequest<Unit>;
}
