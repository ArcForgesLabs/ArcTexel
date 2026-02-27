using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.ViewModels.SubViewModels;

internal class UpdateViewModel : SubViewModel<ViewModelMain>
{
    private double currentProgress;
    private UpdateState updateState = UpdateState.UpToDate;
    private LocalizedString versionText = new LocalizedString("UP_TO_DATE");

    public UpdateViewModel(ViewModelMain owner)
        : base(owner)
    {
    }

    public UpdateState UpdateState
    {
        get => updateState;
        set
        {
            updateState = value;
            OnPropertyChanged(nameof(UpdateState));
            OnPropertyChanged(nameof(IsUpdateAvailable));
            OnPropertyChanged(nameof(UpdateReadyToInstall));
            OnPropertyChanged(nameof(IsDownloading));
            OnPropertyChanged(nameof(IsUpToDate));
            OnPropertyChanged(nameof(UpdateStateString));
        }
    }

    public LocalizedString VersionText
    {
        get => versionText;
        set
        {
            versionText = value;
            OnPropertyChanged(nameof(VersionText));
        }
    }

    public string UpdateStateString => new LocalizedString("UP_TO_DATE");

    public bool IsUpdateAvailable => false;
    public bool UpdateReadyToInstall => false;
    public bool IsDownloading => false;
    public bool IsUpToDate => true;

    public double CurrentProgress
    {
        get => currentProgress;
        set
        {
            currentProgress = value;
            OnPropertyChanged(nameof(CurrentProgress));
        }
    }

    public bool SelfUpdatingAvailable => false;

    public AsyncRelayCommand DownloadCommand => new AsyncRelayCommand(() => Task.CompletedTask);
    public RelayCommand InstallCommand => new RelayCommand(() => { });
}

public enum UpdateState
{
    UnableToCheck,
    Checking,
    FailedDownload,
    ReadyToInstall,
    Downloading,
    UpdateAvailable,
    UpToDate
}
