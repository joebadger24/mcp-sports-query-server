using System.Text.Json;
using SportsQuery.Server.Models;

namespace SportsQuery.Server.ToolBox.Services;

public class BBCSportJsonConvertor
{
    public BBCSportJsonConvertor()
    {
    }

    public static DailySummary DefaultSportConvert(JsonDocument document, DateTime date, string sport)
    {
        if (!document.RootElement.TryGetProperty("eventGroups", out JsonElement eventGroups))
        {
            throw new InvalidOperationException("Invalid JSON structure: 'eventGroups' not found.");
        }

        var summary = new DailySummary { Date = date, Sport = sport };

        foreach (var group in eventGroups.EnumerateArray())
        {
            string tournamentName = group.GetProperty("displayLabel").GetString() ?? "Unknown Tournament";

            if (!group.TryGetProperty("secondaryGroups", out JsonElement secondaryGroups))
                continue;

            var tournamentGroup = new TournamentGroup { Name = tournamentName };

            foreach (var secondary in secondaryGroups.EnumerateArray())
            {
                if (!secondary.TryGetProperty("events", out JsonElement events))
                    continue;

                if (events.GetArrayLength() == 0)
                    continue;

                foreach (var match in events.EnumerateArray())
                {
                    var sportingEvent = new SportingEvent
                    {
                        HomeTeam = match.GetProperty("home").GetProperty("fullName").GetString() ?? "Home",
                        AwayTeam = match.GetProperty("away").GetProperty("fullName").GetString() ?? "Away",
                        DisplayTime = match.GetProperty("time").GetProperty("displayTimeUK").GetString() ?? "Unknown time",
                        Tournament = tournamentName,
                        Sport = sport
                    };

                    tournamentGroup.Events.Add(sportingEvent);
                }
            }

            if (tournamentGroup.Events.Count > 0)
            {
                summary.Tournaments.Add(tournamentGroup);
            }
        }

        return summary;
    }

    public static DailySummary CricketConvert(JsonDocument document, DateTime date, string sport)
    {
        if (!document.RootElement.TryGetProperty("eventGroups", out JsonElement eventGroups))
        {
            throw new InvalidOperationException("Invalid JSON structure: 'eventGroups' not found.");
        }

        var summary = new DailySummary { Date = date, Sport = sport };

        foreach (var group in eventGroups.EnumerateArray())
        {
            if (!group.TryGetProperty("secondaryGroups", out JsonElement secondaryGroups))
                continue;

            foreach (var secondary in secondaryGroups.EnumerateArray())
            {
                var tournamentGroup = new TournamentGroup();

                if (!secondary.TryGetProperty("events", out JsonElement events))
                    continue;

                if (events.GetArrayLength() == 0)
                    continue;

                foreach (var match in events.EnumerateArray())
                {
                    var sportingEvent = new SportingEvent
                    {
                        HomeTeam = match.GetProperty("participants").GetProperty("homeTeam").GetProperty("name").GetString() ?? "Home",
                        AwayTeam = match.GetProperty("participants").GetProperty("awayTeam").GetProperty("name").GetString() ?? "Away",
                        DisplayTime = match.GetProperty("startTime").GetString() ?? "Unknown time",
                        Tournament = match.GetProperty("tournament").GetProperty("name").GetString() ?? "Unknown Tournament",
                        Sport = sport
                    };

                    tournamentGroup.Events.Add(sportingEvent);
                }

                if (tournamentGroup.Events.Count > 0)
                {
                    tournamentGroup.Name = tournamentGroup.Events.First().Tournament;
                    summary.Tournaments.Add(tournamentGroup);
                }
            }  
        }

        return summary;
    }
}