using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.FilterNodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.FilterNodes;

[NodeViewModel("INVERT_FILTER_NODE", "FILTERS", ArcPerfectIcons.Invert)]
internal class InvertFilterNodeViewModel : NodeViewModel<InvertFilterNode>;
