using Avalonia.Media;
using ArcTexel.Models.IO;
using Drawie.Numerics;
using ArcTexel.Helpers;
using ArcTexel.Models.ExceptionHandling;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.Models.Files;

internal class ArcFileType : IoFileType
{
    public static ArcFileType ArcFile { get; } = new ArcFileType();
    public override string DisplayName => new LocalizedString("PIXI_FILE");
    public override string[] Extensions => new[] { ".arc" };

    public override SolidColorBrush EditorColor { get; } = new SolidColorBrush(new Color(255, 226, 1, 45));

    public override FileTypeDialogDataSet.SetKind SetKind { get; } = FileTypeDialogDataSet.SetKind.Arc;

    public override async Task<SaveResult> TrySaveAsync(string pathWithExtension, DocumentViewModel document,
        ExportConfig config, ExportJob? job)
    {
        try
        {
            job?.Report(0, "Serializing document");
            await Parser.ArcParser.V5.SerializeAsync(document.ToSerializable(), pathWithExtension);
            job?.Report(1, "Document serialized");
        }
        catch (UnauthorizedAccessException e)
        {
            return new SaveResult(SaveResultType.SecurityError);
        }
        catch (IOException)
        {
            return new SaveResult(SaveResultType.IoError);
        }
        catch (Exception e)
        {
            CrashHelper.SendExceptionInfo(e);
            Console.WriteLine("Failed to save document: " + e.Message + Environment.NewLine + e.StackTrace);
            return new SaveResult(SaveResultType.UnknownError);
        }

        return new SaveResult(SaveResultType.Success);
    }

    public override SaveResult TrySave(string pathWithExtension, DocumentViewModel document, ExportConfig config,
        ExportJob? job)
    {
        try
        {
            job?.Report(0, "Serializing document");
            Parser.ArcParser.V5.Serialize(document.ToSerializable(), pathWithExtension);
            job?.Report(1, "Document serialized");
        }
        catch (UnauthorizedAccessException e)
        {
            return new SaveResult(SaveResultType.SecurityError);
        }
        catch (IOException)
        {
            return new SaveResult(SaveResultType.IoError);
        }
        catch (Exception e)
        {
            CrashHelper.SendExceptionInfo(e);
            return new SaveResult(SaveResultType.UnknownError);
        }

        return new SaveResult(SaveResultType.Success);
    }
}
