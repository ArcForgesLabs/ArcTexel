using System.Collections;
using ArcTexel.Extensions.CommonApi.Utilities;

namespace ArcTexel.Extensions.CommonApi.FlyUI.Events;

public class ToggleEventArgs : ElementEventArgs<ToggleEventArgs>
{
    public bool IsToggled { get; }

    public ToggleEventArgs(bool isToggled)
    {
        IsToggled = isToggled;
    }

    protected override void SerializeArgs(ByteWriter writer)
    {
        writer.WriteBool(IsToggled);
    }
}
