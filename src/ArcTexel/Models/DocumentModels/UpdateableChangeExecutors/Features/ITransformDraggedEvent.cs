using ChunkyImageLib.DataHolders;
using Drawie.Numerics;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;

public interface ITransformDraggedEvent : IExecutorFeature
{
    public void OnTransformDragged(VecD from, VecD to);
}
