using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.Shaders.Generation.Expressions;
using ArcTexel.ChangeableDocument.Rendering;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;

[NodeInfo("Color")]
public class ColorNode : Node
{
    public FuncInputProperty<Half4> InputColor { get; }
    public FuncOutputProperty<Half4> Color { get; }
    
    public ColorNode()
    {
        InputColor = CreateFuncInput<Half4>("InputColor", "COLOR", Colors.White);
        Color = CreateFuncOutput<Half4>("OutputColor", "COLOR", ctx => ctx.GetValue(InputColor));
    }
    
    protected override void OnExecute(RenderContext context)
    {
        
    }

    public override Node CreateCopy()
    {
        return new ColorNode();
    }
}
