using ArcTexel.SVG.Elements;

namespace ArcTexel.SVG.Features;

public interface IImageFilter
{
    public SvgFilterPrimitive? GetImageFilter();
}
