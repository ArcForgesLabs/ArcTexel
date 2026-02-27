using ArcTexel.Extensions.FlyUI.Elements;

namespace ArcTexel.Extensions.Test;

public class TestStatefulElement : StatefulElement<TestState>
{
    public override TestState CreateState()
    {
        return new();
    }
}
