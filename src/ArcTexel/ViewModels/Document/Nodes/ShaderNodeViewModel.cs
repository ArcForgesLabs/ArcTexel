using System.Collections.Specialized;
using Drawie.Backend.Core.Bridge;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Nodes.Properties;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("SHADER_NODE", "EFFECTS", ArcPerfectIcons.Terminal)]
internal class ShaderNodeViewModel : NodeViewModel<ShaderNode>
{
    public ShaderNodeViewModel()
    {
        Inputs.CollectionChanged += InputsOnCollectionChanged;
    }

    private void InputsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if(e.NewItems == null) return;

        foreach (var newItem in e.NewItems)
        {
            if (newItem is StringPropertyViewModel stringPropertyViewModel)
            {
                stringPropertyViewModel.Kind = DrawingBackendApi.Current.ShaderImplementation.ShaderLanguageExtension;
            }
        }
    }
}
