using Avalonia.Input;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.C, CommonToolType = "Ellipse")]
internal class RasterEllipseToolViewModel : ShapeTool, IRasterEllipseToolHandler
{
    private string defaultActionDisplay = "ELLIPSE_TOOL_ACTION_DISPLAY_DEFAULT";
    public override string ToolNameLocalizationKey => "ELLIPSE_TOOL";

    public RasterEllipseToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
    }

    public override Type[]? SupportedLayerTypes { get; } = { typeof(IRasterLayerHandler) };
    public override LocalizedString Tooltip => new LocalizedString("ELLIPSE_TOOL_TOOLTIP", Shortcut);

    public override string DefaultIcon => ArcPerfectIcons.LowresCircle;

    public override Type LayerTypeToCreateOnEmptyUse { get; } = typeof(ImageLayerNode);

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        DrawFromCenter = ctrlIsDown;
        
        if (shiftIsDown)
        {
            ActionDisplay = "ELLIPSE_TOOL_ACTION_DISPLAY_SHIFT";
            DrawEven = true;
        }
        else
        {
            ActionDisplay = defaultActionDisplay;
            DrawEven = false;
        }
    }

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseRasterEllipseTool();
    }
    
    protected override void OnSelected(bool restoring)
    {
        if(restoring) return;
        
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseRasterEllipseTool();
    }
}
