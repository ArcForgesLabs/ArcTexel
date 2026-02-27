using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.Sdk.Attributes;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("Center")]
public class Center : SingleChildLayoutElement
{
    public Center(ILayoutElement<ControlDefinition> child, Cursor? cursor = null) : base(cursor)
    {
        Child = child;
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition center = new ControlDefinition(UniqueId, GetType());

        if (Child != null)
            center.AddChild(Child.BuildNative());

        return center;
    }
}
