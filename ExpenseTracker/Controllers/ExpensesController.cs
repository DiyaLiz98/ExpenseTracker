using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]/[Action]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseOrchestrator _expenseOrchestrator;

    public ExpenseController(IExpenseOrchestrator expenseOrchestrator)
    {
        _expenseOrchestrator = expenseOrchestrator;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddExpenses([FromBody] List<Expense> expenses)
    {
        try
        {
            await _expenseOrchestrator.AddExpenseAsync(expenses);
            return Ok(new { message = "Expenses added successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Get All Expenses
    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        try
        {
            var expenses = await _expenseOrchestrator.GetExpensesAsync();
            return Ok(expenses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    //Get Expenses by User ID
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetExpensesByUser(int userId)
    {
        try
        {
            var expenses = await _expenseOrchestrator.GetExpensesByUserAsync(userId);
            return Ok(expenses);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
