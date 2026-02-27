using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Svg.Skia;
using ArcTexel.Helpers.Converters;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.ViewModels.Dock;

internal class ColorPickerDockViewModel : DockableViewModel
{
    public const string TabId = "ColorPicker";
    public override string Id => TabId;
    public override string Title => new LocalizedString("COLOR_PICKER_TITLE");
    public override bool CanFloat => true;
    public override bool CanClose => true;

    private ColorsViewModel colorsSubViewModel;

    public ColorsViewModel ColorsSubViewModel
    {
        get => colorsSubViewModel;
        set => SetProperty(ref colorsSubViewModel, value);
    }

    public ColorPickerDockViewModel(ColorsViewModel colorsSubVm)
    {
        ColorsSubViewModel = colorsSubVm;
        TabCustomizationSettings.Icon = ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.ColorPicker);
    }
}
