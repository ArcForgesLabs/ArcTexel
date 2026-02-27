using System.ComponentModel;

namespace ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;

public enum RightClickMode
{
    [Description("USE_SECONDARY_COLOR")]
    SecondaryColor,
    [Description("SHOW_CONTEXT_MENU")]
    ContextMenu,
    [Description("ERASE")]
    Erase,
    [Description("COLOR_PICKER")]
    ColorPicker,
}
