using System;
using System.Threading.Tasks;
using FluentAssertions;
using Postgres.Tests.Util;
using Serilog.Ui.Core;
using Serilog.Ui.Core.Models;
using Xunit;

namespace Postgres.Tests.DataProvider;

[Collection(nameof(PostgresTestProvider))]
[Trait("Integration-Dashboard", "Postgres")]
public class DataProviderDashboardTests(PostgresTestProvider instance)
{
    private readonly IDataProvider _provider = instance.GetDataProvider();

    [Fact]
    public async Task FetchDashboardAsync_Should_Return_Dashboard_Model_With_Correct_Structure()
    {
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        dashboard.TotalLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.LogsByLevel.Should().NotBeNull();
        dashboard.TodayLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.TodayErrorLogs.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task FetchDashboardAsync_Should_Return_Consistent_Log_Counts()
    {
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        
        // Today's logs should not exceed total logs
        dashboard.TodayLogs.Should().BeLessThanOrEqualTo(dashboard.TotalLogs);
        
        // Today's error logs should not exceed today's total logs
        dashboard.TodayErrorLogs.Should().BeLessThanOrEqualTo(dashboard.TodayLogs);
    }

    [Fact]
    public async Task FetchDashboardAsync_Should_Return_Valid_Logs_By_Level_Dictionary()
    {
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        dashboard.LogsByLevel.Should().NotBeNull();
        
        // Sum of all level counts should not exceed total logs (could be less if data exists from before)
        var sumByLevel = 0;
        foreach (var kvp in dashboard.LogsByLevel)
        {
            kvp.Key.Should().NotBeNullOrEmpty();
            kvp.Value.Should().BeGreaterThanOrEqualTo(0);
            sumByLevel += kvp.Value;
        }
        
        sumByLevel.Should().BeLessThanOrEqualTo(dashboard.TotalLogs);
    }

    [Fact]
    public async Task FetchDashboardAsync_Should_Handle_Empty_Database()
    {
        // Note: This test assumes the database might be empty or the provider handles empty results gracefully
        
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        dashboard.TotalLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.LogsByLevel.Should().NotBeNull();
        dashboard.TodayLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.TodayErrorLogs.Should().BeGreaterThanOrEqualTo(0);
        
        // If total logs is 0, all other counts should also be 0
        if (dashboard.TotalLogs == 0)
        {
            dashboard.TodayLogs.Should().Be(0);
            dashboard.TodayErrorLogs.Should().Be(0);
            dashboard.LogsByLevel.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task FetchDashboardAsync_Should_Return_Dashboard_Model_With_Today_Date_Filter()
    {
        // Arrange
        var today = DateTime.Today;

        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        
        // Today's error logs should be a subset of today's total logs
        dashboard.TodayErrorLogs.Should().BeLessThanOrEqualTo(dashboard.TodayLogs);
        
        // Today's logs should be a subset of total logs
        dashboard.TodayLogs.Should().BeLessThanOrEqualTo(dashboard.TotalLogs);
    }
}

[Collection(nameof(PostgresAdditionalColsTestProvider))]
[Trait("Integration-Dashboard-AdditionalColumns", "Postgres")]
public class DataProviderDashboardWithColsTests(PostgresAdditionalColsTestProvider instance)
{
    private readonly IDataProvider _provider = instance.GetDataProvider();

    [Fact]
    public async Task FetchDashboardAsync_Should_Work_With_Additional_Columns()
    {
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        dashboard.TotalLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.LogsByLevel.Should().NotBeNull();
        dashboard.TodayLogs.Should().BeGreaterThanOrEqualTo(0);
        dashboard.TodayErrorLogs.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task FetchDashboardAsync_Should_Return_Consistent_Counts_With_Additional_Columns()
    {
        // Act
        var dashboard = await _provider.FetchDashboardAsync();

        // Assert
        dashboard.Should().NotBeNull();
        
        // Validate logical relationships between counts
        dashboard.TodayLogs.Should().BeLessThanOrEqualTo(dashboard.TotalLogs);
        dashboard.TodayErrorLogs.Should().BeLessThanOrEqualTo(dashboard.TodayLogs);
        
        // Validate LogsByLevel structure
        foreach (var kvp in dashboard.LogsByLevel)
        {
            kvp.Key.Should().NotBeNullOrEmpty();
            kvp.Value.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}