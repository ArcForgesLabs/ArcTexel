using Drawie.Backend.Core.Surfaces.PaintImpl;
using Drawie.Backend.Core.Vector;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces.Shapes;

public interface IReadOnlyPathData : IReadOnlyShapeVectorData, IReadOnlyStrokeJoinable
{
    public VectorPath Path { get; }
}
