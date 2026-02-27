using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.Sdk.Attributes;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("NativeElement")]
public class NativeElement : LayoutElement
{
    public NativeElement(Cursor? cursor) : base(cursor)
    {
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition control = new ControlDefinition(UniqueId, GetType());
        return control;
    }
}
