using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface IOpacity
{
    public SvgProperty<SvgNumericUnit> Opacity { get; }
}
