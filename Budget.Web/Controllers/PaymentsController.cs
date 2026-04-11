using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;
using Dapper;
using Budget.Web.Models;

namespace Budget.Web.Controllers;

[Route("Expenses/{expenseId}/Payments")]
public class PaymentsController : Controller
{
    private readonly IPayments _repository;

    public PaymentsController(IPayments repository)
    {
        _repository = repository;
    }

    [HttpGet("Create", Name = "CreatePayment")]
    public IActionResult Create(int expenseId)
    {
        return View(new PaymentForm() { ExpenseId = expenseId });
    }

}