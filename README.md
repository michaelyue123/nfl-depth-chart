# NFL Depth Chart

A C#/.NET solution for managing a team's depth chart for the NFL Depth Chart
coding challenge.

## Requirements

The solution supports four operations:

- Add a player to a position at a specified depth, or append them to the end.
- Remove a player from a position.
- Get the backups listed after a player at a position.
- Retrieve and print the full depth chart.

A player can be listed at multiple positions. The challenge assumes jersey
numbers uniquely identify players within a team. Sample depths are zero-based.

## Getting started

### Prerequisite

- .NET 8 SDK

### Build

From the repository root:

```bash
dotnet build src/DepthChart/DepthChart.csproj
```

### Run the console demonstration

```bash
dotnet run --project src/DepthChart.Console/DepthChart.Console.csproj
```

The demonstration adds sample players, displays backups, removes players, and
prints the depth chart before and after removal.

### Run tests

```bash
dotnet test tests/DepthChart.Tests/DepthChart.Tests.csproj
```

The test project restores its NuGet dependencies when needed.

## Reference data

The supplied Tampa Bay Buccaneers depth chart is used as reference material.
The console demonstration manually defines a selection of players from the
chart; it does not load or parse the complete chart.

The reference chart contains repeated rows for some positions. The recruiter
confirmed that the specification does not prescribe how to model these rows.
This implementation maintains one list per exact position name and does not
load or consolidate repeated rows from the reference chart.

## Design

A `TeamDepthChart` instance manages one team's depth chart in memory using a
dictionary that maps each position to an ordered list of players. A player's
index in the list represents their depth.

`GetFullDepthChart` returns a snapshot of the chart. The console application
formats and prints the returned data.

## Implementation assumptions

The following are implementation choices where the challenge does not specify
the behavior:

- Depth is treated as a zero-based insertion index in a contiguous list.
- An index outside `0` through the current number of players at that position
  throws `ArgumentOutOfRangeException` without changing the chart.
- Adding a player whose jersey number is already listed at the same position is
  ignored. Depth is validated before checking for duplicates.
- A successful removal returns a list containing the removed player; an
  unsuccessful removal returns an empty list.
- Removing the last player from a position leaves that position in the chart
  with an empty list.
- The implementation assumes the supplied player data respects the challenge's
  team-wide jersey-number uniqueness rule; it does not enforce that rule across
  different positions.

## Position matching and sample inconsistencies

The recruiter confirmed that position names must match exactly. A player is
listed at a position only if they were added at that position.

Some challenge examples use a different position from the one where a player
was added. For example, Jaelon Darden is added under `LWR` but queried under
`QB`, and Mike Evans is added under `LWR` but removed from `WR`. Following the
written rules and recruiter clarification, these calls do not find the player
at the queried position.

## Future considerations

- **Multiple teams:** Manage a separate `TeamDepthChart` instance for each team,
  identified by a team ID. Jersey-number uniqueness would remain scoped to a
  team.
- **Other sports:** Position names are not restricted to NFL values. Sports
  that use the same position-and-ordered-player model could reuse the core
  behavior; sport-specific rules would need further analysis.
- **Persistence and concurrency:** The current implementation is in-memory.
  Persistent storage and concurrent updates would require additional design
  if the application were expanded.