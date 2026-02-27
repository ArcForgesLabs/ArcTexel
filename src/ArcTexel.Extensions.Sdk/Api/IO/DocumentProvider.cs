using ArcTexel.Extensions.CommonApi.Documents;
using ArcTexel.Extensions.CommonApi.IO;
using ArcTexel.Extensions.Sdk.Api.Documents;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.IO;

public class DocumentProvider : IDocumentProvider
{
    public IDocument ActiveDocument => Interop.GetActiveDocument();

    public IDocument? ImportFile(string path, bool associatePath = true)
    {
        return Interop.ImportFile(path, associatePath);
    }

    public IDocument? ImportDocument(byte[] data)
    {
        return Interop.ImportDocument(data);
    }

    public IDocument? GetDocument(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid document ID");

        return new Document(id);
    }
}
