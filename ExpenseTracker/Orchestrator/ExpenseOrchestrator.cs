using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Currency;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Services.Currency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Middleware;

public class ExpenseOrchestrator : IExpenseOrchestrator
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;
    private readonly ILogger<ExpenseOrchestrator> _logger;

    public ExpenseOrchestrator(IExpenseRepository expenseRepository, ILogger<ExpenseOrchestrator> logger, ICurrencyExchangeService currencyExchangeService)
    {
        _expenseRepository = expenseRepository;
        _logger = logger;
        _currencyExchangeService = currencyExchangeService;
    }

    //Orchestrate adding an expense (Basic for now, extend later)
    public async Task AddExpenseAsync(List<Expense> expenses)
    {
        if (expenses == null || expenses.Count == 0)
        {
            throw new ArgumentException("Expense list cannot be empty.");
        }

        foreach (var expense in expenses)
        {
            if (expense.Amount <= 0)
            {
                throw new ArgumentException("Expense amount must be greater than zero.");
            }

            if (!string.Equals(expense.Currency, "USD", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                   decimal? exchangeRate = await _currencyExchangeService.GetExchangeRateAsync(expense.Currency);
                    if (!exchangeRate.HasValue)
                    {
                        throw new InvalidOperationException($"Exchange rate for {expense.Currency} is not available.");
                    }
                    expense.Amount *= exchangeRate.Value; // Convert amount
                    expense.Currency = "USD"; // Store in base currency
                


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get exchange rate for {Currency}", expense.Currency);
                    throw; 
                }
            }

        }

        await _expenseRepository.AddExpenseAsync(expenses);
    }

    //Get All Expenses
    public async Task<List<Expense>> GetExpensesAsync()
    {
        return await _expenseRepository.GetExpensesAsync();
    }

    //Get Expenses by User ID
    public async Task<List<Expense>> GetExpensesByUserAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Invalid user ID.");
        }

        return await _expenseRepository.GetExpensesByUserAsync(userId);
    }
}
