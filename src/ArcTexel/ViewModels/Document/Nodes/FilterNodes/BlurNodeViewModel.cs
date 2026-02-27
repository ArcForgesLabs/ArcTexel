using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.FilterNodes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.FilterNodes;

[NodeViewModel("BLUR_FILTER_NODE", "FILTERS", ArcPerfectIcons.DropletFilled)]
internal class BlurNodeViewModel : NodeViewModel<BlurNode>;
