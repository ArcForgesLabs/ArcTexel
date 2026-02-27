using Avalonia.Controls;
using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.State;

namespace ArcTexel.Extensions.FlyUI.Elements;

public abstract class State : IState<Control>
{
    public ILayoutElement<Control> Build() => BuildElement();
    public abstract LayoutElement BuildElement();

    public void SetState(Action setAction)
    {
        setAction();
        StateChanged?.Invoke();
    }

    public event Action StateChanged;
}
