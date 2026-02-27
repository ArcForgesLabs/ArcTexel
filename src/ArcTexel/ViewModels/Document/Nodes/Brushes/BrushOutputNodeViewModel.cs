using Drawie.Backend.Core.Bridge;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Brushes;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Nodes.Properties;

namespace ArcTexel.ViewModels.Document.Nodes.Brushes;

[NodeViewModel("BRUSH_OUTPUT_NODE", "BRUSHES", ArcPerfectIcons.PaintBrush)]
internal class BrushOutputNodeViewModel : NodeViewModel<BrushOutputNode>
{
    public override void OnInitialized()
    {
        InputPropertyMap[BrushOutputNode.BrushNameProperty].SocketEnabled = false;
        InputPropertyMap[BrushOutputNode.FitToStrokeSizeProperty].SocketEnabled = false;
        InputPropertyMap[BrushOutputNode.UseCustomStampBlenderProperty].ValueChanged += OnValueChanged;
        InputPropertyMap[BrushOutputNode.ContentProperty].ConnectedOutputChanged += OnContentConnectionChanged;
        InputPropertyMap[BrushOutputNode.ContentTransformProperty].IsVisible = InputPropertyMap[BrushOutputNode.ContentProperty].ConnectedOutput != null;
        if(InputPropertyMap[BrushOutputNode.CustomStampBlenderCodeProperty] is StringPropertyViewModel codeProperty)
        {
            codeProperty.IsVisible = (bool)InputPropertyMap[BrushOutputNode.UseCustomStampBlenderProperty].Value;
            codeProperty.Kind = DrawingBackendApi.Current.ShaderImplementation.ShaderLanguageExtension;
        }
    }

    private void OnValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args)
    {
        InputPropertyMap[BrushOutputNode.CustomStampBlenderCodeProperty].IsVisible = (bool)args.NewValue;
        InputPropertyMap[BrushOutputNode.StampBlendModeProperty].IsVisible = !(bool)args.NewValue;
    }

    private void OnContentConnectionChanged(object? sender, EventArgs eventArgs)
    {
        var connection = InputPropertyMap[BrushOutputNode.ContentProperty].ConnectedOutput;
        InputPropertyMap[BrushOutputNode.ContentTransformProperty].IsVisible = connection != null;
    }
}
