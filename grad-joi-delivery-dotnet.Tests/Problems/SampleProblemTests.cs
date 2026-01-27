using grad_joi_delivery_dotnet.Problems;
using Xunit;

namespace grad_joi_delivery_dotnet.Tests.Problems;

public class SampleProblemTests
{
    [Fact]
    public void CalculateCost_WhenDistanceIs5Km_ReturnsBaseCost()
    {
        double distance = 5.0;
        double expectedCost = 50.0;

        double actualCost = SampleProblem.CalculateCost(distance);

        Assert.Equal(expectedCost, actualCost);
    }

    [Fact]
    public void CalculateCost_WhenDistanceIsLessThan5Km_ReturnsBaseCost()
    {
        double distance = 3.0;
        double expectedCost = 50.0;

        double actualCost = SampleProblem.CalculateCost(distance);

        Assert.Equal(expectedCost, actualCost);
    }

    [Fact]
    public void CalculateCost_WhenDistanceIsGreaterThan5Km_ReturnsBaseCostPlusExtra()
    {
        double distance = 8.0;
        double expectedCost = 50.0 + (8.0 - 5.0) * 10.0; // 50 + 30 = 80

        double actualCost = SampleProblem.CalculateCost(distance);

        Assert.Equal(expectedCost, actualCost);
    }

    [Fact]
    public void CalculateCost_WhenDistanceIsZero_ThrowsArgumentException()
    {
        double distance = 0.0;

        Assert.Throws<ArgumentException>(() => SampleProblem.CalculateCost(distance));
    }

    [Fact]
    public void CalculateCost_WhenDistanceIsNegative_ThrowsArgumentException()
    {
        double distance = -5.0;

        Assert.Throws<ArgumentException>(() => SampleProblem.CalculateCost(distance));
    }
}
