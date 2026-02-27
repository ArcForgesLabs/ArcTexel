using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;

namespace ArcTexel.ChangeableDocument.Changeables;

internal class DocumentGraphPipe : DocumentMemoryPipe<IReadOnlyNodeGraph>
{
    public DocumentGraphPipe(Document document) : base(document)
    {
    }

    protected override IReadOnlyNodeGraph? GetData()
    {
        return Document.NodeGraph;
    }
}
