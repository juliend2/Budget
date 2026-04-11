using System.ComponentModel.DataAnnotations;

namespace Budget.Web.Models;

public class PaymentForm
{
    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaidAt { get; set; }

    [Required]
    public int ExpenseId { get; set; }
}
