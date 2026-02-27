using Avalonia.Input;

namespace ArcTexel.Extensions.UI.Overlays;

public interface IOverlayPointer
{
    public PointerType Type { get; }
    public void Capture(IOverlay? overlay);
}
