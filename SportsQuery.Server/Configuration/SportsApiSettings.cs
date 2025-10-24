namespace SportsQuery.Server.Configuration;

public class SportsApiSettings
{
    public string BaseUrl { get; set; } = "https://www.bbc.co.uk/wc-data/container/sport-data-scores-fixtures";
    public string[] SupportedSports { get; set; } = 
    [
        "football",
        "cricket",
        "rugby-union",
        "rugby-league"
    ];
}