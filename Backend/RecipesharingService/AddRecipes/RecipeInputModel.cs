namespace RecipesharingService.AddRecipes
{
    public class RecipeInputModel
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
        public required string UserId { get; set; }
        public IFormFile? Picture { get; set; } // Image file
    }

}
