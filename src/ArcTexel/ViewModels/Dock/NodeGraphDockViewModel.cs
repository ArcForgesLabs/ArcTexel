using Avalonia.Input;
using ArcDocks.Core.Docking.Events;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.ViewModels.Dock;

internal class NodeGraphDockViewModel : DockableViewModel, IDockableSelectionEvents
{
    private DocumentManagerViewModel document;

    public const string TabId = "NodeGraph";

    public override string Id { get; } = TabId;
    public override string Title => new LocalizedString("NODE_GRAPH_TITLE");
    public override bool CanFloat => true;
    public override bool CanClose => true;

    public DocumentManagerViewModel DocumentManagerSubViewModel
    {
        get => document;
        set => SetProperty(ref document, value);
    }

    public NodeGraphDockViewModel(DocumentManagerViewModel document)
    {
        DocumentManagerSubViewModel = document;

        TabCustomizationSettings.Icon = ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.Nodes);
    }

    void IDockableSelectionEvents.OnSelected()
    {
        DocumentManagerSubViewModel.Owner.ShortcutController.OverwriteContext(this.GetType());
    }

    void IDockableSelectionEvents.OnDeselected()
    {
        DocumentManagerSubViewModel.Owner.ShortcutController.ClearContext(this.GetType());
    }
}
