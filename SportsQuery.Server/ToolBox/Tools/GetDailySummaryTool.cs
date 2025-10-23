using System.Text;
using System.Text.Json;
using SportsQuery.Server.ToolBox.Interfaces;

namespace SportsQuery.Server.ToolBox.Tools;

public class GetDailySummaryTool : ITool
{
    private static readonly HttpClient _httpClient = new();

    private static readonly HashSet<string> SupportedSports = new()
    {
        "football",
        "cricket",
        "rugby-union",
        "rugby-league"
    };

    public string Name => "get-daily-summary";
    public string Description => 
        """
        
        Returns a summary of sports events for a given date and sport.
        Usage: get-daily-summary [yyyy-MM-dd] [sport]
        Example: get-daily-summary 2025-10-24 rugby-union
        
        """;

    public async Task<string> Execute(string[] args)
    {
        var date = DateTime.UtcNow;
        var sport = "football";

        if (args.Length >= 1 && DateTime.TryParse(args[0], out DateTime parsedDate))
        {
            date = parsedDate;
        }

        if (args.Length >= 2)
        {
            string inputSport = args[1].ToLowerInvariant();

            if (SupportedSports.Contains(inputSport))
            {
                sport = inputSport;
            }
            else
            {
                return $"Unsupported sport: '{inputSport}'\n Supported sports: {string.Join(", ", SupportedSports)}";
            }
        }

        string url = $"https://www.bbc.co.uk/wc-data/container/sport-data-scores-fixtures?" +
            $"selectedStartDate={date:yyyy-MM-dd}&selectedEndDate={date:yyyy-MM-dd}&todayDate={date:yyyy-MM-dd}" +
            $"&urn=urn%3Abbc%3Asportsdata%3A{sport}";

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            return $"Error fetching data: {ex.Message}";
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
        if (!doc.RootElement.TryGetProperty("eventGroups", out JsonElement eventGroups))
        {
            return "No event groups found in the response.";
        }

        StringBuilder sb = new();
        sb.AppendLine($"\nDaily Summary for {date:yyyy-MM-dd} ({sport})\n");

        foreach (var group in eventGroups.EnumerateArray())
        {
            string tournamentName = group.GetProperty("displayLabel").GetString() ?? "Unknown Tournament";

            if (!group.TryGetProperty("secondaryGroups", out JsonElement secondaryGroups))
                continue;

            foreach (var secondary in secondaryGroups.EnumerateArray())
            {
                if (!secondary.TryGetProperty("events", out JsonElement events))
                    continue;

                if (events.GetArrayLength() == 0)
                    continue;

                sb.AppendLine($"{tournamentName}");

                foreach (var match in events.EnumerateArray())
                {
                    string home = match.GetProperty("home").GetProperty("fullName").GetString() ?? "Home";
                    string away = match.GetProperty("away").GetProperty("fullName").GetString() ?? "Away";
                    string time = match.GetProperty("time").GetProperty("displayTimeUK").GetString() ?? "Unknown time";

                    sb.AppendLine($"- {home} vs {away} at {time}");
                }

                sb.AppendLine();
            }
        }

        return sb.ToString().Trim();
    }
}
