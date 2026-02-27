using ArcTexel.Extensions.CommonApi.Documents;
using ArcTexel.Extensions.Sdk.Bridge;

namespace ArcTexel.Extensions.Sdk.Api.Documents;

public class Document : IDocument
{
    public Guid Id => documentId;
    private Guid documentId;

    internal Document(Guid documentId)
    {
        this.documentId = documentId;
    }


    public void Resize(int width, int height)
    {
        Native.resize_document(documentId.ToString(), width, height);
    }
}
