namespace Budget.Web.Models;

public record BudgetLine(
    int Id,
    decimal Amount,
    string Description,
    DateTime ToBePaidAt,
    DateTime? PaidAt,
    int? ExpenseTemplateId,
    decimal Remaining
)
{
    public string MapKey()
    {
        var half = ToBePaidAt.Day < 15 ? 1 : 2;
        return $"{ToBePaidAt.Year}-{ToBePaidAt.Month}-{half}";
    }
}