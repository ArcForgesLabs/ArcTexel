using Avalonia.Controls;

namespace ArcTexel.Extensions.FlyUI.Elements.Native;

public class NativeElement : LayoutElement
{
    public Control Native { get; }

    public NativeElement(Control native)
    {
        Native = native;
    }
    protected override Control CreateNativeControl()
    {
        return Native;
    }
}
