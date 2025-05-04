using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesharingService.AddRecipes;
using RecipesharingService.GetAllRecipes;
using RecipesharingService.Models;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            return Ok(recipes);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe([FromBody] Recipe recipe)
        {
            await _mediator.Send(new AddRecipeCommand(recipe));
            return CreatedAtAction(nameof(GetAllRecipes), new { id = recipe.Id }, recipe);
        }
    }

}
