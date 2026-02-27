using ArcTexel.Models.Handlers;

namespace ArcTexel.ViewModels.Nodes;

internal sealed class NodeFrameViewModel : NodeFrameViewModelBase
{
    public NodeFrameViewModel(Guid id, IEnumerable<INodeHandler> nodes) : base(id, nodes)
    {
        CalculateBounds();
    }

    protected override void CalculateBounds()
    {
        throw new NotImplementedException();
    }
}
