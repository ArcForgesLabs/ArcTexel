using ArcTexel.Extensions.CommonApi.Windowing;
using ArcTexel.Extensions.Sdk.Api;
using ArcTexel.Extensions.Sdk.Api.Commands;
using ArcTexel.Extensions.Sdk.Api.IO;
using ArcTexel.Extensions.Sdk.Api.Logging;
using ArcTexel.Extensions.Sdk.Api.Palettes;
using ArcTexel.Extensions.Sdk.Api.Ui;
using ArcTexel.Extensions.Sdk.Api.UserData;
using ArcTexel.Extensions.Sdk.Api.UserPreferences;
using ArcTexel.Extensions.Sdk.Api.Window;

namespace ArcTexel.Extensions.Sdk;

public class ArcTexelApi
{
    public Logger Logger { get; }
    public WindowProvider WindowProvider { get; }
    public Preferences Preferences { get; }
    public PalettesProvider Palettes { get; }
    public CommandProvider Commands { get; }
    public DocumentProvider Documents { get; }
    public VisualTreeProvider VisualTreeProvider { get; }
    public UserDataProvider UserDataProvider { get; }

    public ArcTexelApi()
    {
        Logger = new Logger();
        WindowProvider = new WindowProvider();
        Preferences = new Preferences();
        Palettes = new PalettesProvider();
        Commands = new CommandProvider();
        Documents = new DocumentProvider();
        VisualTreeProvider = new VisualTreeProvider();
        UserDataProvider = new UserDataProvider();
    }
}
