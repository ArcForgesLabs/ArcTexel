using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface ITransformable
{
    public SvgProperty<SvgTransformUnit> Transform { get; }
}
