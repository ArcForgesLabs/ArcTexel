using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Shapes.Data;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IReadOnlyVectorNode : IReadOnlyLayerNode
{
    public IReadOnlyShapeVectorData? ShapeData { get; }
}
