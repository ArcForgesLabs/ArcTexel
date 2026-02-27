using ArcTexel.ChangeableDocument.Changeables.Interfaces;

namespace ArcTexel.ChangeableDocument.Changeables.Brushes;

public interface IBrush
{
    public string? FilePath { get; }
    public Guid OutputNodeId { get; }
    IReadOnlyDocument Document { get; }
}
