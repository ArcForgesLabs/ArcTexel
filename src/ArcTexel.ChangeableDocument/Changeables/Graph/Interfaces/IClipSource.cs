using Drawie.Backend.Core.Surfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IClipSource
{
    public void DrawClipSource(SceneObjectRenderContext context, Canvas drawOnto);
}
