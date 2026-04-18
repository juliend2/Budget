namespace Budget.Web.Models;

public record Expense
{
    public Expense() { } // Empty constructor for Dapper

    public int Id { get; init; }
    public float Amount { get; init; }
    public DateTime ToBePaidAt { get; init; }
    public string Description { get; init; } = string.Empty;
    public int ExpenseTemplateId { get; init; }
}