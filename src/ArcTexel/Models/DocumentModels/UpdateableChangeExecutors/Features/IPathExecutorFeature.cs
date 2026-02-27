using Drawie.Backend.Core.Vector;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;

public interface IPathExecutorFeature : IExecutorFeature
{
    public void OnPathChanged(VectorPath path);
}
