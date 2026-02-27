using ArcTexel.ChangeableDocument.Actions.Generated;
using ArcTexel.ChangeableDocument.Changeables.Animations;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces.Shapes;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Tools.Tools;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("VECTOR_LAYER", "STRUCTURE", ArcPerfectIcons.VectorPen)]
internal class VectorLayerNodeViewModel : StructureMemberViewModel<VectorLayerNode>, IVectorLayerHandler
{
    private Dictionary<Type, Type> quickToolsMap = new Dictionary<Type, Type>()
    {
        { typeof(IReadOnlyEllipseData), typeof(VectorEllipseToolViewModel) },
        { typeof(IReadOnlyRectangleData), typeof(VectorRectangleToolViewModel) },
        { typeof(IReadOnlyLineData), typeof(VectorLineToolViewModel) },
        { typeof(IReadOnlyTextData), typeof(TextToolViewModel) },
        { typeof(IReadOnlyPathData), typeof(VectorPathToolViewModel) }
    };
    
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

    public Type? QuickEditTool
    {
        get
        {
            var shapeData = GetShapeData(Document.AnimationDataViewModel.ActiveFrameTime);
            if (shapeData is null)
                return null;

            foreach (var tool in quickToolsMap)
            {
                if(shapeData.GetType().IsAssignableTo(tool.Key))
                    return tool.Value;
            }
            
            return null;
        }
    }

    public IReadOnlyShapeVectorData? GetShapeData(KeyFrameTime frameTime)
    {
        return ((IReadOnlyVectorNode)Internals.Tracker.Document.FindMember(Id))?.ShapeData;
    }
}
