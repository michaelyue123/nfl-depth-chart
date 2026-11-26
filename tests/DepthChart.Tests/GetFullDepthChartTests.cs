using Xunit;

namespace DepthChart.Tests;

public class GetFullDepthChartTests
{
    private readonly TeamDepthChart _depthChart = new();
    private readonly Player _tomBrady = TestPlayers.TomBrady;
    private readonly Player _blaineGabbert = TestPlayers.BlaineGabbert;
    private readonly Player _kyleTrask = TestPlayers.KyleTrask;

    [Fact]
    public void GetFullDepthChart_EmptyChart_ReturnsEmptyDictionary()
    {
        // Arrange & Act 
        var chart = _depthChart.GetFullDepthChart();

        // Assert
        Assert.Empty(chart);
    }

    [Fact]
    public void GetFullDepthChart_MultiplePositions_ReturnsPlayersInOrder()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("RT", _blaineGabbert);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);
        _depthChart.AddPlayerToDepthChart("RT", _tomBrady);

        // Act
        var chart = _depthChart.GetFullDepthChart();

        // Assert
        Assert.Equal([_tomBrady, _kyleTrask], chart["QB"]);
        Assert.Equal([_blaineGabbert, _tomBrady], chart["RT"]);
    }

    [Fact]
    public void GetFullDepthChart_ChartChangesAfterRetrieval_PreviousSnapshotRemainsUnchanged()
    {
        // Arrange & Act
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        var oldChart = _depthChart.GetFullDepthChart();

        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);
        var latestChart = _depthChart.GetFullDepthChart();
        
        // Assert
        Assert.Equal([_tomBrady], oldChart["QB"]);
        Assert.Equal([_tomBrady, _blaineGabbert], latestChart["QB"]);
    }
}