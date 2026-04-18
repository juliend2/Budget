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
}