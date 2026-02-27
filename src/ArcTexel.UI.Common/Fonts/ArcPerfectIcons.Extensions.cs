using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using ArcTexel.UI.Common.Rendering;

namespace ArcTexel.UI.Common.Fonts;

public static class ArcPerfectIconExtensions
{
    private static readonly FontFamily arcPerfectFontFamily =
        new("avares://ArcTexel.UI.Common/Fonts/ArcPerfect.ttf#pixiperfect");


    public static Stream GetFontStream()
    {
        return AssetLoader.Open(new Uri("avares://ArcTexel.UI.Common/Fonts/ArcPerfect.ttf"));
    }

    public static IImage ToIcon(string unicode, double size = 18)
    {
        if (string.IsNullOrEmpty(unicode)) return null;

        return new IconImage(unicode, arcPerfectFontFamily, size, Colors.White);
    }

    public static IImage ToIcon(string unicode, double size, double rotation)
    {
        if (string.IsNullOrEmpty(unicode)) return null;

        return new IconImage(unicode, arcPerfectFontFamily, size, Colors.White, rotation);
    }

    public static string? TryGetByName(string? icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            return null;
        }

        if (Application.Current.Styles.TryGetResource(icon, null, out object resource))
        {
            return resource as string;
        }

        return icon;
    }
}
