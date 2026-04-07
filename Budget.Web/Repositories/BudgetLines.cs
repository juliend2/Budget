using Budget.Web.Models;

namespace Budget.Web.Repositories;

public interface IBudgetLine
{
    Task<IEnumerable<BudgetLine>> GetPendingExpensesAsync();
}