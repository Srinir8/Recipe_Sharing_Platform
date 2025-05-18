using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentInputModel input)
        {
            var comment = new Comment
            {
                RecipeId = input.RecipeId,
                UserId = input.UserId,
                Content = input.Content,
            };
            await _service.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetComments), new { recipeId = comment.RecipeId }, comment);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            await _service.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}