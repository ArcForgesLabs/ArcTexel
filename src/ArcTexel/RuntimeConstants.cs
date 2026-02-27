using System.Diagnostics;
using System.Reflection;
using ArcTexel.Models.IO;

namespace ArcTexel;

public static class RuntimeConstants
{
    private static Dictionary<string, string> appSettings =
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(ReadAppSettings());

    private static string ReadAppSettings()
    {
        string installDirPath = Paths.InstallDirectoryPath;
        string appsettingsPath = Path.Combine(installDirPath, "appsettings.json");
        if (!File.Exists(appsettingsPath))
        {
            return "{}";
        }
        
        using StreamReader reader = new StreamReader(appsettingsPath);
        return reader.ReadToEnd();
    }


    public static string? AnalyticsUrl =>
        appSettings.TryGetValue("AnalyticsUrl", out string? url) ? url : null;

    public static string? ArcTexelApiUrl =>
        appSettings.TryGetValue("ArcTexelApiUrl", out string? apiUrl) ? apiUrl : null;

    public static string? ArcTexelApiKey =>
        appSettings.TryGetValue("ArcTexelApiKey", out string? apiKey) ? apiKey : null;
}
