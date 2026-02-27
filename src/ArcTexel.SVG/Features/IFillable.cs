using ArcTexel.SVG.Enums;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface IFillable
{
    public SvgProperty<SvgPaintServerUnit> Fill { get; }
    public SvgProperty<SvgNumericUnit> FillOpacity { get; }
}
