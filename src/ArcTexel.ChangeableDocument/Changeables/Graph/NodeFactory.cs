using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Graph;

public interface INodeFactory
{
    public Type NodeType { get; }

    public Node CreateNode(IReadOnlyDocument document);
}

public abstract class NodeFactory<T> : INodeFactory where T : Node
{
    public Type NodeType { get; } = typeof(T);
    
    public abstract T CreateNode(IReadOnlyDocument document);

    Node INodeFactory.CreateNode(IReadOnlyDocument document)
    {
        return CreateNode(document);
    }
}
