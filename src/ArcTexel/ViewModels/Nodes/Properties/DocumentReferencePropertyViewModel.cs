using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using ArcTexel.ChangeableDocument.Changeables;
using ArcTexel.ChangeableDocument.Changeables.Brushes;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;
using ArcTexel.Helpers;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.Document.Blackboard;
using ArcTexel.ViewModels.Tools.ToolSettings.Settings;

namespace ArcTexel.ViewModels.Nodes.Properties;

internal class DocumentReferencePropertyViewModel : NodePropertyViewModel<DocumentReference>
{
    private string? originalFilePath;
    public string? OriginalFilePath
    {
        get => originalFilePath;
        set => SetProperty(ref originalFilePath, value);
    }

    public ICommand PickGraphFileCommand { get; }

    public DocumentReferencePropertyViewModel(NodeViewModel node, Type valueType) : base(node, valueType)
    {
        PickGraphFileCommand = new AsyncRelayCommand(OnPickGraphFile);
        ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args)
    {
        if (args.NewValue is DocumentReference docRef)
        {
            OriginalFilePath = docRef.OriginalFilePath;
        }
        else
        {
            OriginalFilePath = null;
        }
    }

    private async Task OnPickGraphFile()
    {
        var any = new FileTypeDialogDataSet(FileTypeDialogDataSet.SetKind.Arc).GetFormattedTypes(false);

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var dialog = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions { FileTypeFilter = any.ToList() });

            if (dialog.Count == 0 || !Importer.IsSupportedFile(dialog[0].Path.LocalPath))
                return;

            var doc = Importer.ImportDocument(dialog[0].Path.LocalPath);
            doc.Operations.InvokeCustomAction(() =>
            {
                Value = new DocumentReference(doc.FullFilePath, doc.Id, doc.AccessInternalReadOnlyDocument());
                OriginalFilePath = doc.FullFilePath;
            });
        }
    }
}
