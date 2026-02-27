using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.Sdk.Attributes;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("Layout")]
public sealed class Layout : SingleChildLayoutElement
{
    public Layout(ILayoutElement<ControlDefinition> body = null)
    {
        Child = body;
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition layout = new ControlDefinition(UniqueId, GetType());

        if (Child != null)
            layout.AddChild(Child.BuildNative());

        return layout;
    }

}
