using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using ArcTexel.Helpers.Extensions;
using ArcTexel.Helpers;
using ArcTexel.Models.Clipboard;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Commands.Attributes.Evaluators;
using ArcTexel.Models.Commands.Search;
using ArcTexel.Models.Controllers;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using Drawie.Numerics;
using ArcTexel.Models.Commands;
using ArcTexel.ViewModels.Dock;
using ArcTexel.ViewModels.Document;
using ArcTexel.Views;

namespace ArcTexel.ViewModels.SubViewModels;
#nullable enable
[Command.Group("ArcTexel.Clipboard", "CLIPBOARD")]
internal class ClipboardViewModel : SubViewModel<ViewModelMain>
{
    private ConcurrentDictionary<string, Task> clipboardTasks = new();
    private bool canPasteImage;
    private string lastTextInClipboard;
    private bool areNodesInClipboard;
    private bool areCelsInClipboard;

    public ClipboardViewModel(ViewModelMain owner)
        : base(owner)
    {
        owner.AttachedToWindow += AttachClipboard;
    }

    private void AttachClipboard(MainWindow window)
    {
        ClipboardController.Initialize(new ArcTexelClipboard(window.Clipboard));
        window.GotFocus += (sender, args) =>
        {
            QueueCheckCanPasteImage();
            QueueFetchTextFromClipboard();
            QueueCheckNodesInClipboard();
            QueueCheckCelsInClipboard();
        };
    }

    [Command.Basic("ArcTexel.Clipboard.Cut", "CUT", "CUT_DESCRIPTIVE",
        Key = Key.X, Modifiers = KeyModifiers.Control,
        MenuItemPath = "EDIT/CUT", MenuItemOrder = 2, Icon = ArcPerfectIcons.Scissors, AnalyticsTrack = true)]
    public async Task Cut()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        var transformActive = doc.TransformViewModel.TransformActive;
        RectD? lastTransformRect = transformActive
            ? doc.TransformViewModel.Corners.AABBBounds
            : null;

