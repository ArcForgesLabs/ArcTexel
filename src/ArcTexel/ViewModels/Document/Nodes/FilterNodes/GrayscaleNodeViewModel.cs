using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.FilterNodes;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.FilterNodes;

[NodeViewModel("GRAYSCALE_FILTER_NODE", "FILTERS", ArcPerfectIcons.Ghost)]
internal class GrayscaleNodeViewModel : NodeViewModel<GrayscaleNode>
{
    private INodePropertyHandler customWeightsProp;
    public override void OnInitialized()
    {
        var modeProp = Inputs.FirstOrDefault(x => x.PropertyName == "Mode");
        customWeightsProp = Inputs.FirstOrDefault(x => x.PropertyName == "CustomWeight");
        modeProp.ValueChanged += ModePropOnValueChanged;

        customWeightsProp.IsVisible = modeProp.Value is GrayscaleNode.GrayscaleMode.Custom;
    }

    private void ModePropOnValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args)
    {
        customWeightsProp.IsVisible = args.NewValue is GrayscaleNode.GrayscaleMode.Custom;
    }
}
