using FakeItEasy;
using Budget.Web.Repositories;
using Budget.Web.Models;

namespace Budget.Tests;

public class ExpenseServiceTests
{
    [Fact]
    public async Task Should_Return_Only_Pending_Expenses()
    {
        // Arrange
        var mockRepo = A.Fake<IBudgetLines>();
        var fakeData = new List<BudgetLine>
        {
            new BudgetLine(1, 100, "Internet", DateTime.Now.AddDays(5), null, null, 100)
        };
        A.CallTo(() => mockRepo.GetAllAsync()).Returns(fakeData);

        // Act
        var result = await mockRepo.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(100, result.First().Remaining);
    }

    [Fact]
    public async Task Should_Return_One_Split_Of_BudgetLine()
    {
        // Arrange
        var data = new List<BudgetLine>() {
            // No splitting since it's the start of the month:
            new BudgetLine(1, 100, "Internet", new DateTime(2026, 04, 01), null, null, 100),
            new BudgetLine(1, 70, "Cafe", new DateTime(2026, 04, 02), null, null, 30)
        };

        // Act
        var results = BudgetLines.SplitBudgetLinesByPay(data);

        // Assert
        Assert.Single(results);
    }

    [Fact]
    public async Task Should_Return_Two_Split_BudgetLines()
    {
        // Arrange
        var data = new List<BudgetLine>()
        {
            new BudgetLine(1, 100, "Internet", new DateTime(2026, 04, 01), null, null, 100),
            // Will split since it's start vs end of the month
            new BudgetLine(1, 70, "Cafe", new DateTime(2026, 04, 29), null, null, 30)
        };

        // Act
        var results = BudgetLines.SplitBudgetLinesByPay(data);

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task Should_Keep_The_BudgetLine_Splits_Order()
    {
        // Arrange
        var data = new List<BudgetLine>()
        {
            // Put those in the wrong order, to make sure GetAllSplitAsync orders things properly:
            new BudgetLine(1, 70, "Cafe", new DateTime(2026, 04, 29), null, null, 30),
            new BudgetLine(1, 100, "Internet", new DateTime(2026, 04, 01), null, null, 100),
        };

        // Act
        var results = BudgetLines.SplitBudgetLinesByPay(data);

        // Assert
        Assert.Equal(new DateTime(2026, 04, 01), results[0][0].ToBePaidAt);
        Assert.Equal(new DateTime(2026, 04, 29), results[1][0].ToBePaidAt);
    }
}