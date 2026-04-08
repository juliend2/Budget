using Budget.Web.Models;

namespace Budget.Web.Repositories;

public interface IBudgetLines
{
    Task<List<BudgetLine>> GetAllAsync();
}

public class BudgetLines : IBudgetLines
{
    public Task<List<BudgetLine>> GetAllAsync()
    {
        return Task.FromResult(new List<BudgetLine>());
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