using Microsoft.AspNetCore.Mvc;
using RecipesharingService.Models;
using RecipesharingService.Services;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetComments(Guid recipeId)
        {
            var comments = await _service.GetCommentsByRecipeIdAsync(recipeId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            await _service.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetComments), new { recipeId = comment.RecipeId }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            await _service.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}