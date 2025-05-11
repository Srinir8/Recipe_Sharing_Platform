using MediatR;
using RecipesharingService.Contracts;

namespace RecipesharingService.DeleteRecipes
{
    public class DeleteRecipeHandler : IRequestHandler<DeleteRecipeCommand, Unit>
    {
        private readonly IRecipeService _service;

        public DeleteRecipeHandler(IRecipeService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteRecipeAsync(request.RecipeId);
            return Unit.Value;
        }
    }
}
