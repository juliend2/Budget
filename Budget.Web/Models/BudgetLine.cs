namespace Budget.Web.Models;

public record BudgetLine
{
    public BudgetLine() { } // Empty constructor for Dapper
    
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

    public DateTime PayDate()
    {
        var date = ToBePaidAt;
        if (date.Day <= 15)
        {
            return new DateTime(date.Year, date.Month, 15);
        }
        else
        {
            int lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, lastDay);
        }
    }
}