using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Matrix;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.Matrix;

[NodeViewModel("TRANSFORM_NODE", "MATRIX", ArcPerfectIcons.CanvasResize)]
internal class TransformNodeViewModel : NodeViewModel<TransformNode>;
