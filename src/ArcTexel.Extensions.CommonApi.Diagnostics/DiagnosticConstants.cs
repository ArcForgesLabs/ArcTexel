using System.Collections.Generic;

namespace ArcTexel.Extensions.CommonApi.Diagnostics;

internal static class DiagnosticConstants
{
    public const string Category = "ArcTexel.CommonAPI";
    
    public const string SettingNamespace = "ArcTexel.Extensions.CommonApi.UserPreferences.Settings";
    public static List<string> settingNames = ["SyncedSetting", "LocalSetting"];
}
