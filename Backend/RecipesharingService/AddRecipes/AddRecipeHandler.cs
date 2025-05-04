using MediatR;
using RecipesharingService.Contracts;

namespace RecipesharingService.AddRecipes
{
    public class AddRecipeHandler : IRequestHandler<AddRecipeCommand, Unit>
    {
        private readonly IRecipeService _service;

        public AddRecipeHandler(IRecipeService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            await _service.AddRecipeAsync(request.Recipe);
            return Unit.Value;
        }
    }
}
