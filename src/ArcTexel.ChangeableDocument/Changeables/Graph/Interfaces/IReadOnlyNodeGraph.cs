using ArcTexel.ChangeableDocument.Rendering;
using Drawie.Backend.Core;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Workspace;
using ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;
using ArcTexel.Common;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IReadOnlyNodeGraph : ICacheable, IDisposable
{
    public IReadOnlyBlackboard Blackboard { get; }
    public IReadOnlyCollection<IReadOnlyNode> AllNodes { get; }
    public IReadOnlyNode LookupNode(Guid guid);
    public IReadOnlyNode OutputNode { get; }
    public void AddNode(IReadOnlyNode node);
    public void RemoveNode(IReadOnlyNode node);
    public bool TryTraverse(Action<IReadOnlyNode> action);
    public bool TryTraverse(IReadOnlyNode end, Action<IReadOnlyNode> action);
    public void Execute(RenderContext context);
    public void Execute(IReadOnlyNode end, RenderContext context);
    Queue<IReadOnlyNode> CalculateExecutionQueue(IReadOnlyNode endNode);
    public IReadOnlyNodeGraph Clone();
    public void Execute(IEnumerable<IReadOnlyNode> nodes, RenderContext context);
}
