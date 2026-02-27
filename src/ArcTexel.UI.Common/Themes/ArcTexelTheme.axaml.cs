using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;

namespace ArcTexel.UI.Common.Themes;

public class ArcTexelTheme : Styles
{
    public ArcTexelTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
        if (OperatingSystem.IsMacOS())
        {
            Application.Current.Styles.Resources["ContentControlThemeFontFamily"] = FontFamily.Parse("Arial");
        }
    }
}
