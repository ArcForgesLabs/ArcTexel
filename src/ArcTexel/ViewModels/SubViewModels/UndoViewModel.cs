using Avalonia.Input;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Commands.Attributes.Evaluators;
using ArcTexel.Models.DocumentModels.UpdateableChangeExecutors.Features;
using ArcTexel.Models.Input;
using ArcTexel.UI.Common.Fonts;

namespace ArcTexel.ViewModels.SubViewModels;

[Command.Group("ArcTexel.Undo", "UNDO")]
internal class UndoViewModel : SubViewModel<ViewModelMain>
{
    public UndoViewModel(ViewModelMain owner)
        : base(owner)
    {
    }

    /// <summary>
    ///     Redo last action.
    /// </summary>
    [Command.Basic("ArcTexel.Undo.Redo", "REDO", "REDO_DESCRIPTIVE", CanExecute = "ArcTexel.Undo.CanRedo", 
        Key = Key.Y, Modifiers = KeyModifiers.Control,
        Icon = ArcPerfectIcons.Redo, MenuItemPath = "EDIT/REDO", MenuItemOrder = 1, AnalyticsTrack = true)]
    [CustomOsShortcut("ArcTexel.Undo.Redo", "MacOS", Key.Z, KeyModifiers.Meta | KeyModifiers.Shift)]
    public void Redo()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null || (!doc.IsChangeFeatureActive<IMidChangeUndoableExecutor>() && !doc.HasSavedRedo))
            return;
        
        doc.Operations.InvokeCustomAction(
            () =>
        {
            Owner.ToolsSubViewModel.OnPreRedoInlet();
        }, false);
        doc.Operations.Redo();
        doc.Operations.InvokeCustomAction(
            () =>
        {
            Owner.ToolsSubViewModel.OnPostRedoInlet();
        }, false);
    }

    /// <summary>
    ///     Undo last action.
    /// </summary>
    [Command.Basic("ArcTexel.Undo.Undo", "UNDO", "UNDO_DESCRIPTIVE", 
        CanExecute = "ArcTexel.Undo.CanUndo", 
        Key = Key.Z, Modifiers = KeyModifiers.Control,
        Icon = ArcPerfectIcons.Undo, MenuItemPath = "EDIT/UNDO", MenuItemOrder = 0, AnalyticsTrack = true)]
    public void Undo()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null || (!doc.IsChangeFeatureActive<IMidChangeUndoableExecutor>() && !doc.HasSavedUndo))
            return;
        doc.Operations.InvokeCustomAction(
            () =>
        {
            Owner.ToolsSubViewModel.OnPreUndoInlet();
        }, false);
        doc.Operations.Undo();
        doc.Operations.InvokeCustomAction(
            () =>
        {
            Owner.ToolsSubViewModel.OnPostUndoInlet();
        }, false);
    }

    /// <summary>
    ///     Returns true if undo can be done.
    /// </summary>
    /// <param name="property">CommandParameter.</param>
    /// <returns>True if can undo.</returns>
    [Evaluator.CanExecute("ArcTexel.Undo.CanUndo", 
        nameof(ViewModelMain.DocumentManagerSubViewModel), 
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument), 
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.HasSavedUndo))]
    public bool CanUndo()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;
        
        IMidChangeUndoableExecutor executor = doc.TryGetExecutorFeature<IMidChangeUndoableExecutor>();
        
        return executor is { CanUndo: true } || doc.HasSavedUndo;
    }

    /// <summary>
    ///     Returns true if redo can be done.
    /// </summary>
    /// <param name="property">CommandProperty.</param>
    /// <returns>True if can redo.</returns>
    [Evaluator.CanExecute("ArcTexel.Undo.CanRedo", 
        nameof(ViewModelMain.DocumentManagerSubViewModel), 
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument), 
        nameof(ViewModelMain.DocumentManagerSubViewModel.ActiveDocument.HasSavedRedo))]
    public bool CanRedo()
    {
        var doc = Owner.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return false;
        
        IMidChangeUndoableExecutor executor = doc.TryGetExecutorFeature<IMidChangeUndoableExecutor>();
        
        return executor is { CanRedo: true } || doc.HasSavedRedo;
    }
}
