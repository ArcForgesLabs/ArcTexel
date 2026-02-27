using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes;

[NodeViewModel("MERGE_NODE", "IMAGE", ArcPerfectIcons.DuplicateImage)]
internal class MergeNodeViewModel : NodeViewModel<MergeNode>;
