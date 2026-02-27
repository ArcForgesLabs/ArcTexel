using Avalonia.Input;
using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;

namespace ArcTexel.ViewModels.Tools.Tools;

[Command.Tool(Key = Key.T)]
internal class TextToolViewModel : ToolViewModel, ITextToolHandler
{
    public const string NewLayerKey = "TEXT_LAYER_NAME";
    public override string ToolNameLocalizationKey => "TEXT_TOOL";
    public override Type[]? SupportedLayerTypes => [];
    public override Type LayerTypeToCreateOnEmptyUse => typeof(VectorLayerNode);
    public override LocalizedString Tooltip => new LocalizedString("TEXT_TOOL_TOOLTIP", Shortcut);

    public override string DefaultIcon => ArcPerfectIcons.TextRound;

    public override bool IsErasable => false;
    public override bool StopsLinkedToolOnUse => false;

    public string? DefaultNewLayerName { get; } = new LocalizedString(NewLayerKey);

    [Settings.Inherited]
    public double FontSize
    {
        get => GetValue<double>();
    }

    public TextToolViewModel()
    {
        Toolbar = ToolbarFactory.Create<TextToolViewModel, TextToolbar>(this);
    }

    public override void UseTool(VecD pos)
    {
        ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseTextTool();
    }

    protected override void OnSelected(bool restoring)
    {
        if (!restoring)
        {
            ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseTextTool();
            ActionDisplay = new LocalizedString("TEXT_TOOL_ACTION_DISPLAY");
        }
    }

    protected override void OnDeselecting(bool transient)
    {
        if (!transient)
        {
            ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument?.Operations.TryStopToolLinkedExecutor();
        }
    }

    protected override void OnSelectedLayersChanged(IStructureMemberHandler[] layers)
    {
        OnDeselecting(false);
        OnSelected(false);
    }

    public override void OnPostUndoInlet()
    {
        OnDeselecting(false);
        OnSelected(false);
    }

    public override void OnPostRedoInlet()
    {
        OnDeselecting(false);
        OnSelected(false);
    }
}
