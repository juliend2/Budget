using Microsoft.AspNetCore.Mvc;
using Budget.Web.Repositories;

namespace Budget.Web.Controllers;

// Le nom de la classe DOIT se terminer par "Controller"
public class ExpensesController : Controller 
{
    private readonly IBudgetLines _repository;

    public ExpensesController(IBudgetLines repository)
    {
        _repository = repository;
    }

    // Cette méthode doit être 'public' et retourner 'IActionResult' ou 'Task<IActionResult>'
    public async Task<IActionResult> Index()
    {
        // return Content("Le controller fonctionne !");
        var data = await _repository.GetAllAsync();
        return View(data);
    }
}