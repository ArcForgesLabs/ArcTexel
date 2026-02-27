using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.State;

namespace ArcTexel.Extensions.Sdk.Api.FlyUI;

public abstract class State : IState<ControlDefinition>
{
    ILayoutElement<ControlDefinition> IState<ControlDefinition>.Build()
    {
        return BuildElement();
    }

    public abstract LayoutElement BuildElement();

    public void SetState(Action setAction)
    {
        setAction();
        StateChanged?.Invoke();
    }

    public event Action StateChanged;
}
