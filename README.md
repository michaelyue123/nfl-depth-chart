# NFL Depth Chart

This repository contains a C#/.NET implementation in progress for the NFL
Depth Chart coding challenge.

## Challenge requirements

The challenge describes four operations:

- Add a player at a position and depth.
- Remove a player from a position.
- Get the backups listed after a player at a position.
- Print the full depth chart.

A player may be listed at multiple positions. Jersey numbers uniquely identify
players within a team. The sample calls use zero-based depths.

## Recruiter clarifications

- Position names must match exactly.
- The supplied Tampa Bay depth chart is reference material; the specification
  does not require either manual preparation or parsing it.
- The specification does not prescribe how repeated position rows should be
  modelled. This project currently has one list per position; it does not load
  or consolidate reference-data rows.

## Sample inconsistencies

Some sample calls use a different position from the one where the player was
added. For example, Jaelon Darden is added to LWR but queried under QB, and
Mike Evans is added to LWR but removed from WR. Under the written rules and the
recruiter's exact-position clarification, these calls should not find the
player at the queried position.

## Implementation assumptions

These are implementation choices, not explicit challenge requirements:

- Depth is a zero-based insertion index in a contiguous list. Omitting the index
  appends the player.
- An index outside `0` through the current player count throws
  `ArgumentOutOfRangeException` and leaves the chart unchanged.
- Adding a player whose jersey number is already present at the same position
  is ignored.
- Depth validation happens before the duplicate-player check.
- A successful removal returns a list containing the removed player; an
  unsuccessful removal returns an empty list.
- Removing the last player from a position leaves that position in the chart
  with an empty list.

## Current implementation

`TeamDepthChart` supports adding and removing players and retrieving the chart
as data. `GetFullDepthChart` returns a snapshot keyed by position, with each
position's players in list order. It does not print or format the chart.

Getting backups is not implemented yet and currently throws
`NotImplementedException`. The add operation checks duplicate jersey numbers
only within the selected position, so team-wide jersey-number uniqueness is
not currently enforced. Position keys are matched exactly.

The Tampa Bay reference chart is not present as project data and is not parsed
or manually loaded. A new chart starts empty; the tests populate it with
players directly.

The current tests cover adding and removing players, insertion and removal
order, duplicate handling, multiple positions, and invalid depths. They do not
yet cover backups.

## Getting started

Prerequisite: .NET 8 SDK.

From the repository root, build the library and run its tests:

```bash
dotnet build src/DepthChart/DepthChart.csproj
dotnet test tests/DepthChart.Tests/DepthChart.Tests.csproj
```

The test project restores its NuGet dependencies when needed.