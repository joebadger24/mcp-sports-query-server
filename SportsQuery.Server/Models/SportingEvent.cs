namespace SportsQuery.Server.Models;

public class SportingEvent
{
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public string DisplayTime { get; set; } = string.Empty;
    public string Tournament { get; set; } = string.Empty;
    public string Sport { get; set; } = string.Empty;
}