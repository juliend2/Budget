using System.ComponentModel.DataAnnotations;

namespace Budget.Web.Models;

public class ExpenseTemplateForm
{
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime InitialToBePaidOn { get; set; }

    [Required]
    public int RepeatabilityIntervalUnit { get; set; }

    [Required]
    public int RepeatabilityIntervalPace { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public bool IsOnHold { get; set; }
}