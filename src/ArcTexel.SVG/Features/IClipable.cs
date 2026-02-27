using ArcTexel.SVG.Elements;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Features;

public interface IClipable
{
    public SvgProperty<SvgStringUnit> ClipPath { get; }
}
