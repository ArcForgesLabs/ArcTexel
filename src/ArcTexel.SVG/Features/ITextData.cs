using ArcTexel.SVG.Enums;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface ITextData
{
    public SvgProperty<SvgStringUnit> FontFamily { get; }
    public SvgProperty<SvgNumericUnit> FontSize { get; }
    public SvgProperty<SvgEnumUnit<SvgFontWeight>> FontWeight { get; }
    public SvgProperty<SvgEnumUnit<SvgFontStyle>> FontStyle { get; }
}
