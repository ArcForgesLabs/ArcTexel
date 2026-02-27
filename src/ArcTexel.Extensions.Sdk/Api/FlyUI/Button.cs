using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.Events;
using ArcTexel.Extensions.Sdk.Attributes;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

[ControlTypeId("Button")]
public class Button : SingleChildLayoutElement
{
    public event ElementEventHandler Click
    {
        add => AddEvent(nameof(Click), value);
        remove => RemoveEvent(nameof(Click), value);
    }

    public Button(ILayoutElement<ControlDefinition> child = null, ElementEventHandler onClick = null, Cursor? cursor = null) : base(cursor)
    {
        Child = child;
        if (onClick != null)
            Click += onClick;
    }

    protected override ControlDefinition CreateControl()
    {
        ControlDefinition button = new ControlDefinition(UniqueId, GetType());
        if (Child != null)
            button.AddChild(Child.BuildNative());

        return button;
    }
}
