using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;
using Budget.Web.Models;

namespace Budget.Web.Controllers;

public class ExpensesController : Controller 
{
    private readonly IBudgetLines _repository;

    public ExpensesController(IBudgetLines repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        _repository.CreateRepeatableExpenses();
        var data = await _repository.GetAllAsync();
        var splitData = BudgetLines.SplitBudgetLinesByPay((List<BudgetLine>)data);
        return View(splitData);
    }
}