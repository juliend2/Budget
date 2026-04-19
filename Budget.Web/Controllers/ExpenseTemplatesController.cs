using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;
using Dapper;
using Budget.Web.Models;

namespace Budget.Web.Controllers;

public class ExpenseTemplatesController : Controller
{
    private readonly IExpenseTemplates _repository;

    public ExpenseTemplatesController(IExpenseTemplates repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var templates = await _repository.GetAllAsync();
        return View(templates);
    }

    [HttpGet("Create", Name = "CreateExpenseTemplate")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create", Name = "CreateExpenseTemplate")]
    public async Task<IActionResult> Create(ExpenseTemplateForm form)
    {
        if (!ModelState.IsValid) return View(form);
        await _repository.CreateAsync(form);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var template = await _repository.GetByIdAsync(id);
        if (template == null) return NotFound();
        return View(new ExpenseTemplateForm
        {
            Id = template.Id,
            Description = template.Description,
            InitialToBePaidOn = template.InitialToBePaidOn,
            RepeatabilityIntervalUnit = template.RepeatabilityIntervalUnit,
            RepeatabilityIntervalPace = template.RepeatabilityIntervalPace,
            Amount = template.Amount,
            IsOnHold = template.IsOnHold
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ExpenseTemplateForm form)
    {
        if (!ModelState.IsValid) return View(form);
        await _repository.UpdateAsync(form);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}