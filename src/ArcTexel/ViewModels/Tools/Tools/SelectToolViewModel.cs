using Avalonia.Input;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Vector;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.Models.Position;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.M)]
internal class SelectToolViewModel : ToolViewModel, ISelectToolHandler
{
    private string defaultActionDisplay = "SELECT_TOOL_ACTION_DISPLAY_DEFAULT";
    public override string ToolNameLocalizationKey => "SELECT_TOOL_NAME";

    public override string DefaultIcon => ArcPerfectIcons.RectangleSelection;

    public SelectToolViewModel()
    {
        ActionDisplay = defaultActionDisplay;
        Toolbar = ToolbarFactory.Create(this);
        Cursor = new Cursor(StandardCursorType.Cross);
    }

    private SelectionMode KeyModifierselectionMode = SelectionMode.New;
    public SelectionMode ResultingSelectionMode => KeyModifierselectionMode != SelectionMode.New ? KeyModifierselectionMode : SelectMode;

    public override Type LayerTypeToCreateOnEmptyUse { get; } = null;

    public override void KeyChanged(bool ctrlIsDown, bool shiftIsDown, bool altIsDown, Key argsKey)
    {
        if (shiftIsDown)
        {
            ActionDisplay = new LocalizedString("SELECT_TOOL_ACTION_DISPLAY_SHIFT");
            KeyModifierselectionMode = SelectionMode.Add;
        }
        else if (ctrlIsDown)
        {
            ActionDisplay = new LocalizedString("SELECT_TOOL_ACTION_DISPLAY_CTRL");
            KeyModifierselectionMode = SelectionMode.Subtract;
        }
        else
        {
            ActionDisplay = defaultActionDisplay;
            KeyModifierselectionMode = SelectionMode.New;
        }
    }

    [Settings.Enum("MODE_LABEL")]
    public SelectionMode SelectMode => GetValue<SelectionMode>();

    [Settings.Enum("SHAPE_LABEL")]
    public SelectionShape SelectShape => GetValue<SelectionShape>();

    //TODO: was Pixel
    public override Type[]? SupportedLayerTypes { get; } = null;

    public override LocalizedString Tooltip => new LocalizedString("SELECT_TOOL_TOOLTIP", Shortcut);

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseSelectTool();
    }
}
