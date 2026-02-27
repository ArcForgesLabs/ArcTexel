using System.Diagnostics;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using ArcTexel.Helpers;
using ArcTexel.Initialization;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Dialogs;
using ArcTexel.OperatingSystem;
using ArcTexel.UI.Common.Fonts;

namespace ArcTexel.ViewModels.SubViewModels;

[Command.Group("ArcTexel.Links", "MISC")]
internal class MiscViewModel : SubViewModel<ViewModelMain>
{
    public MiscViewModel(ViewModelMain owner)
        : base(owner)
    {
    }

    [Command.Internal("ArcTexel.Links.OpenHyperlink")]
    [Command.Basic("ArcTexel.Links.OpenDocumentation", "https://arctexel.net/docs/", "DOCUMENTATION", "OPEN_DOCUMENTATION", Icon = ArcPerfectIcons.Globe,
        MenuItemPath = "HELP/DOCUMENTATION", MenuItemOrder = 0, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Links.OpenWebsite", "https://arctexel.net", "WEBSITE", "OPEN_WEBSITE",
        Icon = ArcPerfectIcons.Globe,
        MenuItemPath = "HELP/WEBSITE", MenuItemOrder = 1, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Links.OpenRepository", "https://github.com/ArcTexel/ArcTexel", "REPOSITORY",
        "OPEN_REPOSITORY", Icon = ArcPerfectIcons.Globe,
        MenuItemPath = "HELP/REPOSITORY", MenuItemOrder = 2, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Links.OpenLicense", "{BaseDir}/LICENSE", "LICENSE", "OPEN_LICENSE", Icon = ArcPerfectIcons.Folder,
        MenuItemPath = "HELP/LICENSE", MenuItemOrder = 3, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Links.OpenOtherLicenses", "{BaseDir}/Third Party Licenses", "THIRD_PARTY_LICENSES",
        "OPEN_THIRD_PARTY_LICENSES", Icon = ArcPerfectIcons.Folder,
        MenuItemPath = "HELP/THIRD_PARTY_LICENSES", MenuItemOrder = 4, AnalyticsTrack = true)]
    public static void OpenUri(string uri)
    {
        try
        {
            if (uri.StartsWith("{BaseDir}"))
            {
                string exeDir = Path.GetDirectoryName(Environment.ProcessPath);
                uri = uri.Replace("{BaseDir}", exeDir ?? string.Empty);
            }

            IOperatingSystem.Current.OpenUri(uri);
        }
        catch (Exception e)
        {
            CrashHelper.SendExceptionInfo(e);
            NoticeDialog.Show(title: "Error", message: $"Couldn't open the address {uri} in your default browser");
        }
    }

    [Command.Internal("ArcTexel.Restart")]
    public static void Restart()
    {
        ClassicDesktopEntry.Active?.Restart();
    }
}
