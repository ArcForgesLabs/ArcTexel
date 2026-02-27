using Avalonia.Input;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.Views.Overlays.BrushShapeOverlay;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.L, CommonToolType = "Line")]
internal class RasterLineToolViewModel : ShapeTool, ILineToolHandler
{
    private string defaultActionDisplay = "LINE_TOOL_ACTION_DISPLAY_DEFAULT";

    public override string ToolNameLocalizationKey => "LINE_TOOL";
    public override LocalizedString Tooltip => new LocalizedString("LINE_TOOL_TOOLTIP", Shortcut);

    public override Type[]? SupportedLayerTypes { get; } = { typeof(IRasterLayerHandler) };
    public override string DefaultIcon => ArcPerfectIcons.LowresLine;

    [Settings.Inherited] public double ToolSize => GetValue<double>();

    public bool Snap { get; private set; }

    public override Type LayerTypeToCreateOnEmptyUse { get; } = typeof(ImageLayerNode);

    public RasterLineToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
        Toolbar = ToolbarFactory.Create<RasterLineToolViewModel, ShapeToolbar>(this);
        var strokeSetting = Toolbar.GetSetting(nameof(ShapeToolbar.ToolSize));
        if (strokeSetting != null)
        {
            strokeSetting.Value = 1d;
        }
    }

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        DrawFromCenter = ctrlIsDown;

        if (shiftIsDown)
        {
            ActionDisplay = "LINE_TOOL_ACTION_DISPLAY_SHIFT";
            Snap = true;
        }
        else
        {
            ActionDisplay = defaultActionDisplay;
            Snap = false;
        }
    }

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseRasterLineTool();
    }

    protected override void OnSelected(bool restoring)
    {
        if (restoring) return;

        var document = ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument;
        document.Tools.UseRasterLineTool();
    }
}
