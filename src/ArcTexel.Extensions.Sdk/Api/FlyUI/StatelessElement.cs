using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.State;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

public abstract class StatelessElement : LayoutElement, IStatelessElement<ControlDefinition>
{
    protected StatelessElement() : base(null)
    {
    }

    protected StatelessElement(Cursor? cursor) : base(cursor)
    {
    }

    public virtual ILayoutElement<ControlDefinition> Build()
    {
        return this;
    }

    public override ControlDefinition BuildNative()
    {
        return CreateControl();
    }

    protected override ControlDefinition CreateControl()
    {
        return Build().BuildNative();
    }
}
