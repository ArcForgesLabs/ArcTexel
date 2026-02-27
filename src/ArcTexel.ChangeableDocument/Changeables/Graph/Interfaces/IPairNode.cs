using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IPairNode
{
    public Guid OtherNode { get; set; }
}
