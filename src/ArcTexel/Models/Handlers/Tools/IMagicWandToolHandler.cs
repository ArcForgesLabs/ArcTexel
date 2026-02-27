using ArcTexel.ChangeableDocument.Enums;
using ArcTexel.Models.Tools;

namespace ArcTexel.Models.Handlers.Tools;

internal interface IMagicWandToolHandler : IToolHandler
{
    public SelectionMode ResultingSelectionMode { get; }
    public DocumentScope DocumentScope { get; }
    public float Tolerance { get; }
}
