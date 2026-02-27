using System.Diagnostics.CodeAnalysis;
using ArcTexel.Extensions.CommonApi.FlyUI;
using ArcTexel.Extensions.CommonApi.FlyUI.Properties;
using ArcTexel.Extensions.Sdk.Api.FlyUI;
using ArcTexel.Extensions.Sdk.Api.Window;

namespace ArcTexel.Extensions.Sdk.Tests;

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "FlyUI style")]
public class WindowContentElement : StatelessElement
{
    public override ILayoutElement<ControlDefinition> Build()
    {
        Layout layout = new Layout();
        return layout;
    }
}