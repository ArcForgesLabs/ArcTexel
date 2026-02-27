using Avalonia.Input;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Preferences;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.UserPreferences.Settings;

namespace ArcTexel.ViewModels.SubViewModels;
#nullable enable
internal class ViewOptionsViewModel : SubViewModel<ViewModelMain>
{
    private bool gridLinesEnabled;

    public bool GridLinesEnabled
    {
        get => gridLinesEnabled;
        set => SetProperty(ref gridLinesEnabled, value);
    }
    
    private bool snappingEnabled = true;
    public bool SnappingEnabled
    {
        get => snappingEnabled;
        set
        {
            SetProperty(ref snappingEnabled, value);
            Owner.DocumentManagerSubViewModel.ActiveDocument.SnappingViewModel.SnappingController.SnappingEnabled = value;
        }
    }
    
    private bool highResRender = true;
    public bool HighResRender
    {
        get => highResRender;
        set
        {
            SetProperty(ref highResRender, value);
            Owner.DocumentManagerSubViewModel.ActiveDocument.SceneRenderer.HighResRendering = value;
        }
    }

    private int maxBilinearSampleSize = 4096;
    public int MaxBilinearSampleSize
    {
        get => maxBilinearSampleSize;
        set
        {
            SetProperty(ref maxBilinearSampleSize, value);
        }
    }

    public ViewOptionsViewModel(ViewModelMain owner)
        : base(owner)
    {
        MaxBilinearSampleSize = ArcTexelSettings.Performance.MaxBilinearSampleSize.Value;

        ArcTexelSettings.Performance.MaxBilinearSampleSize.ValueChanged += (s, e) =>
        {
            MaxBilinearSampleSize = ArcTexelSettings.Performance.MaxBilinearSampleSize.Value;
        };
    }

    [Command.Basic("ArcTexel.View.ToggleGrid", "TOGGLE_GRIDLINES", "TOGGLE_GRIDLINES", Key = Key.OemTilde,
        Modifiers = KeyModifiers.Control,
        Icon = ArcPerfectIcons.Grid)]
    public void ToggleGridLines()
    {
        GridLinesEnabled = !GridLinesEnabled;
    }

    [Command.Basic("ArcTexel.View.ZoomIn", 1, "ZOOM_IN", "ZOOM_IN", CanExecute = "ArcTexel.HasDocument",
        Key = Key.OemPlus,
        Icon = ArcPerfectIcons.ZoomIn, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.View.Zoomout", -1, "ZOOM_OUT", "ZOOM_OUT", CanExecute = "ArcTexel.HasDocument",
        Key = Key.OemMinus,
        Icon = ArcPerfectIcons.ZoomOut, AnalyticsTrack = true)]
    public void ZoomViewport(double zoom)
    {
        ViewportWindowViewModel? viewport = Owner.WindowSubViewModel.ActiveWindow as ViewportWindowViewModel;
        if (viewport is null)
            return;
        viewport.ZoomViewportTrigger.Execute(this, zoom);
    }

    [Command.Basic("ArcTexel.ToggleSnapping", "TOGGLE_SNAPPING", "TOGGLE_SNAPPING",
        Icon = ArcPerfectIcons.Snapping)]
    public void ToggleSnapping()
    {
        SnappingEnabled = !SnappingEnabled;
    }
}
