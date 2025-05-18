namespace RecipesharingService.Models
{
    public class CommentInputModel
    {
        public Guid RecipeId { get; set; }
        public Guid UserId { get; set; }
        public required string Content { get; set; }
    }
}
