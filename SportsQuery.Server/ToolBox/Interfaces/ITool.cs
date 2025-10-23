namespace SportsQuery.Server.ToolBox.Interfaces;

public interface ITool
{
    string Name { get; }

    string Description { get; }

    Task<string> Execute(string[] args);
}
