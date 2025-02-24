using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;

namespace ExpenseTracker.Orchestrator
{
    public class UserOrchestrator : IUserOrchestrator
    {
        private readonly IUserRepository _userRepository;

        public UserOrchestrator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> AddUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Role))
            {
                throw new ArgumentException("All fields are required.");
            }

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid User ID.");

            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user.Id <= 0 || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException("All fields are required.");
            }

            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid User ID.");

            return await _userRepository.DeleteUserAsync(userId);
        }
    }
}
