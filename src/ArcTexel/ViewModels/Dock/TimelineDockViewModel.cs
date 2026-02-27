using ArcDocks.Core.Docking.Events;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.ViewModels.Dock;

internal class TimelineDockViewModel : DockableViewModel, IDockableSelectionEvents
{
    public const string TabId = "Timeline";

    public override string Id => TabId;
    public override string Title => new LocalizedString("TIMELINE_TITLE");
    public override bool CanFloat => true;
    public override bool CanClose => true;
    
    private DocumentManagerViewModel documentManagerSubViewModel;

    public DocumentManagerViewModel DocumentManagerSubViewModel
    {
        get => documentManagerSubViewModel;
        set => SetProperty(ref documentManagerSubViewModel, value);
    }

    public TimelineDockViewModel(DocumentManagerViewModel documentManagerViewModel)
    {
        DocumentManagerSubViewModel = documentManagerViewModel;
        TabCustomizationSettings.Icon = ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.Timeline);
    }

    void IDockableSelectionEvents.OnSelected()
    {
        documentManagerSubViewModel.Owner.ShortcutController?.OverwriteContext(GetType());
    }

    void IDockableSelectionEvents.OnDeselected()
    {
        documentManagerSubViewModel.Owner.ShortcutController?.ClearContext(GetType());
    }
}
