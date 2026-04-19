using Budget.Web.Models;
using MySqlConnector;
using Dapper;

namespace Budget.Web.Repositories;

public interface IExpenseTemplates
{
    Task<IEnumerable<ExpenseTemplate>> GetAllAsync();
    Task<ExpenseTemplate?> GetByIdAsync(int id);
    Task CreateAsync(ExpenseTemplateForm form);
    Task UpdateAsync(ExpenseTemplateForm form);
    Task DeleteAsync(int id);
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

    public async Task<ExpenseTemplate?> GetByIdAsync(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<ExpenseTemplate>(
            @"SELECT    id,
                        description,
                        amount,
                        initial_to_be_paid_on AS InitialToBePaidOn,
                        repeatability_interval_unit AS RepeatabilityIntervalUnit,
                        repeatability_interval_pace AS RepeatabilityIntervalPace,
                        is_on_hold AS IsOnHold
              FROM expense_templates
              WHERE id = @Id",
            new { Id = id });
    }

    public async Task CreateAsync(ExpenseTemplateForm form)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"INSERT INTO expense_templates
                (description, amount, initial_to_be_paid_on, repeatability_interval_unit, repeatability_interval_pace, is_on_hold)
              VALUES
                (@Description, @Amount, @InitialToBePaidOn, @RepeatabilityIntervalUnit, @RepeatabilityIntervalPace, @IsOnHold)",
            new {
                form.Description,
                form.Amount,
                form.InitialToBePaidOn,
                form.RepeatabilityIntervalUnit,
                form.RepeatabilityIntervalPace,
                form.IsOnHold
            });
    }

    public async Task UpdateAsync(ExpenseTemplateForm form)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"UPDATE expense_templates
              SET
                description = @Description,
                amount = @Amount,
                initial_to_be_paid_on = @InitialToBePaidOn,
                repeatability_interval_unit = @RepeatabilityIntervalUnit,
                repeatability_interval_pace = @RepeatabilityIntervalPace,
                is_on_hold = @IsOnHold
              WHERE id = @Id",
            new {
                form.Description,
                form.Amount,
                form.InitialToBePaidOn,
                form.RepeatabilityIntervalUnit,
                form.RepeatabilityIntervalPace,
                form.IsOnHold,
                form.Id
            });
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "DELETE FROM expense_templates WHERE id = @Id",
            new { Id = id });
    }
}