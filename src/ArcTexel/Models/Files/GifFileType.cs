using Avalonia.Media;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Models.Files;

internal class GifFileType : VideoFileType
{
    public static GifFileType GifFile { get; } = new GifFileType();
    public override string[] Extensions { get; } = new[] { ".gif" };
    public override string DisplayName => new LocalizedString("GIF_FILE");
    public override SolidColorBrush EditorColor { get; } = new SolidColorBrush(new Color(255, 180, 0, 255));
}
