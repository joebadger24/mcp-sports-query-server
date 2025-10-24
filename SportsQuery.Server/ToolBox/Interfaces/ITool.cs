using SportsQuery.Server.Models;

namespace SportsQuery.Server.ToolBox.Interfaces;

public interface ITool
{
    string Name { get; }

    string Description { get; }

    Task<ToolResult> ExecuteAsync(string[] args);
}