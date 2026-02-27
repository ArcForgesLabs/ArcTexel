using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Interfaces;

public interface IReadOnlyRasterKeyFrame : IReadOnlyKeyFrame
{
    IReadOnlyChunkyImage GetTargetImage(IReadOnlyCollection<IReadOnlyNode> allNodes);
}
