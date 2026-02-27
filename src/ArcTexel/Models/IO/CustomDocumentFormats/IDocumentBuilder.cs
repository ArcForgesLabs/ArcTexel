using ArcTexel.Helpers;

namespace ArcTexel.Models.IO.CustomDocumentFormats;

internal interface IDocumentBuilder
{
    public void Build(DocumentViewModelBuilder builder, string path);
    public IReadOnlyCollection<string> Extensions { get; }
}
