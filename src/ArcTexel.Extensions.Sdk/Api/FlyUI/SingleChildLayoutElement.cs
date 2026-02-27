using ArcTexel.Extensions.CommonApi.FlyUI;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

public abstract class SingleChildLayoutElement : LayoutElement, ISingleChildLayoutElement<ControlDefinition>
{
    public ILayoutElement<ControlDefinition> Child { get; set; }

    public SingleChildLayoutElement(Cursor? cursor = null) : base(cursor)
    {
    }
}
