using SportsQuery.Server.Models;
using SportsQuery.Server.ToolBox;
using SportsQuery.Server.ToolBox.Interfaces;

internal class Program
{
    private static async Task Main(string[] args)
    {
        ToolBox Toolbox = new();
        var Tools = Toolbox.Tools.ToDictionary(tool => tool.Name, tool => tool, StringComparer.OrdinalIgnoreCase);

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
                foreach (var t in Toolbox.Tools)
                {
                    Console.WriteLine($"- {t.Name}: {t.Description}");
                }
                continue;
            }

            if (Tools.TryGetValue(command, out ITool? tool))
            {
                try
                {
                    ToolResult result = await tool.ExecuteAsync(arguments);
                    Console.WriteLine(result.Message);

                    //if (!string.IsNullOrEmpty(result.Data))
                    //{
                    //    Console.WriteLine("\nData:");
                    //    Console.WriteLine(result.Data);
                    //}
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