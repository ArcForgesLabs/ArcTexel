using System.Drawing;
using Avalonia.Input;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Commands.Attributes.Evaluators;
using ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;
using Drawie.Numerics;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.ViewModels.SubViewModels;

[Command.Group("ArcTexel.Selection", "SELECTION")]
internal class SelectionViewModel : SubViewModel<ViewModelMain>
{
    public SelectionViewModel(ViewModelMain owner)
        : base(owner)
    {
    }

    [Command.Basic("ArcTexel.Selection.SelectAll", "SELECT_ALL", "SELECT_ALL_DESCRIPTIVE", CanExecute = "ArcTexel.HasDocument", Key = Key.A, Modifiers = KeyModifiers.Control,
        MenuItemPath = "SELECT/SELECT_ALL", MenuItemOrder = 8, Icon = ArcPerfectIcons.SelectAll, AnalyticsTrack = true)]
    public void SelectAll()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;
        doc.Operations.SelectAll();
    }

    [Command.Basic("ArcTexel.Selection.Clear", "CLEAR_SELECTION", "CLEAR_SELECTION", CanExecute = "ArcTexel.Selection.IsNotEmpty", Key = Key.D, Modifiers = KeyModifiers.Control,
        MenuItemPath = "SELECT/DESELECT", MenuItemOrder = 9, Icon = ArcPerfectIcons.Deselect, AnalyticsTrack = true)]
    public void ClearSelection()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;
        doc.Operations.ClearSelection();
    }

    [Command.Basic("ArcTexel.Selection.InvertSelection", "INVERT_SELECTION", "INVERT_SELECTION_DESCRIPTIVE", CanExecute = "ArcTexel.Selection.IsNotEmpty", Key = Key.I, Modifiers = KeyModifiers.Control,
        MenuItemPath = "SELECT/INVERT", MenuItemOrder = 10, Icon = ArcPerfectIcons.Invert, AnalyticsTrack = true)]
    public void InvertSelection()
    {
        Owner.DocumentManagerSubViewModel.ActiveDocument?.Operations.InvertSelection();
    }

    [Evaluator.CanExecute("ArcTexel.Selection.IsNotEmpty",
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectionPathBindable),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectionPathBindable.IsEmpty))]
    public bool SelectionIsNotEmpty()
    {
        return !Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectionPathBindable?.IsEmpty ?? false;
    }

    [Evaluator.CanExecute("ArcTexel.Selection.IsNotEmptyAndHasMask", 
        nameof(DocumentManagerViewModel.ActiveDocument),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectedStructureMember),
        nameof(DocumentManagerViewModel.ActiveDocument.SelectedStructureMember.HasMaskBindable))]
    public bool SelectionIsNotEmptyAndHasMask()
    {
        return SelectionIsNotEmpty() && (Owner.DocumentManagerSubViewModel.ActiveDocument?.SelectedStructureMember?.HasMaskBindable ?? false);
    }

    [Command.Basic("ArcTexel.Selection.TransformArea", "TRANSFORM_SELECTED_AREA", "TRANSFORM_SELECTED_AREA", CanExecute = "ArcTexel.Selection.IsNotEmpty", 
        Key = Key.T, Modifiers = KeyModifiers.Control, AnalyticsTrack = true)]
    public void TransformSelectedArea()
    {
        Owner.DocumentManagerSubViewModel.ActiveDocument?.Operations.TransformSelectedArea(false);
    }

    [Command.Basic("ArcTexel.Selection.NudgeSelectedObjectLeft", "NUDGE_SELECTED_LEFT", "NUDGE_SELECTED_LEFT", Key = Key.Left, Parameter = new int[] { -1, 0 }, Icon = ArcPerfectIcons.ChevronLeft, CanExecute = "ArcTexel.Selection.CanNudgeSelectedObject",
        ShortcutContexts = [typeof(ViewportWindowViewModel)])]
    [Command.Basic("ArcTexel.Selection.NudgeSelectedObjectRight", "NUDGE_SELECTED_RIGHT", "NUDGE_SELECTED_RIGHT", Key = Key.Right, Parameter = new int[] { 1, 0 }, Icon = ArcPerfectIcons.ChevronRight, CanExecute = "ArcTexel.Selection.CanNudgeSelectedObject",
        ShortcutContexts = [typeof(ViewportWindowViewModel)])]
    [Command.Basic("ArcTexel.Selection.NudgeSelectedObjectUp", "NUDGE_SELECTED_UP", "NUDGE_SELECTED_UP", Key = Key.Up, Parameter = new int[] { 0, -1 }, Icon = ArcPerfectIcons.ChevronUp, CanExecute = "ArcTexel.Selection.CanNudgeSelectedObject",
        ShortcutContexts = [typeof(ViewportWindowViewModel)])]
    [Command.Basic("ArcTexel.Selection.NudgeSelectedObjectDown", "NUDGE_SELECTED_DOWN", "NUDGE_SELECTED_DOWN", Key = Key.Down, Parameter = new int[] { 0, 1 }, Icon = ArcPerfectIcons.ChevronDown, CanExecute = "ArcTexel.Selection.CanNudgeSelectedObject",
        ShortcutContexts = [typeof(ViewportWindowViewModel)])]
    public void NudgeSelectedObject(int[] dist)
    {
        VecI distance = new(dist[0], dist[1]);
        Owner.DocumentManagerSubViewModel.ActiveDocument?.Operations.NudgeSelectedObject(distance);
    }

    [Command.Basic("ArcTexel.Selection.NewToMask", SelectionMode.New, "MASK_FROM_SELECTION", "MASK_FROM_SELECTION_DESCRIPTIVE", CanExecute = "ArcTexel.Selection.IsNotEmpty",
        MenuItemPath = "SELECT/SELECTION_TO_MASK/TO_NEW_MASK", MenuItemOrder = 12, Icon = ArcPerfectIcons.NewMask, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Selection.AddToMask", SelectionMode.Add, "ADD_SELECTION_TO_MASK", "ADD_SELECTION_TO_MASK", CanExecute = "ArcTexel.Selection.IsNotEmpty",
        MenuItemPath = "SELECT/SELECTION_TO_MASK/ADD_TO_MASK", MenuItemOrder = 13, Icon = ArcPerfectIcons.AddToMask, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Selection.SubtractFromMask", SelectionMode.Subtract, "SUBTRACT_SELECTION_FROM_MASK", "SUBTRACT_SELECTION_FROM_MASK", CanExecute = "ArcTexel.Selection.IsNotEmptyAndHasMask",
        MenuItemPath = "SELECT/SELECTION_TO_MASK/SUBTRACT_FROM_MASK", MenuItemOrder = 14, Icon = ArcPerfectIcons.Subtract, AnalyticsTrack = true)]
    [Command.Basic("ArcTexel.Selection.IntersectSelectionMask", SelectionMode.Intersect, "INTERSECT_SELECTION_MASK", "INTERSECT_SELECTION_MASK", CanExecute = "ArcTexel.Selection.IsNotEmptyAndHasMask",
        MenuItemPath = "SELECT/SELECTION_TO_MASK/INTERSECT_WITH_MASK", MenuItemOrder = 15, Icon = ArcPerfectIcons.Intersect, AnalyticsTrack = true)]
    [Command.Filter("ArcTexel.Selection.ToMaskMenu", "SELECTION_TO_MASK", "SELECTION_TO_MASK", Key = Key.M, Modifiers = KeyModifiers.Control, AnalyticsTrack = true)]
    public void SelectionToMask(SelectionMode mode)
    {
        if (Owner.DocumentManagerSubViewModel.ActiveDocument is null)
            return;
        
        Owner.DocumentManagerSubViewModel.ActiveDocument.Operations.SelectionToMask(mode, Owner.DocumentManagerSubViewModel.ActiveDocument.AnimationDataViewModel.ActiveFrameBindable);
    }

    [Command.Basic("ArcTexel.Selection.CropToSelection", "CROP_TO_SELECTION", "CROP_TO_SELECTION_DESCRIPTIVE", CanExecute = "ArcTexel.Selection.IsNotEmpty",
        MenuItemPath = "SELECT/CROP_TO_SELECTION", MenuItemOrder = 11, Icon = ArcPerfectIcons.CropToSelection, AnalyticsTrack = true)]
    public void CropToSelection()
    {
        var document = Owner.DocumentManagerSubViewModel.ActiveDocument;
        
        document!.Operations.CropToSelection(document.AnimationDataViewModel.ActiveFrameBindable);
    }

    [Evaluator.CanExecute("ArcTexel.Selection.CanNudgeSelectedObject",
        nameof(DocumentManagerViewModel.ActiveDocument))]
    public bool CanNudgeSelectedObject(int[] dist) => Owner.DocumentManagerSubViewModel.ActiveDocument
        ?.IsChangeFeatureActive<ITransformableExecutor>() ?? false;
}
