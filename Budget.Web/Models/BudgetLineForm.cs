using System.ComponentModel.DataAnnotations;

namespace Budget.Web.Models;

public class BudgetLineForm
{
    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime ToBePaidAt { get; set; }
}
