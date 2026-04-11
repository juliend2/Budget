using Budget.Web.Models;
using MySqlConnector;
using Dapper;
using System.Data.Common;

namespace Budget.Web.Repositories;

public interface IBudgetLines
{
    Task<IEnumerable<BudgetLine>> GetAllAsync();
    void CreateRepeatableExpenses();
}

public class BudgetLines : IBudgetLines
{
    private readonly string _connectionString;

    public BudgetLines(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<BudgetLine>> GetAllAsync()
    {
        using var connection = new MySqlConnection(_connectionString);

        const string sql = @"
            SELECT
                d.id AS Id,
                d.amount AS Amount,
                d.description AS Description,
                d.to_be_paid_at AS ToBePaidAt,
                p.paid_at AS PaidAt,
                d.expense_template_id AS ExpenseTemplateId,
                (d.amount - COALESCE(SUM(p.amount), 0)) AS Remaining
            FROM expenses d
            LEFT JOIN payments p ON d.id = p.expense_id
            GROUP BY d.id
            HAVING Remaining > 0 OR NOW() < d.to_be_paid_at
            ORDER BY d.to_be_paid_at";

        return await connection.QueryAsync<BudgetLine>(sql);
    }

    public async void CreateRepeatableExpenses()
    {
        using var connection = new MySqlConnection(_connectionString);
        
        const string sql = @"
            WITH
            grouped_expenses AS (
                SELECT
                    expense_template_id ,
                    MAX(to_be_paid_at) AS max_to_be_paid_at
                FROM expenses AS e
                GROUP BY e.expense_template_id
            ),
            expense_renewal_candidates AS (
                SELECT
                    e.id,
                    COALESCE(e.amount, et.amount) AS amount,
                    COALESCE(e.to_be_paid_at, '2000-01-01') AS ToBePaidAt, -- obviously due
                    COALESCE(e.description, et.description) AS description,
                    et.id AS ExpenseTemplateId,
                    et.repeatability_interval_unit,
                    et.repeatability_interval_pace
                FROM expenses AS e
                JOIN grouped_expenses AS ge ON e.to_be_paid_at = ge.max_to_be_paid_at
                LEFT JOIN expenses_templates AS et ON et.id = e.expense_template_id
                WHERE et.id IS NOT NULL
            )
            SELECT *,
                CASE
                    WHEN repeatability_interval_pace = 'W' THEN DATE_ADD(ToBePaidAt, INTERVAL CAST(repeatability_interval_unit AS SIGNED) WEEK)
                    WHEN repeatability_interval_pace = 'D' THEN DATE_ADD(ToBePaidAt, INTERVAL CAST(repeatability_interval_unit AS SIGNED) DAY)
                    WHEN repeatability_interval_pace = 'M' THEN DATE_ADD(ToBePaidAt, INTERVAL CAST(repeatability_interval_unit AS SIGNED) MONTH)
                    WHEN repeatability_interval_pace = 'Y' THEN DATE_ADD(ToBePaidAt, INTERVAL CAST(repeatability_interval_unit AS SIGNED) YEAR)
                END AS NextPaymentDate
            FROM expense_renewal_candidates";
        
        IEnumerable<ExpenseCandidate> candidates = await connection.QueryAsync<ExpenseCandidate>(sql);
        foreach (var c in candidates)
        {
            Console.WriteLine($"ici: {c.Id} {c.Description} {c.Amount} {c.NextPaymentDate}");
            var untilWhen = DateTime.Now.AddMonths(1);
            if (c.NextPaymentDate < untilWhen) // Avoids creating expenses for the next day
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO expenses
                        (amount, to_be_paid_at, description, expense_template_id)
                    VALUES
                        (@Amount, @ToBePaidAt, @Description, @ExpenseTemplateId)
                    ON DUPLICATE KEY UPDATE id = id ", // Avoids duplicates
                    new { Amount = c.Amount, ToBePaidAt = c.NextPaymentDate, Description = c.Description, ExpenseTemplateId = c.ExpenseTemplateId }
                );
            }
        }
    }

    public static List<List<BudgetLine>> SplitBudgetLinesByPay(List<BudgetLine> lines)
    {
        if (lines.Count() < 2)
        {
            return new List<List<BudgetLine>>(){ lines };
        }

        var dict = new Dictionary<string, List<BudgetLine>>();
        foreach (BudgetLine line in lines)
        {
            if (dict.ContainsKey(line.MapKey()))
            {
                dict[line.MapKey()].Add(line);
            }
            else
            {
                dict.Add(line.MapKey(), new List<BudgetLine>() { line });
            }
        }
        return dict
            .OrderBy(pair => pair.Key)
            .Select(pair => pair.Value)
            .ToList();
    }
}