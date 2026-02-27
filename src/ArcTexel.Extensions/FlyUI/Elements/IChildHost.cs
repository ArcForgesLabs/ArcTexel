using Avalonia.Controls;
using ArcTexel.Extensions.CommonApi.FlyUI;

namespace ArcTexel.Extensions.FlyUI.Elements;

public interface IChildHost : IEnumerable<ILayoutElement<Control>>
{
    public void DeserializeChildren(List<ILayoutElement<Control>> children);
    public void AddChild(ILayoutElement<Control> child);
    public void RemoveChild(ILayoutElement<Control> child);
    public void AppendChild(int atIndex, ILayoutElement<Control> deserializedChild);
}
