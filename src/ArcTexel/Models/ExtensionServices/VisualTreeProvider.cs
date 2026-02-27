using Avalonia.Controls;
using Avalonia.VisualTree;
using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.Ui;
using ArcTexel.Extensions.CommonApi.Windowing;
using ArcTexel.Extensions.FlyUI.Elements;
using ArcTexel.Extensions.FlyUI.Elements.Native;
using ArcTexel.Extensions.Windowing;
using ArcTexel.Views;

namespace ArcTexel.Models.ExtensionServices;

public class VisualTreeProvider : IVisualTreeProvider
{
    ILayoutElement<T>? IVisualTreeProvider.FindElement<T>(string name)
    {
        return FindElement(name) as ILayoutElement<T>;
    }

    ILayoutElement<T>? IVisualTreeProvider.FindElement<T>(string name, IPopupWindow root)
    {
        return FindElement(name, root) as ILayoutElement<T>;
    }

    public ILayoutElement<Control>? FindElement(string name, IPopupWindow root)
    {
        var control = RecursiveLookup((root as PopupWindow).UnderlyingWindow as Window, name);
        return ToNativeType(control);
    }
    public ILayoutElement<Control>? FindElement(string name)
    {
        var control = RecursiveLookup(MainWindow.Current, name);
        return ToNativeType(control);
    }

    private static ILayoutElement<Control>? ToNativeType(Control? control)
    {
        if (control is null)
        {
            return null;
        }

        if (control is Panel panel)
        {
            NativeMultiChildElement nativeElement = new NativeMultiChildElement(panel);
            return nativeElement;
        }

        return new NativeElement(control);
    }

    private Control? RecursiveLookup(Control? control, string name)
    {
        if (control is null)
        {
            return null;
        }

        if (control.Name == name)
        {
            return control;
        }

        foreach (var child in control.GetVisualChildren())
        {
            var found = RecursiveLookup(child as Control, name);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
}
