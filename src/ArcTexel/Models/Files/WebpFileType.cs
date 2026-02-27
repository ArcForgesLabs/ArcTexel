using Avalonia.Media;
using Drawie.Backend.Core.Surfaces;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Models.Files;

internal class WebpFileType : ImageFileType
{
    public override string[] Extensions { get; } = [".webp"];

    public override string DisplayName => new LocalizedString("WEBP_FILE");

    public override EncodedImageFormat EncodedImageFormat { get; } = EncodedImageFormat.Webp;

    public override SolidColorBrush EditorColor { get; } = new(new Color(255, 255, 238, 111));
}
