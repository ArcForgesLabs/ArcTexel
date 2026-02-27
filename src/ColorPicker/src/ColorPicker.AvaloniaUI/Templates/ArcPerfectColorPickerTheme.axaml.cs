using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace ColorPicker.AvaloniaUI.Templates;

public class ArcPerfectColorPickerTheme : Styles
{
    public ArcPerfectColorPickerTheme()
    {
        AvaloniaXamlLoader.Load(this);
    }
}