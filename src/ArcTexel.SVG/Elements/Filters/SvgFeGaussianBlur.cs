using System.Xml;
using Drawie.Backend.Core.Surfaces;
using Drawie.Backend.Core.Surfaces.PaintImpl;
using ArcTexel.SVG.Enums;
using ArcTexel.SVG.Features;
using ArcTexel.SVG.Units;

namespace ArcTexel.SVG.Elements.Filters;

public class SvgFeGaussianBlur() : SvgFilterPrimitive("feGaussianBlur"), IImageFilter
{
    public SvgProperty<SvgNumericUnit> StdDeviation { get; } = new("stdDeviation");
    public SvgProperty<SvgEnumUnit<SvgEdgeMode>> EdgeMode { get; } = new("edgeMode");


    public override void ParseAttributes(XmlReader reader, SvgDefs defs)
    {
        List<SvgProperty> properties = new List<SvgProperty>()
        {
            X, Y, Width, Height,
            StdDeviation
        };

        ParseAttributes(properties, reader, defs);
    }

    public SvgFilterPrimitive? GetImageFilter()
    {
        return this;
    }
}
