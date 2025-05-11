using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesharingService.AddRecipes;
using RecipesharingService.Contracts;
using RecipesharingService.DeleteRecipes;
using RecipesharingService.GetAllRecipes;
using RecipesharingService.Models;
using RecipesharingService.UpdateRecipes;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRecipeService _recipeService;

        public RecipeController(IMediator mediator, IRecipeService recipeService)
        {
            _mediator = mediator;
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(Guid id)
        {
            // Call the service to get the recipe by ID
            var recipe = await _recipeService.GetRecipeByIdAsync(id);

            // Check if the recipe exists
            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            return Ok(recipe);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRecipe([FromForm] RecipeInputModel input)
        {
            byte[]? imageBytes = null;
            if (input.Picture != null)
            {
                using var memoryStream = new MemoryStream();
                await input.Picture.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Create a new Recipe object
            var recipe = new Recipe
            {
                Title = input.Title,
                Description = input.Description,
                Ingredients = input.Ingredients,
                Instructions = input.Instructions,
                UserId = input.UserId,
                Picture = imageBytes
            };

            await _mediator.Send(new AddRecipeCommand(recipe));
            return CreatedAtAction(nameof(GetAllRecipes), new { id = recipe.Id }, recipe);

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRecipe(Guid id, [FromForm] RecipeInputModel input)
        {
            // Convert the Base64-encoded image to a byte array
            byte[]? imageBytes = null;
            if (input.Picture != null)
            {
                using var memoryStream = new MemoryStream();
                await input.Picture.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            // Create a new Recipe object
            var recipe = new Recipe
            {
                Id = id,
                Title = input.Title,
                Description = input.Description,
                Ingredients = input.Ingredients,
                Instructions = input.Instructions,
                UserId = input.UserId,
                Picture = imageBytes,
                UpdatedAt = DateTime.UtcNow
            };

            await _mediator.Send(new UpdateRecipeCommand(recipe));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe(Guid id)
        {
            await _mediator.Send(new DeleteRecipeCommand(id));
            return NoContent();
        }

    }

}
