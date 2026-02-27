using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("MODIFY_IMAGE_LEFT_NODE", "IMAGE", ArcPerfectIcons.PutImage)]
internal class ModifyImageLeftNodeViewModel : NodeViewModel<ModifyImageLeftNode>, IPairNodeStartViewModel;
