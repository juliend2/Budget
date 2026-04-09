using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;

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
        var data = await _repository.GetAllAsync();
        return View(data);
    }
}