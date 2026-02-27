using ArcTexel.SVG.Enums;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface IStrokable
{
    public SvgProperty<SvgPaintServerUnit> Stroke { get; }
    public SvgProperty<SvgNumericUnit> StrokeOpacity { get; }
    public SvgProperty<SvgNumericUnit> StrokeWidth { get; }
    public SvgProperty<SvgEnumUnit<SvgStrokeLineCap>> StrokeLineCap { get; }
    public SvgProperty<SvgEnumUnit<SvgStrokeLineJoin>> StrokeLineJoin { get; }
}
