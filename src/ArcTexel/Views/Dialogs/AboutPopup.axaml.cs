using ArcTexel.Helpers;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Views.Dialogs;

public partial class AboutPopup : ArcTexelPopup
{
    public static LocalizedString VersionText =>
        new LocalizedString("VERSION", VersionHelpers.GetCurrentAssemblyVersionString(true));
    
    public static LocalizedString BuildIdText =>
        new LocalizedString("BUILD_ID", VersionHelpers.GetBuildId());

    public bool DisplayDonationButton
    {
#if STEAM
        get => false;
#else
        get => true;
#endif
    }
    public AboutPopup()
    {
        InitializeComponent();
    }
}

