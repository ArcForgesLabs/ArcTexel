using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Handlers;
using Drawie.Numerics;

namespace ArcTexel.Models.DocumentModels;

internal class DocumentSizeChangedEventArgs
{
    public DocumentSizeChangedEventArgs(IDocument document, VecI oldSize, VecI newSize)
    {
        Document = document;
        OldSize = oldSize;
        NewSize = newSize;
    }

    public VecI OldSize { get; }
    public VecI NewSize { get; }
    public IDocument Document { get; }
}
