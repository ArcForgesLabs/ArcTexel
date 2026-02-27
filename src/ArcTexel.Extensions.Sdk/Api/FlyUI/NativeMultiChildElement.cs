using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.Sdk.Attributes;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("MultiChildNativeElement")]
public class NativeMultiChildElement : LayoutElement
{
    public NativeMultiChildElement(Cursor? cursor) : base(cursor)
    {
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition control = new ControlDefinition(UniqueId, GetType());
        return control;
    }

    public void AppendChild(int atIndex, LayoutElement layoutElement)
    {
        if (layoutElement == null)
            return;

        Interop.AppendElementToNativeMultiChild(UniqueId, layoutElement, atIndex);
    }
}
