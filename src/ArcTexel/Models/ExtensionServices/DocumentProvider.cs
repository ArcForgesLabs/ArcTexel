using Avalonia.Threading;
using ArcTexel.Extensions.CommonApi.Documents;
using ArcTexel.Extensions.CommonApi.IO;
using ArcTexel.Models.IO;
using ArcTexel.ViewModels;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.Models.ExtensionServices;

internal class DocumentProvider : IDocumentProvider
{
    public IDocument? ActiveDocument => fileViewModel.Owner.DocumentManagerSubViewModel.ActiveDocument;
    private FileViewModel fileViewModel;

    public DocumentProvider(FileViewModel fileViewModel)
    {
        this.fileViewModel = fileViewModel;
    }

    public IDocument ImportFile(string path, bool associatePath = true)
    {
        return fileViewModel.OpenFromPath(path, associatePath);
    }

    public IDocument? ImportDocument(byte[] data)
    {
        return fileViewModel.OpenFromArcBytes(data);
    }

    public IDocument? GetDocument(Guid id)
    {
        var document = fileViewModel.Owner.DocumentManagerSubViewModel.Documents.FirstOrDefault(x => x.Id == id);
        return document;
    }
}
