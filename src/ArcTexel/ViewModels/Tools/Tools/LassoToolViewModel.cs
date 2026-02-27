using Avalonia.Input;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Vector;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers.Tools;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.Q)]
internal class LassoToolViewModel : ToolViewModel, ILassoToolHandler
{
    private string defaultActionDisplay = "LASSO_TOOL_ACTION_DISPLAY_DEFAULT";

    public LassoToolViewModel()
    {
        Toolbar = ToolbarFactory.Create(this);
        ActionDisplay = defaultActionDisplay;
    }

    private SelectionMode KeyModifierselectionMode = SelectionMode.New;
    public SelectionMode? ResultingSelectionMode => KeyModifierselectionMode != SelectionMode.New ? KeyModifierselectionMode : SelectMode;

    public override Type LayerTypeToCreateOnEmptyUse { get; } = null;

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        if (shiftIsDown)
        {
            ActionDisplay = "LASSO_TOOL_ACTION_DISPLAY_SHIFT";
            KeyModifierselectionMode = SelectionMode.Add;
        }
        else if (ctrlIsDown)
        {
            ActionDisplay = "LASSO_TOOL_ACTION_DISPLAY_CTRL";
            KeyModifierselectionMode = SelectionMode.Subtract;
        }
        else
        {
            ActionDisplay = defaultActionDisplay;
            KeyModifierselectionMode = SelectionMode.New;
        }
    }

    public override LocalizedString Tooltip => new LocalizedString("LASSO_TOOL_TOOLTIP", Shortcut);

    public override string ToolNameLocalizationKey => "LASSO_TOOL";
    public override string DefaultIcon => ArcPerfectIcons.Lasso;

    //TODO: BrushShape.Pixel;

    public override Type[]? SupportedLayerTypes { get; } = null; // all layer types are supported

    [Settings.Enum("MODE_LABEL")]
    public SelectionMode SelectMode => GetValue<SelectionMode>();
    
    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseLassoTool();
    }
}
