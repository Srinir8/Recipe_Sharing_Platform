using RecipesharingService.Models;
using RecipesharingService.Repositories;

namespace RecipesharingService.Services
{
    public class CommentService
    {
        private readonly CommentRepository _repository;

        public CommentService(CommentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByRecipeIdAsync(Guid recipeId)
        {
            return await _repository.GetByRecipeIdAsync(recipeId);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _repository.AddAsync(comment);
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }

}
