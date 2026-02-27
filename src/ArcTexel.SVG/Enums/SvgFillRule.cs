using ArcTexel.SVG.Attributes;

namespace ArcTexel.SVG.Enums;

public enum SvgFillRule
{
    [SvgValue("nonzero")]
    NonZero,
    
    [SvgValue("evenodd")]
    EvenOdd
}
