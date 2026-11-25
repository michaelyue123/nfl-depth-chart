using Xunit;

namespace DepthChart.Tests;

public class GetBackupsTests
{
    private readonly TeamDepthChart _depthChart = new();
    private readonly Player _tomBrady = TestPlayers.TomBrady;
    private readonly Player _blaineGabbert = TestPlayers.BlaineGabbert;
    private readonly Player _kyleTrask = TestPlayers.KyleTrask;

    [Fact]
    public void GetBackups_PlayerIsFirst_ReturnsAllPlayersAfterInOrder()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);

        // Act
        var backups = _depthChart.GetBackups("QB", _tomBrady);

        // Assert
        Assert.Equal([_blaineGabbert, _kyleTrask], backups);
    }

    [Fact]
    public void GetBackups_PlayerIsInMiddle_ReturnsOnlyPlayersAfter()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);

        // Act
        var backups = _depthChart.GetBackups("QB", _blaineGabbert);

        // Assert
        Assert.Equal([_kyleTrask], backups);
    }

    [Fact]
    public void GetBackups_PlayerIsLast_ReturnsEmptyList()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);
        _depthChart.AddPlayerToDepthChart("QB", _blaineGabbert);
        _depthChart.AddPlayerToDepthChart("QB", _kyleTrask);

        // Act
        var backups = _depthChart.GetBackups("QB", _kyleTrask);

        // Assert
        Assert.Empty(backups);
    }

    [Fact]
    public void GetBackups_PlayerNotAtPosition_ReturnsEmptyList()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        var backups = _depthChart.GetBackups("QB", _kyleTrask);

        // Assert
        Assert.Empty(backups);
    }

    [Fact]
    public void GetBackups_PositionDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _depthChart.AddPlayerToDepthChart("QB", _tomBrady);

        // Act
        var backups = _depthChart.GetBackups("RT", _tomBrady);

        // Assert
        Assert.Empty(backups);
    }
}