using Budget.Web.Models;
using MySqlConnector;
using Dapper;

namespace Budget.Web.Repositories;

public interface IExpenses
{
    Task<Int32> GetExpenseRemainingAmountAsync(Int32 expenseId);
}

public class Expenses : IExpenses
{
    private readonly string _connectionString;

    public Expenses(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<Int32> GetExpenseRemainingAmountAsync(Int32 expenseId)
    {
        using var connection = new MySqlConnection(_connectionString);

        var totalAmount = await connection.QuerySingleOrDefaultAsync<decimal>(
            @"SELECT amount
              FROM expenses
              WHERE id = @ExpenseId",
            new { ExpenseId = expenseId });

        var paidAmount = await connection.QuerySingleOrDefaultAsync<decimal>(
            @"SELECT COALESCE(SUM(amount), 0) AS sum
              FROM payments
              WHERE expense_id = @ExpenseId",
            new { ExpenseId = expenseId });

        return (int)(totalAmount - paidAmount);
    }
}