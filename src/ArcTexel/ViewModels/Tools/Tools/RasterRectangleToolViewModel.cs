using Avalonia.Input;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.R, CommonToolType = "Rectangle")]
internal class RasterRectangleToolViewModel : ShapeTool, IRasterRectangleToolHandler
{
    private string defaultActionDisplay = "RECTANGLE_TOOL_ACTION_DISPLAY_DEFAULT";

    public override string ToolNameLocalizationKey => "RECTANGLE_TOOL";
    public override Type[]? SupportedLayerTypes { get; } = { typeof(IRasterLayerHandler) };
    public override LocalizedString Tooltip => new LocalizedString("RECTANGLE_TOOL_TOOLTIP", Shortcut);

    public bool Filled { get; set; } = false;

    public override string DefaultIcon => ArcPerfectIcons.LowresSquare;

    public override Type LayerTypeToCreateOnEmptyUse { get; } = typeof(ImageLayerNode);


    [Settings.Percent("RADIUS", 0, ExposedByDefault = true, Min = 0)]
    public float CornerRadius
    {
        get
        {
            return GetValue<float>();
        }
        set
        {
            SetValue(value);
        }
    }


    public RasterRectangleToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
        Toolbar = ToolbarFactory.Create<RasterRectangleToolViewModel, FillableShapeToolbar>(this);
    }

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        DrawFromCenter = ctrlIsDown;

        if (shiftIsDown)
        {
            DrawEven = true;
            ActionDisplay = "RECTANGLE_TOOL_ACTION_DISPLAY_SHIFT";
        }
        else
        {
            DrawEven = false;
            ActionDisplay = defaultActionDisplay;
        }
    }

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseRasterRectangleTool();
    }

    protected override void OnSelected(bool restoring)
    {
        if (restoring) return;

        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseRasterRectangleTool();
    }
}
