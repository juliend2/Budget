using Budget.Web.Models;
using MySqlConnector;
using Dapper;

namespace Budget.Web.Repositories;

public interface IExpenseTemplates
{
    Task<IEnumerable<ExpenseTemplate>> GetAllAsync();
    Task CreateAsync(ExpenseTemplateForm form);
}

public class ExpenseTemplates : IExpenseTemplates
{
    private readonly string _connectionString;

    public ExpenseTemplates(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<ExpenseTemplate>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_connectionString);
        return await connection.QueryAsync<ExpenseTemplate>(
            @"SELECT    id,
                        description,
                        amount,
                        initial_to_be_paid_on AS InitialToBePaidOn,
                        repeatability_interval_unit AS RepeatabilityIntervalUnit,
                        repeatability_interval_pace AS RepeatabilityIntervalPace,
                        is_on_hold AS IsOnHold
              FROM expense_templates");
    }

    public async Task CreateAsync(ExpenseTemplateForm form)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"INSERT INTO expense_templates (description, amount)
              VALUES (@Description, @Amount)",
            new { form.Description, form.Amount });
    }

    public async Task UpdateAsync(ExpenseTemplateForm form)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"UPDATE expense_templates
              SET description = @Description, amount = @Amount
              WHERE id = @Id",
            new { form.Description, form.Amount, form.Id });
    }
}