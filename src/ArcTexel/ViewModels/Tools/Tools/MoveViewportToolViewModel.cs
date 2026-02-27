using Avalonia;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Drawie.Backend.Core.Vector;
using ArcTexel.Helpers;
using ArcTexel.Helpers.Extensions;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.H, Transient = Key.Space, TransientImmediate = true)]
internal class MoveViewportToolViewModel : ToolViewModel
{
    public override string ToolNameLocalizationKey => "MOVE_VIEWPORT_TOOL";
    public override Type[]? SupportedLayerTypes { get; } = null;
    public override Type LayerTypeToCreateOnEmptyUse { get; } = null;
    public override bool HideHighlight => true;
    public override LocalizedString Tooltip => new LocalizedString("MOVE_VIEWPORT_TOOLTIP", Shortcut);

    public override string DefaultIcon => ArcPerfectIcons.Hand;

    public override bool StopsLinkedToolOnUse => false;

    public MoveViewportToolViewModel()
    {
        Cursor = new Cursor(StandardCursorType.SizeAll);
    }

    protected override void OnSelected(bool restoring)
    {
        if (ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument == null)
            return;

        ActionDisplay = new LocalizedString("MOVE_VIEWPORT_ACTION_DISPLAY");
        ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument.SuppressAllOverlayEvents(ToolName);
    }

    protected override void OnDeselecting(bool transient)
    {
        if (ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument == null)
            return;

        base.OnDeselecting(transient);
        ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument.RestoreAllOverlayEvents(ToolName);
    }
}
