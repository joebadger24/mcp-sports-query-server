using System.Text.Json;

namespace SportsQuery.Server.Models;

public class ToolResult
{
    public bool Success { get; set; }
    public string Data { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static ToolResult CreateSuccess(object? data, string message = "")
    {
        string jsonData = data != null
            ? JsonSerializer.Serialize(data, JsonOptions)
            : string.Empty;

        return new ToolResult
        {
            Success = true,
            Data = jsonData,
            Message = message
        };
    }

    public static ToolResult CreateError(string message)
    {
        return new ToolResult
        {
            Success = false,
            Data = string.Empty,
            Message = message
        };
    }
}