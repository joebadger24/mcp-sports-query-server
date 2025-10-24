using SportsQuery.Server.ToolBox.Interfaces;
using SportsQuery.Server.ToolBox.Tools;

namespace SportsQuery.Server.ToolBox;

public class ToolBox
{
    public List<ITool> Tools =
    [
        new GetDailySummaryTool()
    ];
}

/*
 * Main tools:
 * 
 * GetDailySummaryTool - Fetches and summarizes sports events for a specified date and sport.
 * 
 * How to Watch Tool - Provides information on how to watch a particular sports event.
 * 
 * Upcoming Fixtures Tool - Retrieves upcoming sports fixtures for a given team or player or league
 * 
 */