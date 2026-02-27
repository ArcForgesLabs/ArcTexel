using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("MODIFY_IMAGE_RIGHT_NODE", "IMAGE", null)]
internal class ModifyImageRightNodeViewModel : NodeViewModel<ModifyImageRightNode>, IPairNodeEndViewModel;
