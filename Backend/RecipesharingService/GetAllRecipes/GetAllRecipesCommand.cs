using MediatR;
using RecipesharingService.Models;

namespace RecipesharingService.GetAllRecipes
{
    public record GetAllRecipesQuery : IRequest<IEnumerable<Recipe>>;
}
