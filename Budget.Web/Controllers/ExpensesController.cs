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

    public IActionResult Create() => View(new BudgetLineForm());

    [HttpPost]
    public async Task<IActionResult> Create(BudgetLineForm form)
    {
        if (!ModelState.IsValid) return View(form);
        await _repository.CreateAsync(form);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var line = await _repository.GetByIdAsync(id);
        if (line == null) return NotFound();
        return View(new BudgetLineForm { Id = line.Id, Amount = line.Amount, Description = line.Description, ToBePaidAt = line.ToBePaidAt });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BudgetLineForm form)
    {
        if (!ModelState.IsValid) return View(form);
        await _repository.UpdateAsync(form);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
