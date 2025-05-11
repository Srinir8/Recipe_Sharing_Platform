using MediatR;
using RecipesharingService.Contracts;

namespace RecipesharingService.UpdateRecipes
{
    public class UpdateRecipeHandler : IRequestHandler<UpdateRecipeCommand, Unit>
    {
        private readonly IRecipeService _service;

        public UpdateRecipeHandler(IRecipeService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            await _service.UpdateRecipeAsync(request.Recipe);
            return Unit.Value;
        }
    }

}
