using Budget.Web.Models;
using MySqlConnector;
using Dapper;

namespace Budget.Web.Repositories;

public interface IBudgetLines
{
    Task<IEnumerable<BudgetLine>> GetAllAsync();
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

    public static List<List<BudgetLine>> GetAllSplitAsync(List<BudgetLine> lines)
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