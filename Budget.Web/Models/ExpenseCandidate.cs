namespace Budget.Web.Models;

public record ExpenseCandidate
{
    public ExpenseCandidate() { } // Empty constructor for Dapper

    public int Id { get; init; }
    public float Amount { get; init; }
    public DateTime ToBePaidAt { get; init; }
    public string Description { get; init; } = string.Empty;
    public int ExpenseTemplateId { get; init; }
    public int RepeatabilityIntervalUnit { get; init; }
    public string RepeatabilityIntervalPace { get; init; } = string.Empty;
    public DateTime NextPaymentDate { get; init; }
}