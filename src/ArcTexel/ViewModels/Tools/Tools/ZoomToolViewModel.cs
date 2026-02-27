using Avalonia.Input;
using Drawie.Backend.Core.Vector;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.Z)]
internal class ZoomToolViewModel : ToolViewModel
{
    private bool zoomOutOnClick = false;
    public bool ZoomOutOnClick
    {
        get => zoomOutOnClick;
        set => SetProperty(ref zoomOutOnClick, value);
    }

    private string defaultActionDisplay = new LocalizedString("ZOOM_TOOL_ACTION_DISPLAY_DEFAULT");

    public override string ToolNameLocalizationKey => "ZOOM_TOOL";
    public override Type[]? SupportedLayerTypes { get; } = null;

    public override bool StopsLinkedToolOnUse => false;

    public override string DefaultIcon => ArcPerfectIcons.ZoomIn;

    public ZoomToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
    }

    public override Type LayerTypeToCreateOnEmptyUse { get; } = null;
    public override bool HideHighlight => true;

    public override LocalizedString Tooltip => new LocalizedString("ZOOM_TOOL_TOOLTIP", Shortcut);

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        if (ctrlIsDown)
        {
            ActionDisplay = new LocalizedString("ZOOM_TOOL_ACTION_DISPLAY_CTRL");
            ZoomOutOnClick = true;
        }
        else
        {
            ActionDisplay = defaultActionDisplay;
            ZoomOutOnClick = false;
        }
    }
}
