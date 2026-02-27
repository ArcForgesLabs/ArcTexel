using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.Properties;
using ArcTexel.Extensions.Sdk.Attributes;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("Icon")]
public class Icon : LayoutElement
{
    public string IconName { get; set; }
    public double Size { get; set; } = 16;
    public Color Color { get; set; } = Colors.White;

    public Icon(string iconName, double size = 16, Color? color = null, Cursor? cursor = null) : base(cursor)
    {
        IconName = iconName;
        Size = size;
        if (color != null)
            Color = color.Value;
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition icon = new ControlDefinition(UniqueId, GetType());
        icon.AddProperty(IconName);
        icon.AddProperty(Size);
        icon.AddProperty(Color);

        return icon;
    }
}
