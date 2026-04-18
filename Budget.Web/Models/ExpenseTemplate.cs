namespace Budget.Web.Models;

public record ExpenseTemplate
{
    public ExpenseTemplate() { } // Empty constructor for Dapper

    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime InitialToBePaidOn { get; init; } = DateTime.Today;
    public int RepeatabilityIntervalUnit { get; init; }
    public string RepeatabilityIntervalPace { get; init; } = string.Empty;
    public int Amount { get; init; }
    public bool IsOnHold { get; init; }
}