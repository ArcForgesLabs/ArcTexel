using Drawie.Backend.Core.Surfaces;
using ArcTexel.ChangeableDocument.Changeables.Graph;

namespace ArcTexel.ChangeableDocument.Changeables.Interfaces;

public interface IVariableSampling
{
    public InputProperty<bool> BilinearSampling { get; }
}
