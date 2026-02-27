using ArcTexel.Extensions.Sdk;

namespace Preferences;

public class PreferencesSampleExtension : ArcTexelExtension
{
    /// <summary>
    ///     This method is called when extension is loaded.
    ///  All extensions are first loaded and then initialized. This method is called before <see cref="OnInitialized"/>.
    /// </summary>
    public override void OnLoaded()
    {
    }

    /// <summary>
    ///     This method is called when extension is initialized. After this method is called, you can use Api property to access ArcTexel API.
    /// </summary>
    public override void OnInitialized()
    {
        Api.Preferences.AddCallback<int>("HelloCount", (name, value) => Api.Logger.Log($"Hello count changed to {value}!"));
        Api.Preferences.AddCallback<double>("TestDouble", (name, value) => Api.Logger.Log($"Test double changed to {value}!"));
        Api.Preferences.AddCallback<string>("TestString", (name, value) => Api.Logger.Log($"Test string changed to {value}!"));
        Api.Preferences.AddCallback<bool>("TestBool", (name, value) => Api.Logger.Log($"Test bool changed to {value}!"));
        
        // Internally this preference will have name "yourCompany.Samples.Preferences:HelloCount".
        int helloCount = Api.Preferences.GetPreference<int>("HelloCount", 0);

        Api.Preferences.UpdatePreference("HelloCount", helloCount + 1);
        
        Api.Preferences.UpdatePreference("TestDouble", 3.14);
        Api.Preferences.UpdatePreference("TestString", "Hello, World!");
        Api.Preferences.UpdatePreference("TestBool", true);

        // This will overwrite built-in ArcTexel preference. Extension must have WriteNonOwnedPreferences permission.
        // Prepending "ArcTexel:" to preference name will access built-in ArcTexel preferences. If you set it to other extension unique name,
        // it will access extension preferences.
        // You can do analogous thing with UpdatePreference.
        Api.Preferences.UpdateLocalPreference(
            "ArcTexel:OverwrittenArcTexelPreference",
            "This is overwritten value of preference that is built-in in ArcTexel.");

        // You don't need any special permission for reading any kind of preference.
        Api.Logger.Log(Api.Preferences.GetLocalPreference<string>("ArcTexel:OverwrittenArcTexelPreference", string.Empty));
    }
}
