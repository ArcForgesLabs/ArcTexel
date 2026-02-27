using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.Ui;
using ArcTexel.Extensions.CommonApi.Windowing;
using ArcTexel.Extensions.Sdk.Api.FlyUI;
using ArcTexel.Extensions.Sdk.Api.Window;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.Ui;

public class VisualTreeProvider : IVisualTreeProvider
{
    ILayoutElement<T>? IVisualTreeProvider.FindElement<T>(string name)
    {
        return FindElement(name) as ILayoutElement<T>;
    }

    ILayoutElement<T> IVisualTreeProvider.FindElement<T>(string name, IPopupWindow root)
    {
        return FindElement(name, root as PopupWindow) as ILayoutElement<T>;
    }

    public LayoutElement? FindElement(string name)
    {
        return Interop.FindUiElement(name);
    }

    public LayoutElement? FindElement(string name, PopupWindow root)
    {
        if (root == null)
        {
            throw new ArgumentNullException(nameof(root), "Root window cannot be null.");
        }

        return Interop.FindUiElement(name, root);
    }
}
