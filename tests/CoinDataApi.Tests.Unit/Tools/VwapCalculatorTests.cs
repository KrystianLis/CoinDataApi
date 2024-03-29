﻿using CoinDataApi.Application.Tools;
using CoinDataApi.Core.Models;
using FluentAssertions;
using Xunit;

namespace CoinDataApi.Tests.Unit.Tools;

public class VwapCalculatorTests
{
    [Fact]
    public void GivenValidData_WhenCalculatingVwap_ThenReturnsCorrectVwap()
    {
        // Arrange
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 100m, TotalVolume = 10m },
            new() { ClosePrice = 200m, TotalVolume = 20m }
        };

        // Act
        var result = VwapCalculator.CalculateVwap(data);

        // Assert
        result.Should().BeApproximately(166.67m, 0.01m);
    }

    [Fact]
    public void GivenEmptyData_WhenCalculatingVwap_ThenReturnsZero()
    {
        // Arrange
        var data = new List<OhlcvData>();

        // Act
        var result = VwapCalculator.CalculateVwap(data);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GivenDataWithZeroTotalVolume_WhenCalculatingVwap_ThenReturnsZero()
    {
        // Arrange
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 100m, TotalVolume = 0m },
            new() { ClosePrice = 200m, TotalVolume = 0m }
        };

        // Act
        var result = VwapCalculator.CalculateVwap(data);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GivenValidData_WhenCalculatingStandardDeviation_ThenReturnsCorrectStandardDeviation()
    {
        // Arrange
        var vwap = 166.67m;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 160m, TotalVolume = 10m },
            new() { ClosePrice = 170m, TotalVolume = 20m },
            new() { ClosePrice = 165m, TotalVolume = 30m }
        };

        // Act
        var result = VwapCalculator.CalculateStandardDeviation(data, vwap);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GivenEmptyData_WhenCalculatingStandardDeviation_ThenReturnsZero()
    {
        // Arrange
        var vwap = 166.67m;
        var data = new List<OhlcvData>();

        // Act
        var result = VwapCalculator.CalculateStandardDeviation(data, vwap);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GivenSingleDataPoint_WhenCalculatingStandardDeviation_ThenReturnsZero()
    {
        // Arrange
        var vwap = 166.67m;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = vwap, TotalVolume = 10m }
        };

        // Act
        var result = VwapCalculator.CalculateStandardDeviation(data, vwap);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GivenVwapEqualToDataPoints_WhenCalculatingStandardDeviation_ThenReturnsZero()
    {
        // Arrange
        var vwap = 166.67m;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = vwap, TotalVolume = 10m },
            new() { ClosePrice = vwap, TotalVolume = 20m }
        };

        // Act
        var result = VwapCalculator.CalculateStandardDeviation(data, vwap);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GivenDataWithHighDeviation_WhenFindingPoints_ThenReturnsPointsAboveStandardDeviation()
    {
        // Arrange
        var vwap = 150m;
        var standardDeviation = 20.0;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 130m, TotalVolume = 10m },
            new() { ClosePrice = 180m, TotalVolume = 20m },
            new() { ClosePrice = 140m, TotalVolume = 30m }
        };

        // Act
        var result = VwapCalculator.FindPointsWithHighDeviation(data, vwap, standardDeviation);

        // Assert
        result.Should().ContainSingle().And.Contain(x => x.ClosePrice == 180m);
    }

    [Fact]
    public void GivenNoDataWithHighDeviation_WhenFindingPoints_ThenReturnsEmptyCollection()
    {
        // Arrange
        var vwap = 150m;
        var standardDeviation = 50.0;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 130m, TotalVolume = 10m },
            new() { ClosePrice = 140m, TotalVolume = 20m }
        };

        // Act
        var result = VwapCalculator.FindPointsWithHighDeviation(data, vwap, standardDeviation);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GivenEmptyData_WhenFindingPoints_ThenReturnsEmptyCollection()
    {
        // Arrange
        var vwap = 150m;
        var standardDeviation = 20.0;
        var data = new List<OhlcvData>();

        // Act
        var result = VwapCalculator.FindPointsWithHighDeviation(data, vwap, standardDeviation);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GivenAllDataWithHighDeviation_WhenFindingPoints_ThenReturnsAllPoints()
    {
        // Arrange
        var vwap = 100m;
        var standardDeviation = 10.0;
        var data = new List<OhlcvData>
        {
            new() { ClosePrice = 120m, TotalVolume = 10m },
            new() { ClosePrice = 130m, TotalVolume = 20m }
        };

        // Act
        var result = VwapCalculator.FindPointsWithHighDeviation(data, vwap, standardDeviation);

        // Assert
        result.Should().HaveCount(2).And.Contain(x => x.ClosePrice == 120m || x.ClosePrice == 130m);
    }
}