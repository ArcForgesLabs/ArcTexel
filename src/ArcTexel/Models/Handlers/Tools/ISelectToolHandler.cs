using ArcTexel.ChangeableDocument.Enums;
using ArcTexel.Models.Position;

namespace ArcTexel.Models.Handlers.Tools;

internal interface ISelectToolHandler : IToolHandler
{
    public SelectionShape SelectShape { get; }
    public SelectionMode ResultingSelectionMode { get; }
}
