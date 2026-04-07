namespace Budget.Web.Models;

public record BudgetLine(
    int Id,
    decimal Amount,
    string Description,
    DateTime ToBePaidAt,
    DateTime? PaidAt,
    int? ExpenseTemplateId,
    decimal Remaining
);