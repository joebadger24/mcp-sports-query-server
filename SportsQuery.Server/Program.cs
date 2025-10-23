using SportsQuery.Server.ToolBox;
using SportsQuery.Server.ToolBox.Interfaces;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ToolBox toolbox = new();
        var tools = toolbox.Tools.ToDictionary(tool => tool.Name, tool => tool, StringComparer.OrdinalIgnoreCase);

        Console.WriteLine("\nSports Query Server is Running...");

        while (true)
        {
            Console.Write("\n> ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];
            string[] arguments = [.. parts.Skip(1)];

            if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            if (command.Equals("tools", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nAvailable tools:");
                foreach (var t in toolbox.Tools)
                {
                    Console.WriteLine($"- {t.Name}: {t.Description}");
                }
                continue;
            }

            if (tools.TryGetValue(command, out ITool? tool))
            {
                try
                {
                    string result = await tool.Execute(arguments);
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while executing tool '{tool.Name}': {ex.Message}");
                }

            }
            else
            {
                Console.WriteLine("Unknown command.");
            }
        }
    }
}