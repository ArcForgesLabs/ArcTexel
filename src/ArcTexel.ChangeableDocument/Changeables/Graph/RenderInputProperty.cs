using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.ChangeableDocument.Rendering;
using Drawie.Backend.Core.Surfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph;

public class RenderInputProperty : InputProperty<Painter?>
{
    internal RenderInputProperty(Node node, string internalName, string displayName, Painter? defaultValue) : base(node, internalName, displayName, defaultValue)
    {
        
    }
}
