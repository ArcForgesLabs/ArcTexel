using ArcTexel.ChangeableDocument.Changeables.Graph;
using Drawie.Backend.Core.Surfaces.PaintImpl;

namespace ArcTexel.ChangeableDocument.Helpers;

internal static class PaintHelper
{
    public static void SetFilters(this Paint paint, Filter? filter)
    {
        paint.ColorFilter = filter?.ColorFilter;
        paint.ImageFilter = filter?.ImageFilter;
    }
}
