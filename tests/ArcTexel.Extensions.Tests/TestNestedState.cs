using ArcTexel.Extensions.FlyUI.Elements;

namespace ArcTexel.Extensions.Test;

public class TestNestedState : State
{
    public override LayoutElement BuildElement()
    {
        return new TestStatefulElement();
    }
}
