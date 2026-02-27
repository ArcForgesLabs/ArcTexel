using Drawie.Backend.Core.Surfaces;
using Drawie.Backend.Core.Surfaces.PaintImpl;
using ArcTexel.ChangeableDocument.Rendering;

namespace ArcTexel.ChangeableDocument.Changeables.Interfaces;

public interface IRasterizable
{
    public void Rasterize(Canvas surface, Paint paint, int atFrame);
}
