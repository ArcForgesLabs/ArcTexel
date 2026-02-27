using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Drawie.Backend.Core;
using ArcTexel.ChangeableDocument;
using ArcTexel.Helpers.Converters;
using ArcTexel.Helpers.Extensions;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Surfaces;
using ArcTexel.Extensions.Exceptions;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Commands.Attributes.Evaluators;
using ArcTexel.Models.Dialogs;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using ArcTexel.Models.Layers;
using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Changeables.Interfaces;
using ArcTexel.Helpers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.UI.Common.Localization;
using ArcTexel.ViewModels.Dock;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.Document.Nodes;
using ArcTexel.Views.Overlays.TextOverlay;

namespace ArcTexel.ViewModels.SubViewModels;
#nullable enable
[Command.Group("ArcTexel.Layer", "LAYER")]
internal class LayersViewModel : SubViewModel<ViewModelMain>
{
    public LayersViewModel(ViewModelMain owner)
        : base(owner)
    {
    }

    [Evaluator.CanExecute("ArcTexel.Layer.CanDeleteSelected",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectedStructureMember))]
    public bool CanDeleteSelected()
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        if (member is null)
            return false;
        return true;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.HasSelectedMembers",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectedStructureMember),
        nameof(DocumentManagerViewModel.ActiveDocument.SoftSelectedStructureMembers))]
    public bool HasSelectedMembers()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;
        return doc.SelectedStructureMember is not null || doc.SoftSelectedStructureMembers.Count > 0;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.HasMultipleSelectedMembers",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectedStructureMember),
        nameof(DocumentManagerViewModel.ActiveDocument.SoftSelectedStructureMembers))]
    public bool HasMultipleSelectedMembers()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;
        int count = doc.SoftSelectedStructureMembers.Count;
        if (doc.SelectedStructureMember is not null)
            count++;
        return count > 1;
    }

    private List<Guid> GetSelected()
    {
        List<Guid> members = new();
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return members;
        if (doc.SelectedStructureMember is not null)
            members.Add(doc.SelectedStructureMember.Id);
        members.AddRange(doc.SoftSelectedStructureMembers.Select(static member => member.Id));
        return members;
    }

    [Command.Basic("ArcTexel.Layer.DeleteAllSelected", "LAYER_DELETE_ALL_SELECTED",
        "LAYER_DELETE_ALL_SELECTED_DESCRIPTIVE", CanExecute = "ArcTexel.Layer.HasSelectedMembers",
        Icon = ArcPerfectIcons.Trash, AnalyticsTrack = true, Key = Key.Delete,
        ShortcutContexts = [typeof(LayersDockViewModel)])]
    public void DeleteAllSelected()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;
        var selected = doc.ExtractSelectedLayers(true).Concat(doc.SelectedMembers).Distinct().ToList();
        if (selected.Count > 0)
        {
            doc.Operations.DeleteStructureMembers(selected);
        }
    }

    [Command.Basic("ArcTexel.Layer.NewFolder", "NEW_FOLDER", "CREATE_NEW_FOLDER",
        CanExecute = "ArcTexel.Layer.CanCreateNewMember",
        Icon = ArcPerfectIcons.FolderPlus, AnalyticsTrack = true)]
    public void NewFolder()
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is not { } doc)
            return;

        using var block = doc.Operations.StartChangeBlock();
        Guid? guid = doc.Operations.CreateStructureMember(StructureMemberType.Folder);
        if (doc.SoftSelectedStructureMembers.Count == 0)
            return;
        var selectedInOrder = doc.GetSelectedMembersInOrder();
        selectedInOrder.Reverse();
        block.ExecuteQueuedActions();

        if (guid is null)
            return;

        if (selectedInOrder.Count > 0)
        {
            Guid lastMovedMember = guid.Value;
            StructureMemberPlacement placement = StructureMemberPlacement.Inside;

            foreach (Guid memberGuid in selectedInOrder)
            {
                doc.Operations.MoveStructureMember(memberGuid, lastMovedMember, placement);
                lastMovedMember = memberGuid;
                if (placement == StructureMemberPlacement.Inside)
                {
                    placement = StructureMemberPlacement.Below;
                }

                block.ExecuteQueuedActions();
            }

            doc.Operations.ClearSoftSelectedMembers();
        }

        doc.Operations.SetSelectedMember(guid.Value);
    }

    [Command.Basic("ArcTexel.Layer.NewLayer", "NEW_LAYER", "CREATE_NEW_LAYER",
        CanExecute = "ArcTexel.Layer.CanCreateNewMember", Key = Key.N,
        Modifiers = KeyModifiers.Control | KeyModifiers.Shift,
        Icon = ArcPerfectIcons.FilePlus, AnalyticsTrack = true)]
    public void NewLayer()
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is not { } doc)
            return;

        doc.Operations.CreateStructureMember(StructureMemberType.ImageLayer);
    }

    public Guid? NewLayer(Type layerType, ActionSource source, string? name = null)
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is not { } doc)
            return null;

        return doc.Operations.CreateStructureMember(layerType, source, name);
    }

    public Guid? ForceNewLayer(Type layerType, ActionSource source, string? name = null)
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is not { } doc)
            return null;

        return doc.Operations.ForceCreateStructureMember(layerType, source, name);
    }

    [Evaluator.CanExecute("ArcTexel.Layer.CanCreateNewMember")]
    public bool CanCreateNewMember()
    {
        return Owner.DocumentManagerSubViewModel.ActiveDocument is { BlockingUpdateableChangeActive: false };
    }

    [Command.Internal("ArcTexel.Layer.ToggleLockTransparency", CanExecute = "ArcTexel.Layer.SelectedMemberIsTransparencyLockable",
        AnalyticsTrack = true)]
    public void ToggleLockTransparency()
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        if (member is not ImageLayerNodeViewModel layerVm)
            return;
        layerVm.LockTransparencyBindable = !layerVm.LockTransparencyBindable;
    }

    [Command.Internal("ArcTexel.Layer.SelectActiveMember", AnalyticsTrack = true)]
    public void SelectActiveMember(Guid memberGuid)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        var member = doc.StructureHelper.Find(memberGuid);
        if (member is null)
            return;

        doc.Operations.SetSelectedMember(member.Id);
    }

    [Command.Internal("ArcTexel.Layer.OpacitySliderDragStarted")]
    public void OpacitySliderDragStarted()
    {
        Owner.DocumentManagerSubViewModel.ActiveDocument?.Tools.UseOpacitySlider();
        Owner.DocumentManagerSubViewModel.ActiveDocument?.EventInlet.OnOpacitySliderDragStarted();
    }

    [Command.Internal("ArcTexel.Layer.OpacitySliderDragged")]
    public void OpacitySliderDragged(double value)
    {
        Owner.DocumentManagerSubViewModel.ActiveDocument?.EventInlet.OnOpacitySliderDragged((float)value);
    }

    [Command.Internal("ArcTexel.Layer.OpacitySliderDragEnded", AnalyticsTrack = true)]
    public void OpacitySliderDragEnded()
    {
        Owner.DocumentManagerSubViewModel.ActiveDocument?.EventInlet.OnOpacitySliderDragEnded();
    }

    [Command.Internal("ArcTexel.Layer.OpacitySliderSet", AnalyticsTrack = true)]
    public void OpacitySliderSet(double value)
    {
        var document = Owner.DocumentManagerSubViewModel.ActiveDocument;

        if (document?.SelectedStructureMember != null)
        {
            document.Operations.SetMemberOpacity(document.SelectedStructureMember.Id, (float)value);
        }
    }

    [Command.Basic("ArcTexel.Layer.DuplicateSelectedMember", "DUPLICATE_SELECTED_LAYER", "DUPLICATE_SELECTED_LAYER",
        Icon = ArcPerfectIcons.DuplicateFile, MenuItemPath = "EDIT/DUPLICATE", MenuItemOrder = 5,
        AnalyticsTrack = true)]
    public void DuplicateMember()
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember == null)
            return;

        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;

        member.Document.Operations.DuplicateMember(member.Id);
    }

    [Command.Basic("ArcTexel.Layer.UnlinkNestedDocument", "UNLINK", "UNLINK_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.SelectedMemberIsNestedDocument",
        Icon = ArcPerfectIcons.ChainBreak, AnalyticsTrack = true)]
    public void UnlinkNestedDocument()
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        if (member is not NestedDocumentNodeViewModel nestedDocVm)
            return;

        nestedDocVm.Document.Operations.UnlinkNestedDocument(nestedDocVm.Id);
    }

    [Command.Internal("ArcTexel.Layer.CreateNestedFromLayer")]
    public void CreateNestedFromLayer(Guid layerGuid)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        doc.Operations.CreateNestedDocumentFromMember(layerGuid);
    }

    [Evaluator.CanExecute("ArcTexel.Layer.SelectedMemberIsLayer",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedMemberIsLayer(object property)
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        return member is ILayerHandler;
    }


    [Evaluator.CanExecute("ArcTexel.Layer.SelectedMemberIsTransparencyLockable",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedMemberIsTransparencyLockable(object property)
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        return member is ITransparencyLockableMember;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.SelectedLayerIsRasterizable",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedLayerIsRasterizable(object property)
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        return member is ILayerHandler && member is not IRasterLayerHandler;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.SelectedMemberIsVectorLayer",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedMemberIsVectorLayer(object property)
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        return member is IVectorLayerHandler;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.AnySelectedMemberIsVectorLayer",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool AnySelectedMemberIsVectorLayer(object property)
    {
        var members = Owner.DocumentManagerSubViewModel.ActiveDocument?.ExtractSelectedLayers();

        if (members is null || members.Count == 0)
            return false;

        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;

        foreach (var member in members)
        {
            var handler = doc.StructureHelper.Find(member);
            if (handler is IVectorLayerHandler)
            {
                return true;
            }
        }

        return false;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.AnySelectedLayerIsRasterizable",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool AnySelectedMemberIsRasterizable(object property)
    {
        var members = Owner.DocumentManagerSubViewModel.ActiveDocument?.ExtractSelectedLayers();

        if (members is null || members.Count == 0)
            return false;

        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;

        foreach (var member in members)
        {
            var handler = doc.StructureHelper.Find(member);
            if (handler is ILayerHandler && handler is not IRasterLayerHandler)
            {
                return true;
            }
        }

        return false;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.SelectedMemberIsNestedDocument",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedMemberIsNestedDocument(object property)
    {
        var member = Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember;
        return member is NestedDocumentNodeViewModel;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.SelectedMemberIsSelectedText",
        nameof(DocumentManagerViewModel.ActiveDocument), nameof(DocumentViewModel.SelectedStructureMember))]
    public bool SelectedMemberIsSelectedText(object property)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;

        var member = doc?.SelectedStructureMember;
        return member is IVectorLayerHandler && doc.TextOverlayViewModel.IsActive &&
               doc.TextOverlayViewModel.CursorPosition != doc.TextOverlayViewModel.SelectionEnd;
    }

    private bool HasSelectedMember(bool above)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null)
            return false;
        if (above)
        {
            return doc.StructureHelper.GetAboveMember(member.Id, false) is not null;
        }

        return doc.StructureHelper.GetBelowMember(member.Id, false) is not null;
    }

    private void MoveSelectedMember(bool upwards)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null)
            return;
        var path = doc!.StructureHelper.FindPath(member.Id);
        if (path.Count < 2 || path[1] is not FolderNodeViewModel folderVm)
            return;
        var parent = folderVm;
        if (parent.Children.Count == 0)
            return;
        int curIndex = parent.Children.IndexOf(path[0]);
        if (upwards)
        {
            if (curIndex == parent.Children.Count - 1)
                return;
            doc.Operations.MoveStructureMember(member.Id, parent.Children[curIndex + 1].Id,
                StructureMemberPlacement.Above);
        }
        else
        {
            if (curIndex == 0)
                return;
            doc.Operations.MoveStructureMember(member.Id, parent.Children[curIndex - 1].Id,
                StructureMemberPlacement.Below);
        }
    }

    [Evaluator.CanExecute("ArcTexel.Layer.ActiveLayerHasMask",
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember.HasMaskBindable))]
    public bool ActiveMemberHasMask() =>
        Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember?.HasMaskBindable ?? false;

    [Evaluator.CanExecute("ArcTexel.Layer.ActiveLayerHasApplyableMask",
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember.HasMaskBindable))]
    public bool ActiveMemberHasApplyableMask() =>
        (Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember?.HasMaskBindable ?? false)
        && Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember is IRasterLayerHandler;

    [Evaluator.CanExecute("ArcTexel.Layer.ActiveLayerHasNoMask",
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.SelectedStructureMember.HasMaskBindable))]
    public bool ActiveLayerHasNoMask() =>
        !Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember?.HasMaskBindable ?? false;

    [Command.Basic("ArcTexel.Layer.CreateMask", "CREATE_MASK", "CREATE_MASK",
        CanExecute = "ArcTexel.Layer.ActiveLayerHasNoMask",
        Icon = ArcPerfectIcons.CreateMask, AnalyticsTrack = true)]
    public void CreateMask()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null || member.HasMaskBindable)
            return;
        doc!.Operations.CreateMask(member);
    }

    [Command.Basic("ArcTexel.Layer.DeleteMask", "DELETE_MASK", "DELETE_MASK",
        CanExecute = "ArcTexel.Layer.ActiveLayerHasMask", Icon = ArcPerfectIcons.Trash, AnalyticsTrack = true)]
    public void DeleteMask()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null || !member.HasMaskBindable)
            return;
        doc!.Operations.DeleteMask(member);
    }

    [Command.Basic("ArcTexel.Layer.ToggleMask", "TOGGLE_MASK", "TOGGLE_MASK",
        CanExecute = "ArcTexel.Layer.ActiveLayerHasMask",
        Icon = ArcPerfectIcons.MaskGhost, AnalyticsTrack = true)]
    public void ToggleMask()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null || !member.HasMaskBindable)
            return;

        member.MaskIsVisibleBindable = !member.MaskIsVisibleBindable;
    }

    [Command.Basic("ArcTexel.Layer.ApplyMask", "APPLY_MASK", "APPLY_MASK",
        CanExecute = "ArcTexel.Layer.ActiveLayerHasApplyableMask", AnalyticsTrack = true)]
    public void ApplyMask()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null || !member.HasMaskBindable)
            return;

        doc!.Operations.ApplyMask(member, doc.AnimationDataViewModel.ActiveFrameBindable);
    }

    [Command.Basic("ArcTexel.Layer.ToggleVisible", "TOGGLE_VISIBILITY", "TOGGLE_VISIBILITY",
        CanExecute = "ArcTexel.HasDocument",
        Icon = ArcPerfectIcons.FileGhost, AnalyticsTrack = true)]
    public void ToggleVisible()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null)
            return;

        member.IsVisibleBindable = !member.IsVisibleBindable;
    }

    [Evaluator.CanExecute("ArcTexel.Layer.HasMemberAbove",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentViewModel.SelectedStructureMember), nameof(DocumentViewModel.AllChangesSaved))]
    public bool HasMemberAbove(object property) => HasSelectedMember(true);

    [Evaluator.CanExecute("ArcTexel.Layer.HasMemberBelow",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentViewModel.SelectedStructureMember), nameof(DocumentViewModel.AllChangesSaved))]
    public bool HasMemberBelow(object property) => HasSelectedMember(false);

    [Command.Basic("ArcTexel.Layer.MoveSelectedMemberUpwards", "MOVE_MEMBER_UP", "MOVE_MEMBER_UP_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.HasMemberAbove", AnalyticsTrack = true)]
    public void MoveSelectedMemberUpwards() => MoveSelectedMember(true);

    [Command.Basic("ArcTexel.Layer.MoveSelectedMemberDownwards", "MOVE_MEMBER_DOWN", "MOVE_MEMBER_DOWN_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.HasMemberBelow", AnalyticsTrack = true)]
    public void MoveSelectedMemberDownwards() => MoveSelectedMember(false);

    [Command.Basic("ArcTexel.Layer.MergeSelected", "MERGE_ALL_SELECTED_LAYERS", "MERGE_ALL_SELECTED_LAYERS",
        CanExecute = "ArcTexel.Layer.HasMultipleSelectedMembers", AnalyticsTrack = true)]
    public void MergeSelected()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;
        var selected = GetSelected();
        if (selected.Count == 0)
            return;
        doc.Operations.MergeStructureMembers(selected);
    }

    public void MergeSelectedWith(bool above)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (doc is null || member is null)
            return;

        IStructureMemberHandler? nextMergeableMember = doc.StructureHelper.GetAboveMember(member.Id, true);
        IStructureMemberHandler? previousMergeableMember = doc.StructureHelper.GetBelowMember(member.Id, true);

        if (!above && previousMergeableMember is null)
            return;
        if (above && nextMergeableMember is null)
            return;

        doc.Operations.MergeStructureMembers(new List<Guid>
        {
            member.Id, above ? nextMergeableMember.Id : previousMergeableMember.Id
        });
    }

    [Command.Basic("ArcTexel.Layer.MergeWithAbove", "MERGE_WITH_ABOVE", "MERGE_WITH_ABOVE_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.HasMemberAbove", AnalyticsTrack = true)]
    public void MergeWithAbove() => MergeSelectedWith(true);

    [Command.Basic("ArcTexel.Layer.MergeWithBelow", "MERGE_WITH_BELOW", "MERGE_WITH_BELOW_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.HasMemberBelow",
        Icon = ArcPerfectIcons.Merge, AnalyticsTrack = true)]
    public void MergeWithBelow() => MergeSelectedWith(false);

    [Evaluator.CanExecute("ArcTexel.Layer.ReferenceLayerExists",
        nameof(ViewModelMain.DocumentManagerSubViewModel),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.ReferenceLayerViewModel),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.ReferenceLayerViewModel.ReferenceTexture))]
    public bool ReferenceLayerExists() =>
        Owner.DocumentManagerSubViewModel.ActiveDocument?.ReferenceLayerViewModel.ReferenceTexture is not null;

    [Evaluator.CanExecute("ArcTexel.Layer.ReferenceLayerDoesntExist",
        nameof(ViewModelMain.DocumentManagerSubViewModel),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument),
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.ReferenceLayerViewModel.ReferenceTexture))]
    public bool ReferenceLayerDoesntExist() =>
        Owner.DocumentManagerSubViewModel.ActiveDocument is not null &&
        Owner.DocumentManagerSubViewModel.ActiveDocument.ReferenceLayerViewModel.ReferenceTexture is null;

    [Command.Basic("ArcTexel.Layer.ImportReferenceLayer", "ADD_REFERENCE_LAYER", "ADD_REFERENCE_LAYER",
        CanExecute = "ArcTexel.Layer.ReferenceLayerDoesntExist",
        Icon = ArcPerfectIcons.AddReference, AnalyticsTrack = true)]
    public async Task ImportReferenceLayer()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        string path = await OpenReferenceLayerFilePicker();
        if (path is null)
            return;

        Surface bitmap;
        try
        {
            bitmap = Surface.Load(path);
        }
        catch (RecoverableException e)
        {
            NoticeDialog.Show(title: "ERROR", message: e.DisplayMessage);
            return;
        }
        catch (ArgumentException e)
        {
            NoticeDialog.Show(title: "ERROR", message: e.Message);
            return;
        }
        catch (Exception e)
        {
            CrashHelper.SendExceptionInfo(e);
            NoticeDialog.Show(title: "ERROR", message: e.Message);
            return;
        }

        byte[] bytes = bitmap.ToByteArray();

        bitmap.Dispose();

        VecI size = new VecI(bitmap.Size.X, bitmap.Size.Y);

        doc.Operations.ImportReferenceLayer(
            [..bytes],
            size);
    }

    private async Task<string> OpenReferenceLayerFilePicker()
    {
        var imagesFilter = new FileTypeDialogDataSet(FileTypeDialogDataSet.SetKind.Image).GetFormattedTypes(true);
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var filePicker = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = new LocalizedString("REFERENCE_LAYER_PATH"), FileTypeFilter = imagesFilter,
            });

            if (filePicker is null || filePicker.Count == 0)
                return null;

            return filePicker[0].Path.LocalPath;
        }

        return null;
    }

    [Command.Basic("ArcTexel.Layer.DeleteReferenceLayer", "DELETE_REFERENCE_LAYER", "DELETE_REFERENCE_LAYER",
        CanExecute = "ArcTexel.Layer.ReferenceLayerExists", Icon = ArcPerfectIcons.Trash, AnalyticsTrack = true)]
    public void DeleteReferenceLayer()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        doc.Operations.DeleteReferenceLayer();
    }

    [Command.Basic("ArcTexel.Layer.TransformReferenceLayer", "TRANSFORM_REFERENCE_LAYER", "TRANSFORM_REFERENCE_LAYER",
        CanExecute = "ArcTexel.Layer.ReferenceLayerExists",
        Icon = ArcPerfectIcons.Crop, AnalyticsTrack = true)]
    public void TransformReferenceLayer()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        doc.Operations.TransformReferenceLayer();
    }

    [Command.Basic("ArcTexel.Layer.ToggleReferenceLayerTopMost", "TOGGLE_REFERENCE_LAYER_POS",
        "TOGGLE_REFERENCE_LAYER_POS_DESCRIPTIVE", CanExecute = "ArcTexel.Layer.ReferenceLayerExists",
        IconEvaluator = "ArcTexel.Layer.ToggleReferenceLayerTopMostIcon", AnalyticsTrack = true)]
    public void ToggleReferenceLayerTopMost()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        doc.ReferenceLayerViewModel.IsTopMost = !doc.ReferenceLayerViewModel.IsTopMost;
    }

    [Command.Basic("ArcTexel.Layer.ResetReferenceLayerPosition", "RESET_REFERENCE_LAYER_POS",
        "RESET_REFERENCE_LAYER_POS", CanExecute = "ArcTexel.Layer.ReferenceLayerExists",
        Icon = ArcPerfectIcons.Reset, AnalyticsTrack = true)]
    public void ResetReferenceLayerPosition()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;

        doc.Operations.ResetReferenceLayerPosition();
    }

    [Command.Basic("ArcTexel.Layer.Rasterize", "RASTERIZE_ACTIVE_LAYER", "RASTERIZE_ACTIVE_LAYER_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.AnySelectedLayerIsRasterizable",
        Icon = ArcPerfectIcons.LowresCircle, MenuItemPath = "LAYER/RASTERIZE_ACTIVE_LAYER",
        AnalyticsTrack = true)]
    public void RasterizeActiveLayer()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var selectedMembers = doc.ExtractSelectedLayers();
        if (selectedMembers.Count == 0)
            return;

        var block = doc.Operations.StartChangeBlock();

        foreach (var member in selectedMembers)
        {
            doc!.Operations.Rasterize(member);
        }

        block.Dispose();
    }


    [Command.Basic("ArcTexel.Layer.ConvertToCurve", "CONVERT_TO_CURVE", "CONVERT_TO_CURVE_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.AnySelectedMemberIsVectorLayer",
        MenuItemPath = "LAYER/CONVERT_TO_CURVE", AnalyticsTrack = true)]
    public void ConvertActiveLayersToCurve()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var selectedMembers = doc.ExtractSelectedLayers();
        if (selectedMembers.Count == 0)
            return;

        var block = doc.Operations.StartChangeBlock();

        foreach (var member in selectedMembers)
        {
            doc!.Operations.ConvertToCurve(member);
        }

        block.Dispose();
    }

    [Command.Basic("ArcTexel.Layer.SeparateShapes", "SEPARATE_SHAPES", "SEPARATE_SHAPES_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.AnySelectedMemberIsVectorLayer",
        MenuItemPath = "LAYER/SEPARATE_SHAPES", AnalyticsTrack = true)]
    public void SeparateShapes()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var selectedMembers = doc.ExtractSelectedLayers();
        if (selectedMembers.Count == 0)
            return;

        var block = doc.Operations.StartChangeBlock();

        foreach (var member in selectedMembers)
        {
            doc!.Operations.SeparateShapes(member);
        }

        block.Dispose();
    }

    [Command.Basic("ArcTexel.Layer.ExtractSelectedText", "EXTRACT_SELECTED_TEXT", "EXTRACT_SELECTED_TEXT_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.SelectedMemberIsSelectedText",
        Key = Key.X, Modifiers = KeyModifiers.Control | KeyModifiers.Shift,
        Parameter = false,
        ShortcutContexts = [typeof(ViewportWindowViewModel), typeof(TextOverlay)],
        MenuItemPath = "LAYER/EXTRACT_SELECTED_TEXT", AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Layer.ExtractSelectedCharacters", "EXTRACT_SELECTED_CHARACTERS",
        "EXTRACT_SELECTED_CHARACTERS_DESCRIPTIVE",
        CanExecute = "ArcTexel.Layer.SelectedMemberIsSelectedText",
        Key = Key.X, Modifiers = KeyModifiers.Control | KeyModifiers.Shift | KeyModifiers.Alt,
        Parameter = true,
        ShortcutContexts = [typeof(ViewportWindowViewModel), typeof(TextOverlay)],
        MenuItemPath = "LAYER/EXTRACT_SELECTED_CHARACTERS", AnalyticsTrack = true)]
    public void ExtractSelectedText(bool extractEachCharacter)
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        var member = doc?.SelectedStructureMember;
        if (member is null)
            return;

        if (member is not VectorLayerNodeViewModel vectorLayer)
            return;

        int startIndex = doc.TextOverlayViewModel.CursorPosition;
        int endIndex = doc.TextOverlayViewModel.SelectionEnd;

        doc!.Operations.ExtractSelectedText(vectorLayer.Id, startIndex, endIndex, extractEachCharacter);
    }

    [Evaluator.Icon("ArcTexel.Layer.ToggleReferenceLayerTopMostIcon")]
    public IImage GetAboveEverythingReferenceLayerIcon()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null || doc.ReferenceLayerViewModel.IsTopMost)
        {
            return ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.LayersTop);
        }

        return ArcPerfectIconExtensions.ToIcon(ArcPerfectIcons.LayersBottom, 18, 180);
    }
}
