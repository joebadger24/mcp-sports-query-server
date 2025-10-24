using System.Text;
using System.Text.Json;
using SportsQuery.Server.Models;
using SportsQuery.Server.ToolBox.Interfaces;
using SportsQuery.Server.ToolBox.Services;

namespace SportsQuery.Server.ToolBox.Tools;

public class GetDailySummaryTool : ITool
{
    private static readonly HttpClient _httpClient = new();

    private static readonly HashSet<string> SupportedSports =
    [
        "football",
        "cricket",
        "rugby-union",
        "rugby-league"
    ];

    public string Name => "get-daily-summary";
    public string Description => 
        """
        
        Returns a summary of sports events for a given date and sport.
        Usage: get-daily-summary [yyyy-MM-dd] [sport]
        Example: get-daily-summary 2025-10-24 rugby-union
        
        """;

    public async Task<ToolResult> ExecuteAsync(string[] args)
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
                return ToolResult.CreateError($"Unsupported sport: '{inputSport}'\n Supported sports: {string.Join(", ", SupportedSports)}");
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
            return ToolResult.CreateError($"Error fetching data: {ex.Message}");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(jsonResponse);

        DailySummary summary = sport switch
        {
            "cricket" => BBCSportJsonConvertor.CricketConvert(doc, date, sport),
            _ => BBCSportJsonConvertor.DefaultSportConvert(doc, date, sport)
        };

        string formattedSummary = FormatSummary(summary);

        return ToolResult.CreateSuccess(summary, formattedSummary);
    }

    private static string FormatSummary(DailySummary summary)
    {
        StringBuilder sb = new();
        sb.AppendLine($"\nDaily Summary for {summary.Date:yyyy-MM-dd} ({summary.Sport})\n");

        foreach (var tournament in summary.Tournaments)
        {
            sb.AppendLine($"Tournament: {tournament.Name}");

            foreach (var ev in tournament.Events)
            {
                sb.AppendLine($" - {ev.DisplayTime}: {ev.HomeTeam} vs {ev.AwayTeam}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}