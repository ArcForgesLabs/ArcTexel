using ArcTexel.Extensions.CommonApi.UserPreferences;
using ArcTexel.Helpers;
using ArcTexel.Models;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.Models.Commands.Attributes.Evaluators;
using ArcTexel.Models.DocumentModels.Autosave;
using ArcTexel.Models.IO;
using ArcTexel.OperatingSystem;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Document;

namespace ArcTexel.ViewModels.SubViewModels;

internal class AutosaveViewModel(ViewModelMain owner, DocumentManagerViewModel documentManager) : SubViewModel<ViewModelMain>(owner)
{
    public static bool SaveSessionStateEnabled => IPreferences.Current!.GetPreference(PreferencesConstants.SaveSessionStateEnabled, PreferencesConstants.SaveSessionStateDefault);

    [Command.Basic("ArcTexel.Autosave.OpenAutosaveFolder", "AUTOSAVE_OPEN_FOLDER", "AUTOSAVE_OPEN_FOLDER_DESCRIPTIVE",
        Icon = ArcPerfectIcons.Folder)]
    public void OpenAutosaveFolder()
    {
        if (Directory.Exists(Paths.PathToUnsavedFilesFolder))
        {
            IOperatingSystem.Current.OpenFolder(Paths.PathToUnsavedFilesFolder);
        }
    }

    [Command.Basic("ArcTexel.Autosave.ToggleAutosave", "AUTOSAVE_TOGGLE", "AUTOSAVE_TOGGLE_DESCRIPTIVE",
        CanExecute = "ArcTexel.Autosave.HasDocumentAndAutosaveEnabled")]
    public void ToggleAutosave()
    {
        var autosaveViewModel = documentManager.ActiveDocument!.AutosaveViewModel;

        autosaveViewModel.CurrentDocumentAutosaveEnabled = !autosaveViewModel.CurrentDocumentAutosaveEnabled;
    }

    [Evaluator.CanExecute("ArcTexel.Autosave.HasDocumentAndAutosaveEnabled")]
    public bool HasDocumentAndAutosaveEnabled() =>
        documentManager.DocumentNotNull() && IPreferences.Current!.GetPreference<bool>(PreferencesConstants.AutosaveEnabled);

    public void CleanupAutosavedFilesAndHistory()
    {
        if (!Directory.Exists(Paths.PathToUnsavedFilesFolder))
            return;

        List<AutosaveHistorySession>? autosaveHistory = IPreferences.Current!.GetLocalPreference<List<AutosaveHistorySession>>(PreferencesConstants.AutosaveHistory);
        if (autosaveHistory is null)
            autosaveHistory = new();

        if (autosaveHistory.Count > 3)
            autosaveHistory = autosaveHistory[^3..];

        foreach (var path in Directory.EnumerateFiles(Paths.PathToUnsavedFilesFolder))
        {
            try
            {
                Guid fileGuid = AutosaveHelper.GetAutosaveGuid(path)!.Value;
                bool lastWriteIsOld = (DateTime.Now - File.GetLastWriteTime(path)).TotalDays > Constants.MaxAutosaveFilesLifetimeDays;
                bool creationDateIsOld = (DateTime.Now - File.GetCreationTime(path)).TotalDays > Constants.MaxAutosaveFilesLifetimeDays;
                bool presentInHistory = autosaveHistory.Any(sess => sess.AutosaveEntries.Any(entry => entry.TempFileGuid == fileGuid));

                if (!presentInHistory && lastWriteIsOld && creationDateIsOld)
                    File.Delete(path);
            }
            catch (Exception e)
            {
                CrashHelper.SendExceptionInfo(e);
            }
        }
    }
}
