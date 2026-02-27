using ArcTexel.Extensions.CommonApi.Documents;

namespace ArcTexel.Extensions.CommonApi.IO;

public interface IDocumentProvider
{
   public IDocument? ActiveDocument { get; }
   public IDocument? ImportFile(string path, bool associatePath = true);
   public IDocument? ImportDocument(byte[] data);

   public IDocument? GetDocument(Guid id);
}
