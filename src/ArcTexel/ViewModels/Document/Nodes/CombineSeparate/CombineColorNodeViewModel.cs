using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.CombineSeparate;
using ArcTexel.Helpers.Extensions;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Nodes;
using ArcTexel.ViewModels.Nodes.Properties;

namespace ArcTexel.ViewModels.Document.Nodes.CombineSeparate;

[NodeViewModel("COMBINE_COLOR_NODE", "COLOR", ArcPerfectIcons.ItemSlot)]
internal class CombineColorNodeViewModel() : CombineSeparateColorNodeViewModel<CombineColorNode>(true);
