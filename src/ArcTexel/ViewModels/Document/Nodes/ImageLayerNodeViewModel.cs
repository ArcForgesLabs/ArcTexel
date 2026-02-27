using ArcTexel.ChangeableDocument.Actions.Generated;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Tools.Tools;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("IMAGE_LAYER_NODE", "STRUCTURE", ArcPerfectIcons.LayersDouble)]
internal class ImageLayerNodeViewModel : StructureMemberViewModel<ImageLayerNode>, ITransparencyLockableMember, IRasterLayerHandler
{
    bool lockTransparency;
    public void SetLockTransparency(bool lockTransparency)
    {
        this.lockTransparency = lockTransparency;
        OnPropertyChanged(nameof(LockTransparencyBindable));
    }
    
    public bool LockTransparencyBindable
    {
        get => lockTransparency;
        set
        {
            if (!Document.BlockingUpdateableChangeActive)
                Internals.ActionAccumulator.AddFinishedActions(new LayerLockTransparency_Action(Id, value));
        }
    }

    private bool shouldDrawOnMask = false;
    public bool ShouldDrawOnMask
    {
        get => shouldDrawOnMask;
        set
        {
            if (value == shouldDrawOnMask)
                return;
            shouldDrawOnMask = value;
            OnPropertyChanged(nameof(ShouldDrawOnMask));
        }
    }

    public Type? QuickEditTool { get; } = typeof(PenToolViewModel);
}
