using Xunit;

namespace DepthChart.Tests;

public class RemovePlayerFromDepthChartTests
{
    private readonly TeamDepthChart _depthChart = new();

    private readonly Player _tomBrady = new(12, "Tom Brady");
    private readonly Player _blaineGabbert = new(11, "Blaine Gabbert");
    private readonly Player _kyleTrask = new(2, "Kyle Trask");

    [Fact]
    public void RemovePlayer_ExistingPlayer_ReturnsRemovedPlayerAndUpdatesChart()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);

        // Act 
        var player = _depthChart.RemovePlayerFromDepthChart("QB", _tomBrady);
        var chart = _depthChart.GetFullDepthChart();

        // Assert
        Assert.Equal([_tomBrady], player);
        Assert.Equal([_blaineGabbert, _kyleTrask], chart["QB"]);
    }

    [Fact]
    public void RemovePlayer_PositionDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        var player = _depthChart.RemovePlayerFromDepthChart("RT", _tomBrady);

        // Assert
        Assert.Equal([], player);
    }

    [Fact]
    public void RemovePlayer_PlayerNotAtPosition_ReturnsEmptyList()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);

        // Act
        var player = _depthChart.RemovePlayerFromDepthChart("QB", _tomBrady);

        // Assert
        Assert.Equal([], player);
    }

    [Fact]
    public void RemovePlayer_PlayerAtMultiplePositions_OnlyRemovesFromSpecifiedPosition()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);
        _depthChart.AddPlayerToDepthChart("RT", _kyleTrask);

        // Act
        var player = _depthChart.RemovePlayerFromDepthChart("QB", _kyleTrask);
        var chart = _depthChart.GetFullDepthChart();

        // Assert
        Assert.Equal([_kyleTrask], player);
        Assert.Contains(_kyleTrask, chart["RT"]);
        Assert.Empty(chart["QB"]);
    }
}