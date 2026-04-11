using Budget.Web.Models;
using MySqlConnector;
using Dapper;

namespace Budget.Web.Repositories;

public interface IPayments
{
    Task CreateAsync(PaymentForm form);
}

public class Payments : IPayments
{
    private readonly string _connectionString;

    public Payments(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task CreateAsync(PaymentForm form)
    {
        using var connection = new MySqlConnection(_connectionString);

        await connection.ExecuteAsync(
            @"INSERT INTO payments (amount, paid_at, expense_id)
              VALUES (@Amount, @PaidAt, @ExpenseId)",
            new { Amount = form.Amount, PaidAt = DateTime.Now, ExpenseId = form.ExpenseId });
    }

}