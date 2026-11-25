using DepthChart;

var depthChart = new TeamDepthChart();

// QB
var tomBrady = new Player(12, "Tom Brady");
var blaineGabbert = new Player(11, "Blaine Gabbert");
var kyleTrask = new Player(2, "Kyle Trask");

// LWR
var mikeEvans = new Player(13, "Mike Evans");
var jaelonDarden = new Player(1, "Jaelon Darden");
var scottMiller = new Player(10, "Scott Miller");

// RWR
var tylerJohnson = new Player(18, "Tyler Johnson");
var breshadPerriman = new Player(16, "Breshad Perriman");

// Offensive tackles: LT and RT
var donovanSmith = new Player(76, "Donovan Smith");
var joshWells = new Player(72, "Josh Wells");
var tristanWirfs = new Player(78, "Tristan Wirfs");

depthChart.AddPlayerToDepthChart("QB", tomBrady);
depthChart.AddPlayerToDepthChart("QB", blaineGabbert);
depthChart.AddPlayerToDepthChart("QB", kyleTrask);

depthChart.AddPlayerToDepthChart("LWR", mikeEvans);
depthChart.AddPlayerToDepthChart("LWR", jaelonDarden);
depthChart.AddPlayerToDepthChart("LWR", scottMiller);

depthChart.AddPlayerToDepthChart("RWR", tylerJohnson);
depthChart.AddPlayerToDepthChart("RWR", breshadPerriman);

depthChart.AddPlayerToDepthChart("LT", donovanSmith);
depthChart.AddPlayerToDepthChart("LT", joshWells);

depthChart.AddPlayerToDepthChart("RT", tristanWirfs);
depthChart.AddPlayerToDepthChart("RT", joshWells);

Console.WriteLine("=== Initial depth chart ===");
PrintDepthChart(depthChart.GetFullDepthChart());

// Query backups.
PrintBackups(depthChart, "LWR", mikeEvans);
PrintBackups(depthChart, "RWR", breshadPerriman);
PrintBackups(depthChart, "RT", scottMiller);

// Remove players.
RemoveAndPrint(depthChart, "RWR", tylerJohnson);
RemoveAndPrint(depthChart, "LWR", tomBrady);

Console.WriteLine("\n=== Depth chart after removals ===");
PrintDepthChart(depthChart.GetFullDepthChart());

static void PrintBackups(
    TeamDepthChart depthChart,
    string position,
    Player player)
{
    var backups = depthChart.GetBackups(position, player);

    Console.WriteLine($"\nBackups for #{player.JerseyNumber} {player.Name} at {position}:");

    if (backups.Count == 0)
    {
        Console.WriteLine("  No backups found.");
        return;
    }

    foreach (var backup in backups)
    {
        Console.WriteLine($"  #{backup.JerseyNumber} - {backup.Name}");
    }
}

static void RemoveAndPrint(
    TeamDepthChart depthChart,
    string position,
    Player player)
{
    var removedPlayers = depthChart.RemovePlayerFromDepthChart(position, player);

    Console.WriteLine($"\nRemoving #{player.JerseyNumber} {player.Name} from {position}:");

    if (removedPlayers.Count == 0)
    {
        Console.WriteLine("  Player not found at this position.");
        return;
    }

    foreach (var removedPlayer in removedPlayers)
    {
        Console.WriteLine($"  Removed #{removedPlayer.JerseyNumber} - {removedPlayer.Name}");
    }
}

static void PrintDepthChart(
    IReadOnlyDictionary<string, IReadOnlyList<Player>> chart)
{
    foreach (var (position, players) in chart)
    {
        var formattedPlayers = players.Select(
            player => $"(#{player.JerseyNumber}, {player.Name})");

        Console.WriteLine($"{position} - {string.Join(", ", formattedPlayers)}");
    }
}