using ArcTexel.ChangeableDocument.Enums;

namespace ArcTexel.Models.Handlers.Tools;

internal interface ILassoToolHandler : IToolHandler
{
    public SelectionMode? ResultingSelectionMode { get; }
}
