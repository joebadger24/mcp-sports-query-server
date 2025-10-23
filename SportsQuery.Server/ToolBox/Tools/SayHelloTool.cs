using SportsQuery.Server.ToolBox.Interfaces;

namespace SportsQuery.Server.ToolBox.Tools;

public class SayHelloTool : ITool
{
    public string Name => "hello";

    public string Description => "Returns a greeting message.";

    public async Task<string> Execute(string[] args)
    {
        await Task.Delay(1000);

        return "Hello! How can I assist you today?";
    }
}
