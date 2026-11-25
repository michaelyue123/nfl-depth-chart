namespace DepthChart;

public sealed class TeamDepthChart
{
    private readonly Dictionary<string, List<Player>> depthChartRecord = new();

    // positionDepth is a zero-based index; null means append.
    public void AddPlayerToDepthChart(
        string position,
        Player player,
        int? positionDepth = null)
    {
        bool positionExists = depthChartRecord.TryGetValue(position, out var positionRecord);

        positionRecord ??= new List<Player>();

        int insertIndex = positionDepth ?? positionRecord.Count;

        if (insertIndex < 0 || insertIndex > positionRecord.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(positionDepth),
                "Position depth is out of the current range.");
        }

        // A duplicate at the same position is ignored.
        if (positionRecord.Any(existing =>
            existing.JerseyNumber == player.JerseyNumber))
        {
            return;
        }

        positionRecord.Insert(insertIndex, player);

        // Add a new position only after validation succeeds.
        if (!positionExists)
        {
            depthChartRecord.Add(position, positionRecord);
        }
    }

    // Returns the removed player in a list, or an empty list if not found.
    public IReadOnlyList<Player> RemovePlayerFromDepthChart(
        string position,
        Player player)
    {
        if (!depthChartRecord.TryGetValue(position, out var positionRecord))
        {
            return [];
        }

        int index = positionRecord.FindIndex(
            existing => existing.JerseyNumber == player.JerseyNumber);

        if (index == -1)
        {
            return [];
        }

        Player removedPlayer = positionRecord[index];
        positionRecord.RemoveAt(index);

        return [removedPlayer];
    }

    public IReadOnlyList<Player> GetBackups(string position, Player player)
    {
        if (!depthChartRecord.TryGetValue(position, out var positionRecord))
        {
            return [];
        }

        int index = positionRecord.FindIndex(
            existing => existing.JerseyNumber == player.JerseyNumber);

        if (index == -1 || index == positionRecord.Count - 1)
        {
            return [];
        }

        return positionRecord[(index + 1)..].AsReadOnly();
    }

    public IReadOnlyDictionary<string, IReadOnlyList<Player>> GetFullDepthChart()
    {
        return depthChartRecord.ToDictionary(
            entry => entry.Key,
            entry => (IReadOnlyList<Player>)entry.Value.ToArray());
    }
}