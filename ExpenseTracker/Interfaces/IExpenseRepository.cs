using ExpenseTracker.Models;
namespace ExpenseTracker.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddExpenseAsync(List<Expense> expenses);
        Task<List<Expense>> GetExpensesAsync();
        Task<List<Expense>> GetExpensesByUserAsync(int userId);

    }
}
