using SportsQuery.Server.ToolBox.Interfaces;
using SportsQuery.Server.ToolBox.Tools;

namespace SportsQuery.Server.ToolBox;

public class ToolBox
{
    public List<ITool> Tools =
    [
        new SayHelloTool(),
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

/*
 * Tech Points:
 * 
 * Have a Class for a sporting event
 * 
 * Have a Class for a sporting team or player
 * 
 * Have a Class for a tournament or league
 * 
 * Have a Class for a Channel or Streaming Service
 * 
 */
