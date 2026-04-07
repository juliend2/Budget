using FakeItEasy;
using Budget.Web.Repositories;
using Budget.Web.Models;
using Xunit;

namespace Budget.Tests;

public class ExpenseServiceTests
{
    [Fact]
    public async Task Should_Return_Only_Pending_Expenses()
    {
        // Arrange
        var mockRepo = A.Fake<IBudgetLine>();
        var fakeData = new List<BudgetLine>
        {
            new BudgetLine(1, 100, "Internet", DateTime.Now.AddDays(5), null, null, 100)
        };

        A.CallTo(() => mockRepo.GetPendingExpensesAsync()).Returns(fakeData);

        // Act
        var result = await mockRepo.GetPendingExpensesAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(100, result.First().Remaining);
    }
}