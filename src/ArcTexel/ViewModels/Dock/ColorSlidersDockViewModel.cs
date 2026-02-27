using Avalonia;
using Avalonia.Media;
using ArcTexel.Helpers.Converters;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.ViewModels.Dock;

internal class ColorSlidersDockViewModel : DockableViewModel
{
    public const string TabId = "ColorSliders";
    public override string Id => TabId;
    public override string Title => new LocalizedString("COLOR_SLIDERS_TITLE");
    public override bool CanFloat => true;
    public override bool CanClose => true;

    private ColorsViewModel colorsSubViewModel;

    public ColorsViewModel ColorsSubViewModel
    {
        get => colorsSubViewModel;
        set => SetProperty(ref colorsSubViewModel, value);
    }

    public ColorSlidersDockViewModel(ColorsViewModel colorsSubVm)
    {
        ColorsSubViewModel = colorsSubVm;
        TabCustomizationSettings.Icon = ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.ColorSliders);
    }
}
