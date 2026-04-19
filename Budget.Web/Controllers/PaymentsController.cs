using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;
using Dapper;
using Budget.Web.Models;

namespace Budget.Web.Controllers;

[Route("Expenses/{expenseId}/Payments")]
public class PaymentsController : Controller
{
    private readonly IPayments _repository;
    private readonly IExpenses _expenses;

    public PaymentsController(IPayments repository, IExpenses expenses)
    {
        _repository = repository;
        _expenses = expenses;
    }

    [HttpGet("Create", Name = "CreatePayment")]
    public async Task<IActionResult> Create(int expenseId)
    {
        var remaining = await _expenses.GetExpenseRemainingAmountAsync(expenseId);
        return View(new PaymentForm() { ExpenseId = expenseId, Amount = remaining });
    }

    [HttpPost("Create", Name = "CreatePayment")]
    public async Task<IActionResult> Create(PaymentForm form)
    {
        if (!ModelState.IsValid) return View(form);
        await _repository.CreateAsync(form);
        return RedirectToAction("Index", "Expenses");
    }

}