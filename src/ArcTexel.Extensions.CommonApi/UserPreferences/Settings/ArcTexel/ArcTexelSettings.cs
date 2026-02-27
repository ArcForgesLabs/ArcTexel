namespace ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;

public static class ArcTexelSettings
{
    private const string ArcTexel = "ArcTexel";

    public static class Palettes
    {
        public static LocalSetting<IEnumerable<string>> FavouritePalettes { get; } =
            LocalSetting.NonOwned<IEnumerable<string>>(ArcTexel);
    }

    public static class Update
    {
        public static SyncedSetting<bool> CheckUpdatesOnStartup { get; } = SyncedSetting.NonOwned(ArcTexel, true);

        public static SyncedSetting<string> UpdateChannel { get; } = SyncedSetting.NonOwned<string>(ArcTexel);
    }

    public static class Debug
    {
        public static SyncedSetting<bool> IsDebugModeEnabled { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static LocalSetting<string> PoEditorApiKey { get; } = new($"{ArcTexel}:POEditor_API_Key");
    }

    public static class Tools
    {
        public static SyncedSetting<bool> EnableSharedToolbar { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static SyncedSetting<bool> SelectionTintingEnabled { get; } = SyncedSetting.NonOwned(ArcTexel, true);

        public static SyncedSetting<RightClickMode> RightClickMode { get; } =
            SyncedSetting.NonOwned<RightClickMode>(ArcTexel);

        public static SyncedSetting<bool> IsPenModeEnabled { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static SyncedSetting<string> PrimaryToolset { get; } =
            SyncedSetting.NonOwned<string>(ArcTexel, "PAINT_TOOLSET");
    }

    public static class File
    {
        public static SyncedSetting<int> DefaultNewFileWidth { get; } = SyncedSetting.NonOwned(ArcTexel, 64);

        public static SyncedSetting<int> DefaultNewFileHeight { get; } = SyncedSetting.NonOwned(ArcTexel, 64);

        public static LocalSetting<IEnumerable<string>> RecentlyOpened { get; } =
            LocalSetting.NonOwned<IEnumerable<string>>(ArcTexel, []);

        public static SyncedSetting<int> MaxOpenedRecently { get; } = SyncedSetting.NonOwned(ArcTexel, 8);
    }

    public static class StartupWindow
    {
        public static SyncedSetting<bool> ShowStartupWindow { get; } = SyncedSetting.NonOwned(ArcTexel, true);

        public static SyncedSetting<bool> DisableNewsPanel { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static SyncedSetting<bool> NewsPanelCollapsed { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static SyncedSetting<IEnumerable<int>> LastCheckedNewsIds { get; } =
            SyncedSetting.NonOwned<IEnumerable<int>>(ArcTexel);
    }

    public static class Discord
    {
        public static SyncedSetting<bool> EnableRichPresence { get; } = SyncedSetting.NonOwned(ArcTexel, true);

        public static SyncedSetting<bool> ShowDocumentName { get; } = SyncedSetting.NonOwned<bool>(ArcTexel);

        public static SyncedSetting<bool> ShowDocumentSize { get; } = SyncedSetting.NonOwned(ArcTexel, true);

        public static SyncedSetting<bool> ShowLayerCount { get; } = SyncedSetting.NonOwned(ArcTexel, true);
    }

    public static class Analytics
    {
        public static SyncedSetting<bool> AnalyticsEnabled { get; } = SyncedSetting.NonOwned(ArcTexel, true);
    }

    public static class Scene
    {
        public static SyncedSetting<bool> AutoScaleBackground { get; } = SyncedSetting.NonOwned(ArcTexel,
            PreferencesConstants.AutoScaleBackgroundDefault, PreferencesConstants.AutoScaleBackground);

        public static SyncedSetting<double> CustomBackgroundScaleX { get; } = SyncedSetting.NonOwned(ArcTexel,
            PreferencesConstants.CustomBackgroundScaleDefault, PreferencesConstants.CustomBackgroundScaleX);

        public static SyncedSetting<double> CustomBackgroundScaleY { get; } = SyncedSetting.NonOwned(ArcTexel,
            PreferencesConstants.CustomBackgroundScaleDefault, PreferencesConstants.CustomBackgroundScaleY);

        public static SyncedSetting<string> PrimaryBackgroundColor { get; } = SyncedSetting.NonOwned(ArcTexel,
            PreferencesConstants.PrimaryBackgroundColorDefault, PreferencesConstants.PrimaryBackgroundColor);

        public static SyncedSetting<string> SecondaryBackgroundColor { get; } = SyncedSetting.NonOwned(ArcTexel,
            PreferencesConstants.SecondaryBackgroundColorDefault, PreferencesConstants.SecondaryBackgroundColor);
    }

    public static class Performance
    {
        public static SyncedSetting<int> MaxBilinearSampleSize { get; } = SyncedSetting.NonOwned(
            ArcTexel,
            PreferencesConstants.MaxBilinearSampleSizeDefault,
            PreferencesConstants.MaxBilinearSampleSize);

        public static SyncedSetting<bool> DisablePreviews { get; } = SyncedSetting.NonOwned(
            ArcTexel,
            PreferencesConstants.DisablePreviewsDefault,
            PreferencesConstants.DisablePreviews);
    }

    public static class Appearance
    {
        public static SyncedSetting<bool> UseSystemDecorations { get; } = SyncedSetting.NonOwned(
            ArcTexel,
            PreferencesConstants.UseSystemWindowDecorationsDefault,
            PreferencesConstants.UseSystemWindowDecorations);
    }
}
