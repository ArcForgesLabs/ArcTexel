using ChunkyImageLib.DataHolders;
using Drawie.Backend.Core.Numerics;
using Drawie.Numerics;
using ArcTexel.Models.Handlers;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;

public interface ITransformableExecutor : IExecutorFeature
{
    public bool IsTransforming { get; }
    public void OnTransformChanged(ShapeCorners corners); 
    public void OnTransformApplied();
    public void OnLineOverlayMoved(VecD start, VecD end);
    public void OnSelectedObjectNudged(VecI distance);
    public bool IsTransformingMember(Guid id);
}
