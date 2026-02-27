using ArcTexel.Extensions.CommonApi.Logging;

namespace ArcTexel.Models.ExtensionServices;

internal class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void LogError(string message)
    {
        Console.WriteLine($"Error: {message}");
    }

    public void LogWarning(string message)
    {
        Console.WriteLine($"Warning: {message}");
    }
}
