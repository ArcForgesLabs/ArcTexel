using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface IFilterable
{
    public SvgProperty<SvgFilterUnit> Filter { get; }
}
