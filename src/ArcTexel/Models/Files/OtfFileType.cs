using ArcTexel.Models.IO;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.Models.Files;

internal class OtfFileType : IoFileType
{
    public override string[] Extensions { get; } = new[] { ".otf" };
    public override string DisplayName => new LocalizedString("OPEN_TYPE_FONT");
    public override FileTypeDialogDataSet.SetKind SetKind { get; } = FileTypeDialogDataSet.SetKind.Vector;

    public override bool CanSave => false;

    public override Task<SaveResult> TrySaveAsync(string pathWithExtension, DocumentViewModel document,
        ExportConfig config, ExportJob? job)
    {
        throw new NotSupportedException("Saving OTF files is not supported.");
    }

    public override SaveResult TrySave(string pathWithExtension, DocumentViewModel document, ExportConfig config,
        ExportJob? job)
    {
        throw new NotSupportedException("Saving OTF files is not supported.");
    }
}
