using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface IExpenseOrchestrator
    {
        Task AddExpenseAsync(List<Expense> expenses);
        Task<List<Expense>> GetExpensesAsync();
        Task<List<Expense>>GetExpensesByUserAsync(int userId);
    }
}
