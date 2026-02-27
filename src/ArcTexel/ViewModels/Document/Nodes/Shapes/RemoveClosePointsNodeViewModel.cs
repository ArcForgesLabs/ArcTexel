using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Shapes;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.Shapes;

[NodeViewModel("REMOVE_CLOSE_POINTS", "SHAPE", ArcPerfectIcons.PointsCrossed)]
internal class RemoveClosePointsNodeViewModel : NodeViewModel<RemoveClosePointsNode>;
