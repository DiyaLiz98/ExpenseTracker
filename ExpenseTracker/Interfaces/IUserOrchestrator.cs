using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface IUserOrchestrator
    {
        Task<int> AddUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
