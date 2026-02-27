using Drawie.Backend.Core.Surfaces.PaintImpl;
using Drawie.Backend.Core.Vector;
using ArcTexel.ViewModels.Tools.Tools;

namespace ArcTexel.Models.Handlers.Tools;

internal interface IVectorPathToolHandler : IToolHandler
{
    public VectorPathFillType FillMode { get; }
    public StrokeCap StrokeLineCap { get; }
    public StrokeJoin StrokeLineJoin { get; }
}
