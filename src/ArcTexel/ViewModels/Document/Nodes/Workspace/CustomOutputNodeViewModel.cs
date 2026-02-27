using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Workspace;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.Workspace;

[NodeViewModel("CUSTOM_OUTPUT_NODE", "WORKSPACE", ArcPerfectIcons.Surveillance)]
internal class CustomOutputNodeViewModel : NodeViewModel<CustomOutputNode>
{
    public override void OnInitialized()
    {
        InputPropertyMap[CustomOutputNode.FullViewportRenderPropertyName].ValueChanged += (property, args) =>
        {
            InputPropertyMap[CustomOutputNode.SizePropertyName].IsVisible = !(bool)args.NewValue!;
        };
    }
}
