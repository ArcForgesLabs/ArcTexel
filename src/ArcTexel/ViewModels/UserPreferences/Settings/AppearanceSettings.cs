using ArcTexel.Extensions.CommonApi.UserPreferences;

namespace ArcTexel.ViewModels.UserPreferences.Settings;

internal class AppearanceSettings : SettingsGroup
{
    private bool useSystemDecorations = GetPreference(PreferencesConstants.UseSystemWindowDecorations, PreferencesConstants.UseSystemWindowDecorationsDefault);

    public bool UseSystemDecorations
    {
        get => useSystemDecorations;
        set => RaiseAndUpdatePreference(ref useSystemDecorations, value, PreferencesConstants.UseSystemWindowDecorations);
    }
}
