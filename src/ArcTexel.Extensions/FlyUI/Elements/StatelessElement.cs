using Avalonia.Controls;
using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.State;

namespace ArcTexel.Extensions.FlyUI.Elements;

public abstract class StatelessElement : LayoutElement, IStatelessElement<Control>
{
    public override Control BuildNative()
    {
        return CreateNativeControl();
    }

    protected override Control CreateNativeControl()
    {
        return Build().BuildNative();
    }

    public virtual ILayoutElement<Control> Build()
    {
        return this;
    }
}
