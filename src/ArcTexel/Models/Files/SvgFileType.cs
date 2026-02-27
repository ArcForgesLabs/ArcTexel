using Avalonia.Media;
using ArcTexel.ChangeableDocument.Rendering;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using Drawie.Numerics;
using ArcTexel.SVG;
using ArcTexel.SVG.Elements;
using ArcTexel.SVG.Features;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.Document.Nodes;

namespace ArcTexel.Models.Files;

internal class SvgFileType : IoFileType
{
    public override string[] Extensions { get; } = new[] { ".svg" };
    public override string DisplayName => new LocalizedString("SVG_FILE");
    public override FileTypeDialogDataSet.SetKind SetKind { get; } = FileTypeDialogDataSet.SetKind.Vector;
    public override SolidColorBrush EditorColor { get; } = new SolidColorBrush(Color.FromRgb(0, 128, 0));

    public override async Task<SaveResult> TrySaveAsync(string pathWithExtension, DocumentViewModel document,
        ExportConfig config, ExportJob? job)
    {
        job?.Report(0, string.Empty);
        SvgDocument svgDocument = document.ToSvgDocument(0, config.ExportSize, config.VectorExportConfig);

        job?.Report(0.5, string.Empty);
        string xml = svgDocument.ToXml();

        xml = $"<!-- Created with ArcTexel (https://arctexel.net) -->{Environment.NewLine}" + xml;

        job?.Report(0.75, string.Empty);
        await using FileStream fileStream = new(pathWithExtension, FileMode.Create);
        await using StreamWriter writer = new(fileStream);
        await writer.WriteAsync(xml);

        job?.Report(1, string.Empty);
        return new SaveResult(SaveResultType.Success);
    }

    public override SaveResult TrySave(string pathWithExtension, DocumentViewModel document, ExportConfig config,
        ExportJob? job)
    {
        job?.Report(0, string.Empty);
        SvgDocument svgDocument = document.ToSvgDocument(0, config.ExportSize, config.VectorExportConfig);

        job?.Report(0.5, string.Empty);
        string xml = svgDocument.ToXml();

        xml = $"<!-- Created with ArcTexel (https://arctexel.net) -->{Environment.NewLine}" + xml;

        job?.Report(0.75, string.Empty);
        using FileStream fileStream = new(pathWithExtension, FileMode.Create);
        using StreamWriter writer = new(fileStream);
        writer.Write(xml);

        job?.Report(1, string.Empty);
        return new SaveResult(SaveResultType.Success);
    }
}
