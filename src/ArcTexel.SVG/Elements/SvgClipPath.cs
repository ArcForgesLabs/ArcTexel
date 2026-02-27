using System.Xml;
using ArcTexel.SVG.Enums;
using ArcTexel.SVG.Features;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Elements;

public class SvgClipPath() : SvgElement("clipPath"), IElementContainer
{
    public List<SvgElement> Children { get; } = new();

    public SvgProperty<SvgEnumUnit<SvgRelativityUnit>> ClipPathUnits { get; } = new("clipPathUnits");

    public override void ParseAttributes(XmlReader reader, SvgDefs defs)
    {
        List<SvgProperty> properties = new List<SvgProperty>() { ClipPathUnits };
        ParseAttributes(properties, reader, defs);
    }
}
