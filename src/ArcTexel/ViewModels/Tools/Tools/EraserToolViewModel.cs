using Avalonia.Input;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Vector;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using Drawie.Numerics;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.E)]
internal class EraserToolViewModel : BrushBasedToolViewModel, IEraserToolHandler
{
    public EraserToolViewModel()
    {
        ActionDisplay = "ERASER_TOOL_ACTION_DISPLAY";
        //Toolbar = ToolbarFactory.Create<EraserToolViewModel, PenToolbar>(this);
    }

    public override bool IsErasable => true;

    public override string ToolNameLocalizationKey => "ERASER_TOOL";
    public override string DefaultIcon => ArcPerfectIcons.Eraser;

    public override LocalizedString Tooltip => new LocalizedString("ERASER_TOOL_TOOLTIP", Shortcut);

    protected override Toolbar CreateToolbar()
    {
        return ToolbarFactory.Create<EraserToolViewModel, BrushToolbar>(this);
    }

    protected override void SwitchToTool()
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseEraserTool();
    }
}
