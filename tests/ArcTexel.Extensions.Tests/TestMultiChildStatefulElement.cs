using ArcTexel.Extensions.FlyUI.Elements;

namespace ArcTexel.Extensions.Test;

public class TestMultiChildStatefulElement : StatefulElement<TestMultiChildState>
{
    public override TestMultiChildState CreateState()
    {
        return new TestMultiChildState();
    }
}
