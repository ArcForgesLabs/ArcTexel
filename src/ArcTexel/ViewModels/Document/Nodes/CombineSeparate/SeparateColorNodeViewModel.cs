using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.CombineSeparate;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;

namespace ArcTexel.ViewModels.Document.Nodes.CombineSeparate;

[NodeViewModel("SEPARATE_COLOR_NODE", "COLOR", ArcPerfectIcons.ItemSlotOut)]
internal class SeparateColorNodeViewModel() : CombineSeparateColorNodeViewModel<SeparateColorNode>(false);
