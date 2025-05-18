namespace RecipesharingService.Models
{
    public class LikeInputModel
    {
        public Guid RecipeId { get; set; }
        public Guid UserId { get; set; }
    }
}
