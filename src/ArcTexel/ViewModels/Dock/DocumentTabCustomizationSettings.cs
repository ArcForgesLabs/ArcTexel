using Avalonia.Media;
using ArcDocks.Core.Docking;
using ArcTexel.Helpers;

namespace ArcTexel.ViewModels.Dock;

public class DocumentTabCustomizationSettings : TabCustomizationSettings
{
    private FontStyle fontStyle = FontStyle.Normal;
    private SavedState savedState;
    public SavedState SavedState
    {
        get => savedState;
        set
        {
            SetField(ref savedState, value);
            OnPropertyChanged(nameof(ShowUnsavedDot));
            OnPropertyChanged(nameof(SavedStateColor));
        }
    }

    public FontStyle FontStyle
    {
        get => fontStyle;
        set => SetField(ref fontStyle, value);
    }

    public bool ShowUnsavedDot => SavedState != SavedState.Saved;
    public IBrush SavedStateColor => SavedState switch
    {
        SavedState.Saved => Brushes.Transparent,
        SavedState.Autosaved => ThemeResources.AutosaveDotBrush,
        SavedState.Unsaved => ThemeResources.UnsavedDotBrush,
        _ => Brushes.Transparent
    };

    public DocumentTabCustomizationSettings(object? icon = null, bool showCloseButton = false, SavedState savedState = SavedState.Saved) : base(icon, showCloseButton)
    {
        SavedState = savedState;
    }
}

public enum SavedState
{
    Saved,
    Autosaved,
    Unsaved
}
