using ChunkyImageLib;
using Drawie.Backend.Core;
using ArcTexel.Models.Rendering;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.Models.Handlers;

internal interface ICelHandler : IDisposable
{
    TexturePreview? PreviewTexture { get; set; }
    public int StartFrameBindable { get; }
    public int DurationBindable { get; }
    public bool IsSelected { get; set; }
    public Guid LayerGuid { get; }
    public Guid Id { get; }
    public bool IsVisible { get; }
    public IDocument Document { get; }
    bool IsWithinRange(int frame);
}
