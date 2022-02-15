using Application.Common.Models;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests;

public class SquareCalculationServiceTests
{
    [Fact]
    public async Task CalculateSquaresAsync_Input1_ShouldReturnOneSquare()
    {
        // Arrange
        var service = new SquareCalculationService();
        var points = new List<PointModel>
        {
            new PointModel(1, 1),
            new PointModel(-1, 1),
            new PointModel(1, -1),
            new PointModel(-1, -1)
        };

        // Act
        var squares = await service.CalculateSquaresAsync(points);

        // Assert
        Assert.Single(squares);
    }

    [Fact]
    public async Task CalculateSquaresAsync_Input2_ShouldReturnTwoSquares()
    {
        // Arrange
        var service = new SquareCalculationService();
        var points = new List<PointModel>
        {
            new PointModel(1, 1),
            new PointModel(-1, 1),
            new PointModel(1, -1),
            new PointModel(-1, -1),
            new PointModel(0, 0),
            new PointModel(0, 1),
            new PointModel(1, 0),
        };

        // Act
        var squares = await service.CalculateSquaresAsync(points);

        // Assert
        Assert.Equal(2, squares.Count());
    }

    [Fact]
    public async Task CalculateSquaresAsync_Input2_WithAdditionalPoint_ShouldReturnTwoSquares()
    {
        // Arrange
        var service = new SquareCalculationService();
        var points = new List<PointModel>
        {
            new PointModel(1, 1),
            new PointModel(-1, 1),
            new PointModel(1, -1),
            new PointModel(-1, -1),
            new PointModel(0, 0),
            new PointModel(0, 1),
            new PointModel(1, 0),
            new PointModel(1, 5),
        };

        // Act
        var squares = await service.CalculateSquaresAsync(points);

        // Assert
        Assert.Equal(2, squares.Count());
    }
}
