using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Text;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.Text;

[NodeViewModel("SLICE_TEXT_NODE", "SHAPE", ArcPerfectIcons.Slice)]
internal class SliceTextNodeViewModel : NodeViewModel<SliceTextNode>
{
    private NodePropertyViewModel<bool> _useLengthProperty;
    private NodePropertyViewModel _lengthProperty;
    
    public override void OnInitialized()
    {
        _useLengthProperty = FindInputProperty<bool>("UseLength");
        _lengthProperty = FindInputProperty("Length");
        
        _useLengthProperty.ValueChanged += UseLengthValueChanged;
    }

    private void UseLengthValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args) =>
        _lengthProperty.IsVisible = _useLengthProperty.Value;
}
