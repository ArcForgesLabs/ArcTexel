using ArcTexel.ChangeableDocument.Changeables.Animations;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

namespace ArcTexel.Models.Handlers;

internal interface IVectorLayerHandler : ILayerHandler
{
    public IReadOnlyShapeVectorData? GetShapeData(KeyFrameTime frameTime);
}
