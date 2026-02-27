using ArcTexel.Extensions.CommonApi.UserPreferences;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Tools;
using ArcTexel.ViewModels.Tools.Tools;

namespace ArcTexel.ViewModels.SubViewModels;

[Command.Group("ArcTexel.Stylus", "STYLUS")]
internal class StylusViewModel : SubViewModel<ViewModelMain>
{
    private bool isPenModeEnabled;
    private bool useTouchGestures;

    public bool ToolSetByStylus { get; set; }

    private ToolViewModel PreviousTool { get; set; }

    public StylusViewModel(ViewModelMain owner)
        : base(owner)
    {
        isPenModeEnabled = ArcTexelSettings.Tools.IsPenModeEnabled.Value;
    }

    //TODO: Fix stylus support
    /*[Command.Internal("ArcTexel.Stylus.StylusOutOfRange")]
    public void StylusOutOfRange(StylusEventArgs e)
    {
        //Owner.BitmapManager.UpdateHighlightIfNecessary(true);
    }*/

    /*[Command.Internal("ArcTexel.Stylus.StylusSystemGesture")]
    public void StylusSystemGesture(StylusSystemGestureEventArgs e)
    {
        if (e.SystemGesture is SystemGesture.Drag or SystemGesture.Tap)
        {
            return;
        }

        e.Handled = true;
    }*/

    /*[Command.Internal("ArcTexel.Stylus.StylusDown")]
    public void StylusDown(StylusButtonEventArgs e)
    {
        e.Handled = true;

        if (e.StylusButton.Guid == StylusPointProperties.TipButton.Id && e.Inverted)
        {
            PreviousTool = Owner.ToolsSubViewModel.ActiveTool;
            Owner.ToolsSubViewModel.SetActiveTool<EraserToolViewModel>(true);
            ToolSetByStylus = true;
        }
    }*/

    /*[Command.Internal("ArcTexel.Stylus.StylusUp")]
    public void StylusUp(StylusButtonEventArgs e)
    {
        e.Handled = true;

        if (ToolSetByStylus && e.StylusButton.Guid == StylusPointProperties.TipButton.Id && e.Inverted)
        {
            Owner.ToolsSubViewModel.SetActiveTool(PreviousTool, false);
        }
    }*/
}
