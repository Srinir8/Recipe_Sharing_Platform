using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipesharingService.Models;
using RecipesharingService.Services;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _service;

        public LikeController(LikeService service)
        {
            _service = service;
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetLikes(Guid recipeId)
        {
            var likes = await _service.GetLikesByRecipeIdAsync(recipeId);
            return Ok(likes);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddLike([FromBody] LikeInputModel input)
        {
            var like = new Like
            {
                RecipeId = input.RecipeId,
                UserId = input.UserId,
            };
            await _service.AddLikeAsync(like);
            return CreatedAtAction(nameof(GetLikes), new { recipeId = like.RecipeId }, like);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            await _service.DeleteLikeAsync(id);
            return NoContent();
        }
    }
}
