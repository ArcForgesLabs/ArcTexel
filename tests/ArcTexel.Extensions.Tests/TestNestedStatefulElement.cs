using ArcTexel.Extensions.FlyUI.Elements;

namespace ArcTexel.Extensions.Test;

public class TestNestedStatefulElement : StatefulElement<TestNestedState>
{
    public override TestNestedState CreateState()
    {
        return new();
    }
}
