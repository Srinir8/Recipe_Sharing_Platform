using RecipesharingService.Models;
using RecipesharingService.Repositories;

namespace RecipesharingService.Services
{
    public class LikeService
    {
        private readonly LikeRepository _repository;

        public LikeService(LikeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Like>> GetLikesByRecipeIdAsync(Guid recipeId)
        {
            return await _repository.GetByRecipeIdAsync(recipeId);
        }

        public async Task AddLikeAsync(Like like)
        {
            await _repository.AddAsync(like);
        }

        public async Task DeleteLikeAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}
