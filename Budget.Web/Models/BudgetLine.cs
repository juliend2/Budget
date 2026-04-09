namespace Budget.Web.Models;

public record BudgetLine
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime ToBePaidAt { get; init; }
    public DateTime? PaidAt { get; init; }
    public int? ExpenseTemplateId { get; init; }
    public decimal Remaining { get; init; }

    public string MapKey()
    {
        var half = ToBePaidAt.Day <= 15 ? 1 : 2;
        return $"{ToBePaidAt.Year}-{ToBePaidAt.Month}-{half}";
    }
}