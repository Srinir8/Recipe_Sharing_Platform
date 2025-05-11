using RecipesharingService.Models;
using RecipesharingService.Repositories;

namespace RecipesharingService.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User?> GetUserByGoogleIdAsync(string googleId)
        {
            return await _repository.GetByGoogleIdAsync(googleId);
        }

        public async Task AddUserAsync(User user)
        {
            await _repository.AddAsync(user);
        }
    }
}
