using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<int> AddUserAsync(User user)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Name", user.Name },
                { "@Email", user.Email },
                { "@Password", user.Password },  // 🔹 Stored as plain text for now
                { "@Role", user.Role }
            };

            var newUserId = await DatabaseHelper.ExecuteScalarAsync<int>("dbo.usp_AddUser", parameters);
            return newUserId;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };
            var users = await DatabaseHelper.ExecuteQueryAsync<User>("dbo.sp_GetUserById", parameters);
            return users.Count > 0 ? users[0] : null;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await DatabaseHelper.ExecuteQueryAsync<User>("dbo.sp_GetAllUsers");
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var parameters = new Dictionary<string, object>
        {
            { "@UserId", user.Id },
            { "@Name", user.Name },
            { "@Email", user.Email }
        };

            await DatabaseHelper.ExecuteNonQueryAsync("dbo.sp_UpdateUser", parameters);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };
            await DatabaseHelper.ExecuteNonQueryAsync("dbo.sp_DeleteUser", parameters);
            return true;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var parameters = new Dictionary<string, object>
            {
                {"@Email", email }
            };

            var result = await DatabaseHelper.ExecuteQueryAsync<User>("dbo.usp_GetUserByEmail", parameters);
            return result.FirstOrDefault();
        }
    }
}