        doc.Operations.TryStopActiveExecutor();
        await Copy(lastTransformRect);
        doc.Operations.DeleteSelectedPixels(doc.AnimationDataViewModel.ActiveFrameBindable, true, lastTransformRect);
    }

    
    [Command.Basic("ArcTexel.Clipboard.PasteAsNewLayer", true, "PASTE_AS_NEW_LAYER", "PASTE_AS_NEW_LAYER_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanPaste", Key = Key.V, Modifiers = KeyModifiers.Control,
        ShortcutContexts = [typeof(ViewportWindowViewModel), typeof(LayersDockViewModel)],
        Icon = ArcPerfectIcons.PasteAsNewLayer, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Clipboard.Paste", false, "PASTE", "PASTE_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanPaste", Key = Key.V, Modifiers = KeyModifiers.Shift,
        MenuItemPath = "EDIT/PASTE", MenuItemOrder = 4, Icon = ArcPerfectIcons.Paste, AnalyticsTrack = true)]
    public void Paste(bool pasteAsNewLayer)
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is null)
            return;

        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await ClipboardController.TryPasteFromClipboard(doc, Owner.DocumentManagerSubViewModel, pasteAsNewLayer);
        });
    }

    [Command.Basic("ArcTexel.Clipboard.PasteReferenceLayer", "PASTE_REFERENCE_LAYER",
        "PASTE_REFERENCE_LAYER_DESCRIPTIVE", CanExecute = "ArcTexel.Clipboard.CanPaste",
        Icon = ArcPerfectIcons.PasteReferenceLayer, AnalyticsTrack = true)]
    public void PasteReferenceLayer(DataTransfer data)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            DataImage imageData =
                (data == null
                    ? await ClipboardController.GetImagesFromClipboard()
                    : ClipboardController.GetImage(new[] { new ImportedObject(data) }).Result).First();
            using var surface = imageData.Image;

            var bitmap = imageData.Image.ToWriteableBitmap();

            byte[] pixels = bitmap.ExtractPixels();

            doc.Operations.ImportReferenceLayer(
                pixels.ToImmutableArray(),
                imageData.Image.Size);

            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow!.Activate();
            }
        });
    }

    [Command.Internal("ArcTexel.Clipboard.PasteReferenceLayerFromPath")]
    public void PasteReferenceLayer(string path)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;

        if (doc is null)
            return;

        // TODO: Exception handling would probably be good
        var bitmap = Importer.GetPreviewSurface(path);

        if (bitmap is null)
            return;

        byte[] pixels = bitmap.ToWriteableBitmap().ExtractPixels();

        if (pixels.Length == 0)
            return;

        doc.Operations.ImportReferenceLayer(
            pixels.ToImmutableArray(),
            new VecI(bitmap.Size.X, bitmap.Size.Y));
    }

    [Command.Basic("ArcTexel.Clipboard.PasteColor", false, "PASTE_COLOR", "PASTE_COLOR_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanPasteColor", IconEvaluator = "ArcTexel.Clipboard.PasteColorIcon",
        AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Clipboard.PasteColorAsSecondary", true, "PASTE_COLOR_SECONDARY",
        "PASTE_COLOR_SECONDARY_DESCRIPTIVE", CanExecute = "ArcTexel.Clipboard.CanPasteColor",
        IconEvaluator = "ArcTexel.Clipboard.PasteColorIcon", AnalyticsTrack = true)]
    public void PasteColor(bool secondary)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (!ColorHelper.ParseAnyFormat(
                    (await ClipboardController.Clipboard.GetTextAsync())?.Trim() ?? string.Empty,
                    out var result))
            {
                return;
            }

            if (!secondary)
            {
                Owner.ColorsSubViewModel.PrimaryColor = result.Value;
            }
            else
            {
                Owner.ColorsSubViewModel.SecondaryColor = result.Value;
            }
        });
    }

    [Command.Basic("ArcTexel.Clipboard.PasteNodes", "PASTE_NODES", "PASTE_NODES_DESCRIPTIVE",
        ShortcutContexts = [typeof(NodeGraphDockViewModel)], Key = Key.V, Modifiers = KeyModifiers.Control,
        CanExecute = "ArcTexel.Clipboard.CanPasteNodes", Icon = ArcPerfectIcons.Paste, AnalyticsTrack = true)]
    public void PasteNodes()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            Guid documentId = await ClipboardController.GetDocumentId();
            Guid[] toDuplicate = await ClipboardController.GetNodeIds();

            List<Guid> newIds = new();

            Dictionary<Guid, Guid> nodeMapping = new();

            using var block = doc.Operations.StartChangeBlock();

            DocumentViewModel targetDocument = Owner.DocumentManagerSubViewModel.ActiveDocument;

            if (documentId != Owner.DocumentManagerSubViewModel.ActiveDocument.Id)
            {
                targetDocument = Owner.DocumentManagerSubViewModel.Documents.FirstOrDefault(x => x.Id == documentId);
                if (targetDocument == null)
                {
                    return;
                }
            }

            foreach (var nodeId in toDuplicate)
            {
                Guid? newId = doc.Operations.ImportNode(nodeId, targetDocument);
                if (newId != null)
                {
                    newIds.Add(newId.Value);
                    nodeMapping.Add(nodeId, newId.Value);
                }
            }

            if (newIds.Count == 0)
                return;

            block.ExecuteQueuedActions();

            ConnectRelatedNodes(targetDocument, doc, nodeMapping);

            doc.Operations.InvokeCustomAction(() =>
            {
                foreach (var node in doc.NodeGraph.AllNodes)
                {
                    node.IsNodeSelected = false;
                }

                foreach (var node in newIds)
                {
                    var nodeInstance = doc.NodeGraph.AllNodes.FirstOrDefault(x => x.Id == node);
                    if (nodeInstance != null)
                    {
                        nodeInstance.IsNodeSelected = true;
                    }
                }
            });
        });
    }

    [Command.Basic("ArcTexel.Clipboard.PasteCels", "PASTE_CELS", "PASTE_CELS_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanPasteCels", Key = Key.V, Modifiers = KeyModifiers.Control,
        ShortcutContexts = [typeof(TimelineDockViewModel)], Icon = ArcPerfectIcons.Paste, AnalyticsTrack = true)]
    public void PasteCels()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var cels = await ClipboardController.GetCelIds();

            if (cels.Length == 0)
                return;

            using var block = doc.Operations.StartChangeBlock();

            List<Guid> newCels = new();
            List<ICelHandler> celsToSelect = new();

            int minStartFrame = int.MaxValue;

            foreach (var cel in cels)
            {
                var foundCel = doc.AnimationDataViewModel.AllCels.FirstOrDefault(x => x.Id == cel);
                if (foundCel == null)
                    continue;

                celsToSelect.Add(foundCel);
                minStartFrame = Math.Min(minStartFrame, foundCel.StartFrameBindable);
            }

            int delta = doc.AnimationDataViewModel.ActiveFrameBindable - minStartFrame;

            foreach (var cel in celsToSelect)
            {
                int celFrame = cel.StartFrameBindable + delta;
                Guid? newCel = doc.AnimationDataViewModel.CreateCel(cel.LayerGuid,
                    celFrame, cel.LayerGuid,
                    cel.StartFrameBindable);
                if (newCel != null)
                {
                    int duration = cel.DurationBindable;
                    doc.Operations.ChangeCelLength(newCel.Value, celFrame, duration);
                    newCels.Add(newCel.Value);
                }
            }

            doc.Operations.InvokeCustomAction(() =>
            {
                foreach (var cel in doc.AnimationDataViewModel.AllCels)
                {
                    cel.IsSelected = false;
                }

                foreach (var cel in newCels)
                {
                    var celInstance = doc.AnimationDataViewModel.AllCels.FirstOrDefault(x => x.Id == cel);
                    if (celInstance != null)
                    {
                        celInstance.IsSelected = true;
                    }
                }
            });
        });
    }

    [Command.Basic("ArcTexel.Clipboard.Copy", "COPY", "COPY_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanCopy",
        Key = Key.C, Modifiers = KeyModifiers.Control,
        ShortcutContexts = [typeof(ViewportWindowViewModel), typeof(LayersDockViewModel)],
        MenuItemPath = "EDIT/COPY", MenuItemOrder = 3, Icon = ArcPerfectIcons.Copy, AnalyticsTrack = true)]
    public async Task Copy()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        await ClipboardController.CopyToClipboard(doc, null);

        SetHasImageInClipboard();
    }

    private async Task Copy(RectD? lastTransformRect)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        await ClipboardController.CopyToClipboard(doc, lastTransformRect);

        SetHasImageInClipboard();
    }

    [Command.Basic("ArcTexel.Clipboard.CopyVisible", "COPY_VISIBLE", "COPY_VISIBLE_DESCRIPTIVE",
        CanExecute = "ArcTexel.Clipboard.CanCopy",
        Key = Key.C, Modifiers = KeyModifiers.Shift,
        MenuItemPath = "EDIT/COPY_VISIBLE", MenuItemOrder = 3, Icon = ArcPerfectIcons.Copy, AnalyticsTrack = true)]
    public async Task CopyVisible()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        await ClipboardController.CopyVisibleToClipboard(
            doc, Owner.WindowSubViewModel.ActiveWindow is ViewportWindowViewModel viewportWindowViewModel
                ? viewportWindowViewModel.RenderOutputName
                : null);

        SetHasImageInClipboard();
    }

    [Command.Basic("ArcTexel.Clipboard.CopyNodes", "COPY_NODES", "COPY_NODES_DESCRIPTIVE",
        Key = Key.C, Modifiers = KeyModifiers.Control,
        ShortcutContexts = [typeof(NodeGraphDockViewModel)],
        CanExecute = "ArcTexel.Clipboard.CanCopyNodes",
        Icon = ArcPerfectIcons.Copy, AnalyticsTrack = true)]
    public async Task CopySelectedNodes()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        var selectedNodes = doc.NodeGraph.AllNodes.Where(x => x.IsNodeSelected).Select(x => x.Id).ToArray();
        if (selectedNodes.Length == 0)
            return;

        await ClipboardController.CopyNodes(selectedNodes, doc.Id);

        areNodesInClipboard = true;
        ClearHasImageInClipboard();
    }

    [Command.Basic("ArcTexel.Clipboard.CopyCels", "COPY_CELS",
        "COPY_CELS_DESCRIPTIVE", CanExecute = "ArcTexel.Clipboard.CanCopyCels",
        ShortcutContexts = [typeof(TimelineDockViewModel)],
        Key = Key.C, Modifiers = KeyModifiers.Control, Icon = ArcPerfectIcons.Copy, AnalyticsTrack = true)]
    public async Task CopySelectedCels()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        var selectedCels = doc.AnimationDataViewModel.AllCels.Where(x => x.IsSelected).Select(x => x.Id).ToArray();
        if (selectedCels.Length == 0)
            return;

        await ClipboardController.CopyCels(selectedCels, doc.Id);

        areCelsInClipboard = true;
        ClearHasImageInClipboard();
    }


    [Command.Basic("ArcTexel.Clipboard.CopyPrimaryColorAsHex", CopyColor.PrimaryHEX, "COPY_COLOR_HEX",
        "COPY_COLOR_HEX_DESCRIPTIVE", IconEvaluator = "ArcTexel.Clipboard.CopyColorIcon", AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Clipboard.CopyPrimaryColorAsRgb", CopyColor.PrimaryRGB, "COPY_COLOR_RGB",
        "COPY_COLOR_RGB_DESCRIPTIVE", IconEvaluator = "ArcTexel.Clipboard.CopyColorIcon", AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Clipboard.CopySecondaryColorAsHex", CopyColor.SecondaryHEX,
        "COPY_COLOR_SECONDARY_HEX",
        "COPY_COLOR_SECONDARY_HEX_DESCRIPTIVE", IconEvaluator = "ArcTexel.Clipboard.CopyColorIcon",
        AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Clipboard.CopySecondaryColorAsRgb", CopyColor.SecondardRGB,
        "COPY_COLOR_SECONDARY_RGB",
        "COPY_COLOR_SECONDARY_RGB_DESCRIPTIVE", IconEvaluator = "ArcTexel.Clipboard.CopyColorIcon",
        AnalyticsTrack = true)]
    [Command.Filter("ArcTexel.Clipboard.CopyColorToClipboard", "COPY_COLOR_TO_CLIPBOARD", "COPY_COLOR",
        Key = Key.C,
        Modifiers = KeyModifiers.Shift | KeyModifiers.Alt, AnalyticsTrack = true)]
    public async Task CopyColorAsHex(CopyColor color)
    {
        var targetColor = color switch
        {
            CopyColor.PrimaryHEX or CopyColor.PrimaryRGB => Owner.ColorsSubViewModel.PrimaryColor,
            _ => Owner.ColorsSubViewModel.SecondaryColor
        };

        string text = color switch
        {
            CopyColor.PrimaryHEX or CopyColor.SecondaryHEX => targetColor.A == 255
                ? $"#{targetColor.R:X2}{targetColor.G:X2}{targetColor.B:X2}"
                : targetColor.ToString(),
            _ => targetColor.A == 255
                ? $"rgb({targetColor.R},{targetColor.G},{targetColor.B})"
                : $"rgba({targetColor.R},{targetColor.G},{targetColor.B},{targetColor.A})",
        };

        await ClipboardController.Clipboard.SetTextAsync(text);
    }

    [Evaluator.CanExecute("ArcTexel.Clipboard.CanPaste")]
    public bool CanPaste(object parameter)
    {
        if (!Owner.DocumentIsNotNull(null)) return false;

        if (parameter is DataTransfer data)
            return ClipboardController.IsImage(data);

        QueueCheckCanPasteImage();
        return canPasteImage;
    }


    [Evaluator.CanExecute("ArcTexel.Clipboard.CanCopyCels")]
    public bool CanCopyCels()
    {
        return Owner.DocumentIsNotNull(null) &&
               Owner.DocumentManagerSubViewModel.ActiveDocument.AnimationDataViewModel.AllCels.Any(x => x.IsSelected);
    }

    [Evaluator.CanExecute("ArcTexel.Clipboard.CanCopyNodes",
        nameof(Owner.DocumentManagerSubViewModel),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument.NodeGraph))]
    public bool CanCopyNodes()
    {
        return Owner.DocumentIsNotNull(null) &&
               Owner.DocumentManagerSubViewModel.ActiveDocument.NodeGraph.AllNodes.Any(x => x.IsNodeSelected);
    }


    [Evaluator.CanExecute("ArcTexel.Clipboard.CanPasteNodes",
        nameof(Owner.DocumentManagerSubViewModel),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument.NodeGraph))]
    public bool CanPasteNodes()
    {
        if (!Owner.DocumentIsNotNull(null)) return false;

        QueueCheckNodesInClipboard();
        return areNodesInClipboard;
    }

    [Evaluator.CanExecute("ArcTexel.Clipboard.CanPasteCels",
        nameof(Owner.DocumentManagerSubViewModel),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument.AnimationDataViewModel))]
    public bool CanPasteCels()
    {
        if (!Owner.DocumentIsNotNull(null)) return false;

        QueueCheckCelsInClipboard();
        return areCelsInClipboard;
    }

    [Evaluator.CanExecute("ArcTexel.Clipboard.CanPasteColor")]
    public bool CanPasteColor()
    {
        QueueFetchTextFromClipboard();
        return ColorHelper.ParseAnyFormat(lastTextInClipboard?.Trim() ?? string.Empty, out _);
    }

    [Evaluator.CanExecute("ArcTexel.Clipboard.CanCopy",
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument.TransformViewModel.TransformActive),
        nameof(Owner.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember))]
    public bool CanCopy()
    {
        return Owner.DocumentManagerSubViewModel.ActiveDocument != null &&
               (Owner.SelectionSubViewModel.SelectionIsNotEmpty() ||
                Owner.DocumentManagerSubViewModel.ActiveDocument.TransformViewModel.TransformActive
                || Owner.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember != null);
    }

    [Evaluator.Icon("ArcTexel.Clipboard.PasteColorIcon")]
    public IImage GetPasteColorIcon()
    {
        Color color;

        QueueFetchTextFromClipboard();
        color = ColorHelper.ParseAnyFormat(lastTextInClipboard?.Trim() ?? string.Empty,
            out var result)
            ? result.Value.ToOpaqueMediaColor()
            : Colors.Transparent;
        return ColorSearchResult.GetIcon(color.ToOpaqueColor());
    }

    [Evaluator.Icon("ArcTexel.Clipboard.CopyColorIcon")]
    public IImage GetCopyColorIcon(object data)
    {
        if (data is CopyColor color)
        {
        }
        else if (data is Models.Commands.Commands.Command.BasicCommand command)
        {
            color = (CopyColor)command.Parameter;
        }
        else if (data is Models.Commands.Search.CommandSearchResult result)
        {
            color = (CopyColor)((Models.Commands.Commands.Command.BasicCommand)result.Command).Parameter;
        }
        else
        {
            throw new ArgumentException("data must be of type CopyColor, BasicCommand or CommandSearchResult");
        }

        var targetColor = color switch
        {
            CopyColor.PrimaryHEX or CopyColor.PrimaryRGB => Owner.ColorsSubViewModel.PrimaryColor,
            _ => Owner.ColorsSubViewModel.SecondaryColor
        };

        return ColorSearchResult.GetIcon(targetColor.ToOpaqueMediaColor().ToOpaqueColor());
    }

    private void ConnectRelatedNodes(IDocument sourceDoc, DocumentViewModel targetDoc,
        Dictionary<Guid, Guid> nodeMapping)
    {
        foreach (var connection in sourceDoc.NodeGraphHandler.Connections)
        {
            if (nodeMapping.TryGetValue(connection.InputNode.Id, out var inputNode) &&
                nodeMapping.TryGetValue(connection.OutputNode.Id, out var outputNode))
            {
                var inputNodeInstance = targetDoc.NodeGraph.AllNodes.FirstOrDefault(x => x.Id == inputNode);
                var outputNodeInstance = targetDoc.NodeGraph.AllNodes.FirstOrDefault(x => x.Id == outputNode);

                if (inputNodeInstance == null || outputNodeInstance == null)
                    continue;

                var inputProperty =
                    inputNodeInstance.Inputs.FirstOrDefault(x =>
                        x.PropertyName == connection.InputProperty.PropertyName);
                var outputProperty =
                    outputNodeInstance.Outputs.FirstOrDefault(x =>
                        x.PropertyName == connection.OutputProperty.PropertyName);

                if (inputProperty == null || outputProperty == null)
                    continue;

                targetDoc.NodeGraph.ConnectProperties(inputProperty, outputProperty);
            }
        }
    }

    private void QueueCheckCanPasteImage()
    {
        QueueClipboardTask("CheckCanPasteImage", ClipboardController.IsImageInClipboard, canPasteImage,
            x =>
            {
                canPasteImage = x;
                CommandController.CanExecuteChanged("ArcTexel.Clipboard.CanPaste");
            });
    }

    private void QueueFetchTextFromClipboard()
    {
        QueueClipboardTask("FetchTextFromClipboard", ClipboardController.GetTextFromClipboard, lastTextInClipboard,
            x =>
            {
                lastTextInClipboard = x;
                CommandController.CanExecuteChanged("ArcTexel.Clipboard.CanPasteColor");
            });
    }

    private void QueueCheckNodesInClipboard()
    {
        QueueClipboardTask("CheckNodesInClipboard", ClipboardController.AreNodesInClipboard, areNodesInClipboard,
            x =>
            {
                areNodesInClipboard = x;
                CommandController.CanExecuteChanged("ArcTexel.Clipboard.CanPasteNodes");
            });
    }

    private void QueueCheckCelsInClipboard()
    {
        QueueClipboardTask("CheckCelsInClipboard", ClipboardController.AreCelsInClipboard, areCelsInClipboard,
            x =>
            {
                areCelsInClipboard = x;
                CommandController.CanExecuteChanged("ArcTexel.Clipboard.CanPasteCels");
            });
    }

    private void SetHasImageInClipboard()
    {
        canPasteImage = true;
        areNodesInClipboard = false;
        areCelsInClipboard = false;
        lastTextInClipboard = string.Empty;
    }

    private void ClearHasImageInClipboard()
    {
        canPasteImage = false;
        lastTextInClipboard = string.Empty;
    }

    private void QueueClipboardTask<T>(string key, Func<Task<T>> task, T value, Action<T> updateAction)
    {
        if (clipboardTasks.TryGetValue(key, out var t))
        {
            if (t.IsFaulted)
            {
                clipboardTasks.Remove(key, out _);
            }

            return;
        }

        var newTask = Dispatcher.UIThread.InvokeAsync(async () =>
        {
            T result = await task();
            if (!EqualityComparer<T>.Default.Equals(result, value))
            {
                updateAction(result);
                CommandController.CanExecuteChanged("ArcTexel.Clipboard");
            }

            clipboardTasks.Remove(key, out _);
        });

        clipboardTasks.TryAdd(key, newTask);
    }

    public enum CopyColor
    {
        PrimaryHEX,
        PrimaryRGB,
        SecondaryHEX,
        SecondardRGB
    }
}
