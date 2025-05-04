using MediatR;
using RecipesharingService.Contracts;
using RecipesharingService.Models;

namespace RecipesharingService.GetAllRecipes
{
    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipesQuery, IEnumerable<Recipe>>
    {
        private readonly IRecipeService _service;

        public GetAllRecipesHandler(IRecipeService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Recipe>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            return await _service.GetAllRecipesAsync();
        }
    }
}
