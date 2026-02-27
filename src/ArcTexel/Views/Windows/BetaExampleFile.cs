using Avalonia.Platform;
using Drawie.Backend.Core;
using ArcTexel.Models.IO;
using ArcTexel.Parser;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Views.Windows;

public class BetaExampleFile : IDisposable
{
    private readonly string resourcePath;
    
    public Texture PreviewImage { get; }
    
    public LocalizedString DisplayName { get; }
    
    public BetaExampleFile(string name, LocalizedString displayName)
    {
        resourcePath = Path.Combine(Paths.DataResourceUri, "BetaExampleFiles", name);
        DisplayName = displayName;
        
        var stream = GetStream();
        var bytes = ArcParser.ReadPreview(stream);

        PreviewImage = Texture.Load(bytes);
    }
    
    public Stream GetStream() => AssetLoader.Open(new Uri(resourcePath));

    public void Dispose()
    {
        PreviewImage.Dispose();
    }
}
