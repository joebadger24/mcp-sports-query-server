namespace SportsQuery.Server.Models;

public class DailySummary
{
    public DateTime Date { get; set; }
    public string Sport { get; set; } = string.Empty;
    public List<TournamentGroup> Tournaments { get; set; } = [];
}

public class TournamentGroup
{
    public string Name { get; set; } = string.Empty;
    public List<SportingEvent> Events { get; set; } = [];
}