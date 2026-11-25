using Xunit;

namespace DepthChart.Tests;

public class AddPlayerToDepthChartTests
{
    private readonly TeamDepthChart _depthChart = new();
    private readonly Player _tomBrady = TestPlayers.TomBrady;
    private readonly Player _blaineGabbert = TestPlayers.BlaineGabbert;
    private readonly Player _kyleTrask = TestPlayers.KyleTrask;

    [Fact]
    public void AddPlayer_NewPosition_AddsPlayerToDepthChart()
    {
        // Act
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Assert
        var chart = _depthChart.GetFullDepthChart();

        Assert.True(chart.ContainsKey("QB"));
        Assert.Equal([_tomBrady], chart["QB"]);
    }

    [Fact]
    public void AddPlayer_WithoutDepth_AppendsPlayer()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal([_tomBrady, _blaineGabbert], players);
    }

    [Fact]
    public void AddPlayer_AtSpecifiedDepth_ShiftsPlayersDown()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);

        // Act
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert, 1);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal(
            [_tomBrady, _blaineGabbert, _kyleTrask],
            players);
    }

    [Fact]
    public void AddPlayer_AtExistingPosition_AndDepthZero_InsertsFirst()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);

        // Act
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady, 0);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal([_tomBrady, _blaineGabbert], players);
    }

    [Fact]
    public void AddPlayer_AtListCount_AppendsPlayer()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert, 1);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal([_tomBrady, _blaineGabbert], players);
    }

    [Fact]
    public void AddPlayer_SamePlayerAtSamePosition_DoesNotAddDuplicate()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal([_tomBrady], players);
    }

    [Fact]
    public void AddPlayer_SameJerseyNumberAtSamePosition_DoesNotAddDuplicate()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Same number, different name: the number identifies the player.
        var sameNumber = new Player(12, "Different Name");

        // Act
        _depthChart.AddPlayerToDepthChart("QB", sameNumber, 0);

        // Assert
        var players = _depthChart.GetFullDepthChart()["QB"];

        Assert.Equal([_tomBrady], players);
    }

    [Fact]
    public void AddPlayer_SamePlayerAtDifferentPositions_AddsToBoth()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("LT", _tomBrady);

        // Act
        _depthChart.AddPlayerToDepthChart("RT", _tomBrady);

        // Assert
        var chart = _depthChart.GetFullDepthChart();

        Assert.Equal([_tomBrady], chart["LT"]);
        Assert.Equal([_tomBrady], chart["RT"]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void AddPlayer_InvalidDepth_ThrowsAndDoesNotChangeExistingPosition(
        int invalidDepth)
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _depthChart.AddPlayerToDepthChart(
                "QB",
                _blaineGabbert,
                invalidDepth));

        var players = _depthChart.GetFullDepthChart()["QB"];
        Assert.Equal([_tomBrady], players);
    }

    [Fact]
    public void AddPlayer_InvalidDepthForNewPosition_DoesNotCreatePosition()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _depthChart.AddPlayerToDepthChart("QB", _tomBrady, 1));

        var chart = _depthChart.GetFullDepthChart();
        Assert.False(chart.ContainsKey("QB"));
    }
}