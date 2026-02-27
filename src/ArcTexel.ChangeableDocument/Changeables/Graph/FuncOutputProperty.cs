using ArcTexel.ChangeableDocument.Changeables.Graph.Context;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using Drawie.Backend.Core.Shaders;

namespace ArcTexel.ChangeableDocument.Changeables.Graph;

public class FuncOutputProperty<T> : OutputProperty<Func<FuncContext, T>>
{
    internal FuncOutputProperty(Node node, string internalName, string displayName, Func<FuncContext, T> defaultValue) : base(node, internalName, displayName, defaultValue)
    {
        
    }
}
