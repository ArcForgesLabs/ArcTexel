using Avalonia.Media;
using ArcTexel.Helpers.Converters;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.ViewModels.Dock;

internal class DocumentPreviewDockViewModel : DockableViewModel
{
    public const string TabId = "DocumentPreview";

    public override string Id => TabId;
    public override string Title => new LocalizedString("PREVIEW_TITLE");
    public override bool CanFloat => true;
    public override bool CanClose => true;

    private ColorsViewModel colorsSubViewModel;

    public ColorsViewModel ColorsSubViewModel
    {
        get => colorsSubViewModel;
        set => SetProperty(ref colorsSubViewModel, value);
    }

    private DocumentManagerViewModel documentManagerSubViewModel;

    public DocumentManagerViewModel DocumentManagerSubViewModel
    {
        get => documentManagerSubViewModel;
        set => SetProperty(ref documentManagerSubViewModel, value);
    }

    public DocumentPreviewDockViewModel(ColorsViewModel colorsSubViewModel, DocumentManagerViewModel documentManagerViewModel)
    {
        ColorsSubViewModel = colorsSubViewModel;
        DocumentManagerSubViewModel = documentManagerViewModel;
        TabCustomizationSettings.Icon = ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.Compass);
    }
}
