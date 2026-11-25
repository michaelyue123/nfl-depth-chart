namespace DepthChart;

public sealed class TeamDepthChart
{
    readonly Dictionary<string, List<Player>> depthChartRecord = new Dictionary<string, List<Player>>();

    // positionDepth is a zero-based index; null means append.
    public void AddPlayerToDepthChart(string position, Player player, int? positionDepth = null)
    {
        bool positionExists = depthChartRecord.TryGetValue(position, out var positionRecord);

        positionRecord ??= new List<Player>();

        // Append to the end when no depth is specified.
        int insertIndex = positionDepth ?? positionRecord.Count;

        // Reject a depth outside the valid insertion range.
        if (insertIndex < 0 || insertIndex > positionRecord.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(positionDepth),
                "Position depth is out of the current range.");
        }

        // Ignore a player who is already listed at this position.
        if (positionRecord.Any(existing =>
            existing.JerseyNumber == player.JerseyNumber))
        {
            return;
        }

        positionRecord.Insert(insertIndex, player);

        // Add a new position only after the player is inserted successfully.
        if (!positionExists)
        {
            depthChartRecord.Add(position, positionRecord);
        }
    }

    // The specification describes a player on success and an empty list on failure.
    public IReadOnlyList<Player> RemovePlayerFromDepthChart(string position, Player player)
    {   
        // Return an empty list if the position does not exist.
        if(!depthChartRecord.TryGetValue(position, out var positionRecord))
        {
            return [];
        }

        int index = positionRecord.FindIndex(existing => existing.JerseyNumber == player.JerseyNumber);

        // Return an empty list if the player is not listed
        if(index == -1)
        {   
            return [];
        }

        Player removedPlayer = positionRecord[index];
        positionRecord.RemoveAt(index);

        return [removedPlayer];
    }

    public IReadOnlyList<Player> GetBackups(string position, Player player)
    {
        // Return an empty list if the position does not exist.
        if(!depthChartRecord.TryGetValue(position, out var positionRecord))
        {
            return [];
        }

        int index = positionRecord.FindIndex(existing => existing.JerseyNumber == player.JerseyNumber);

        // Return an empty list if the player is not listed or has no backups.
        if(index == -1 || index == positionRecord.Count-1)
        {
            return [];
        }

        // Return players after index in order
        return positionRecord[(index + 1)..].AsReadOnly();
    }

    // Returns chart data; presentation/printing can be handled separately.
    public IReadOnlyDictionary<string, IReadOnlyList<Player>> GetFullDepthChart()
    {
        return depthChartRecord.ToDictionary(
            entry => entry.Key,
            entry => (IReadOnlyList<Player>) entry.Value.ToArray()
        );
    }
}
