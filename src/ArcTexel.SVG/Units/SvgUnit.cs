using ArcTexel.SVG.Elements;

namespace ArcTexel.SVG.Units;

public interface ISvgUnit
{
    public string ToXml(DefStorage defs);
    public void ValuesFromXml(string readerValue, SvgDefs defs);
}
