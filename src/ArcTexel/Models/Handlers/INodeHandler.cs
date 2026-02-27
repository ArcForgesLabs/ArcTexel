using System.ComponentModel;
using Avalonia;
using Avalonia.Media;
using ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;
using ArcTexel.Models.Structures;
using Drawie.Numerics;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.Models.Handlers;

public interface INodeHandler : INotifyPropertyChanged, IDisposable
{
    public Guid Id { get; }
    public string NodeNameBindable { get; set; }
    public IBrush CategoryBackgroundBrush { get; }
    public string InternalName { get; }
    public NodeMetadata Metadata { get; set; }
    public ObservableRangeCollection<INodePropertyHandler> Inputs { get; }
    public ObservableRangeCollection<INodePropertyHandler> Outputs { get; }
    public TexturePreview? Preview { get; set; }
    public VecD PositionBindable { get; set; }
    public Rect UiSize { get; set; }
    public bool IsNodeSelected { get; set; }
    public string Icon { get; }
    public void TraverseBackwards(Func<INodeHandler, Traverse> func);
    public void TraverseBackwards(Func<INodeHandler, INodeHandler, Traverse> func);
    public void TraverseBackwards(Func<INodeHandler, INodeHandler, INodePropertyHandler, Traverse> func);
    public void TraverseForwards(Func<INodeHandler, Traverse> func);
    public void TraverseForwards(Func<INodeHandler, INodeHandler, Traverse> func);
    public void TraverseForwards(Func<INodeHandler, INodeHandler, INodePropertyHandler, Traverse> func);
    public void TraverseForwards(Func<INodeHandler, INodeHandler, INodePropertyHandler, INodePropertyHandler, Traverse> func);
    public HashSet<NodeFrameViewModelBase> Frames { get; }
    public IReadOnlyDictionary<string, INodePropertyHandler> InputPropertyMap { get; }
    public IReadOnlyDictionary<string, INodePropertyHandler> OutputPropertyMap { get; }
}
