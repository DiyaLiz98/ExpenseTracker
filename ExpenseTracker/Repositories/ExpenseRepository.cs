using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Newtonsoft.Json;

public class ExpenseRepository : IExpenseRepository
{
    // ✅ Async: Add Multiple Expenses
    public async Task AddExpenseAsync(List<Expense> expenseList)
    {
        var json = JsonConvert.SerializeObject(expenseList);

        var parameters = new Dictionary<string, object>
        {
            { "@Expenses", json }
        };

        await DatabaseHelper.ExecuteNonQueryAsync("sp_InsertMultipleExpenses", parameters);
    }

    // ✅ Async: Get All Expenses
    public async Task<List<Expense>> GetExpensesAsync()
    {
        return await DatabaseHelper.ExecuteQueryAsync<Expense>("sp_GetAllExpenses");
    }

    // Async: Get Expenses by User ID
    public async Task<List<Expense>> GetExpensesByUserAsync(int userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@UserId", userId }
        };

        return await DatabaseHelper.ExecuteQueryAsync<Expense>("sp_GetExpensesByUser", parameters);
    }
}
