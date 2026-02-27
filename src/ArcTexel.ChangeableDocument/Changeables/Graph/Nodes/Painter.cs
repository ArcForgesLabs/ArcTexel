using ArcTexel.ChangeableDocument.Rendering;
using Drawie.Backend.Core.Surfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;

public class Painter(Action<RenderContext, Canvas> paint)
{
    public Action<RenderContext, Canvas> Paint { get; } = paint;

    public override int GetHashCode()
    {
        return Paint.GetHashCode();
    }
}
