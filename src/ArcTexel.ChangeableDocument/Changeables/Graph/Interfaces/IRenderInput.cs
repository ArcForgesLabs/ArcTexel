using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core;
using Drawie.Backend.Core.Surfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IRenderInput
{
    RenderInputProperty Background { get; }
}
