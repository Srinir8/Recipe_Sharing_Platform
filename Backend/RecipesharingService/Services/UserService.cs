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

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _repository.AddAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _repository.UpdateAsync(user);
        }
    }

}
