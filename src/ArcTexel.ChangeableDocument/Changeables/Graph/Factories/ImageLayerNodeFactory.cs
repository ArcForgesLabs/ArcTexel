using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Factories;

public class ImageLayerNodeFactory : NodeFactory<ImageLayerNode>
{
    public override ImageLayerNode CreateNode(IReadOnlyDocument document)
    {
        return new ImageLayerNode(document.Size, document.ProcessingColorSpace);
    }
}
