using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.Windowing;

namespace ArcTexel.Extensions.CommonApi.Ui;

public interface IVisualTreeProvider
{
    public ILayoutElement<T>? FindElement<T>(string name);
    public ILayoutElement<T>? FindElement<T>(string name, IPopupWindow root);
}
