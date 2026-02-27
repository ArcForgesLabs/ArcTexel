using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using ArcTexel.Helpers.Extensions;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Helpers;
using ArcTexel.Models.Controllers;
using ArcTexel.Models.Services.NewsFeed;
using ArcTexel.Models.Structures;
using ArcTexel.Models.UserData;
using ArcTexel.OperatingSystem;
using ArcTexel.ViewModels.SubViewModels;
using ArcTexel.Views.Dialogs;

namespace ArcTexel.Views.Windows;

/// <summary>
/// Interaction logic for HelloTherePopup.xaml.
/// </summary>
internal partial class HelloTherePopup : ArcTexelPopup
{
    public RecentlyOpenedCollection RecentlyOpened { get => FileViewModel.RecentlyOpened; }

    public static readonly StyledProperty<FileViewModel> FileViewModelProperty =
        AvaloniaProperty.Register<HelloTherePopup, FileViewModel>(nameof(FileViewModel));

    public static readonly StyledProperty<bool> RecentlyOpenedEmptyProperty =
        AvaloniaProperty.Register<HelloTherePopup, bool>(nameof(RecentlyOpenedEmpty));

    public static readonly StyledProperty<bool> IsFetchingNewsProperty =
        AvaloniaProperty.Register<HelloTherePopup, bool>(nameof(IsFetchingNews), defaultValue: default(bool));

    public static readonly StyledProperty<bool> ShowAllBetaExamplesProperty =
        AvaloniaProperty.Register<HelloTherePopup, bool>(nameof(ShowAllBetaExamples));

    public static readonly StyledProperty<bool> NewsPanelCollapsedProperty =
        AvaloniaProperty.Register<HelloTherePopup, bool>(nameof(NewsPanelCollapsed), defaultValue: false);

    public static readonly StyledProperty<bool> FailedFetchingNewsProperty =
        AvaloniaProperty.Register<HelloTherePopup, bool>(nameof(FailedFetchingNews), defaultValue: false);

    static HelloTherePopup()
    {
        NewsPanelCollapsedProperty.Changed.Subscribe(NewsPanelCollapsedChangedCallback);
    }

    public bool NewsPanelCollapsed
    {
        get { return (bool)GetValue(NewsPanelCollapsedProperty); }
        set { SetValue(NewsPanelCollapsedProperty, value); }
    }

    public bool IsFetchingNews
    {
        get { return (bool)GetValue(IsFetchingNewsProperty); }
        set { SetValue(IsFetchingNewsProperty, value); }
    }

    public bool ShowAllBetaExamples
    {
        get => GetValue(ShowAllBetaExamplesProperty);
        set => SetValue(ShowAllBetaExamplesProperty, value);
    }

    public bool FailedFetchingNews
    {
        get { return (bool)GetValue(FailedFetchingNewsProperty); }
        set { SetValue(FailedFetchingNewsProperty, value); }
    }

    public ObservableRangeCollection<News> News { get; set; } = new ObservableRangeCollection<News>();

    public static string VersionText =>
        $"v{VersionHelpers.GetCurrentAssemblyVersionString()}";

    public FileViewModel FileViewModel
    {
        get => (FileViewModel)GetValue(FileViewModelProperty);
        set => SetValue(FileViewModelProperty, value);
    }

    public bool RecentlyOpenedEmpty
    {
        get => (bool)GetValue(RecentlyOpenedEmptyProperty);
        set => SetValue(RecentlyOpenedEmptyProperty, value);
    }

    public AsyncRelayCommand OpenFileCommand { get; set; }

    public AsyncRelayCommand OpenNewFileCommand { get; set; }

    public RelayCommand NewFromClipboardCommand { get; set; }

    public RelayCommand<string> OpenRecentCommand { get; set; }

    public RelayCommand<string> OpenInExplorerCommand { get; set; }

    public RelayCommand<bool> SetShowAllBetaExamplesCommand { get; set; }

    public bool IsClosing { get; private set; }

    private NewsProvider NewsProvider { get; set; }

    public bool NewsDisabled => _newsDisabled;

    public bool ShowDonateButton => // Steam doesn't allow external donations :(
#if STEAM
        false;
#else
        true;
#endif

    private bool _newsDisabled = false;
    
    private bool hasImageInClipboard = false;

    public HelloTherePopup(FileViewModel fileViewModel)
    {
        DataContext = this;
        FileViewModel = fileViewModel;

        OpenFileCommand = new AsyncRelayCommand(OpenFile);
        OpenNewFileCommand = new AsyncRelayCommand(OpenNewFile);
        OpenRecentCommand = new RelayCommand<string>(OpenRecent);
        SetShowAllBetaExamplesCommand = new RelayCommand<bool>(SetShowAllBetaExamples);
        OpenInExplorerCommand = new RelayCommand<string>(OpenInExplorer, CanOpenInExplorer);
        NewFromClipboardCommand = new RelayCommand(NewFromClipboard, CanOpenFromClipboard);

        RecentlyOpenedEmpty = RecentlyOpened.Count == 0;
        RecentlyOpened.CollectionChanged += RecentlyOpened_CollectionChanged;

        _newsDisabled = ArcTexelSettings.StartupWindow.DisableNewsPanel.Value;

        NewsProvider = new NewsProvider();

        CheckHasClipboardInImage();

        Closing += (_, _) => { IsClosing = true; };

        Activated += RefreshClipboardImg;
        Loaded += HelloTherePopup_OnLoaded;

        InitializeComponent();

        int newsWidth = 300;

        NewsPanelCollapsed = ArcTexelSettings.StartupWindow.NewsPanelCollapsed.Value;

        if (_newsDisabled || NewsPanelCollapsed)
        {
            grid.ColumnDefinitions.Last<ColumnDefinition>().Width = new GridLength(0);
            newsWidth = 0;
        }

        if (RecentlyOpenedEmpty)
        {
            Width = 550 + newsWidth;
            Height = 680;
        }
        else if (RecentlyOpened.Count < 4)
        {
            Width = 545 + newsWidth;
            Height = 680;
        }
        else if (RecentlyOpened.Count < 7)
        {
            Width = 575 + newsWidth;
            Height = 850;
        }
    }

    private void SetShowAllBetaExamples(bool value)
    {
        ShowAllBetaExamples = value;
    }

    private static void NewsPanelCollapsedChangedCallback(AvaloniaPropertyChangedEventArgs<bool> e)
    {
        HelloTherePopup helloTherePopup = (HelloTherePopup)e.Sender;

        if (helloTherePopup._newsDisabled)
            return;

        if (e.NewValue.Value)
        {
            helloTherePopup.grid.ColumnDefinitions.Last<ColumnDefinition>().Width = new GridLength(0);
            helloTherePopup.Width -= 300;
        }
        else
        {
            helloTherePopup.grid.ColumnDefinitions.Last<ColumnDefinition>().Width = new GridLength(300);
            helloTherePopup.Width += 300;
        }

        ArcTexelSettings.StartupWindow.NewsPanelCollapsed.Value = e.NewValue.Value;
    }

    private void RecentlyOpened_CollectionChanged(object sender,
        System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        RecentlyOpenedEmpty = FileViewModel.RecentlyOpened.Count == 0;
    }

    private async Task OpenFile()
    {
        Application.Current.ForDesktopMainWindow(mainWindow => mainWindow.Activate());
        Close();
        await FileViewModel.OpenFromOpenFileDialog();
    }

    private async Task OpenNewFile()
    {
        Application.Current.ForDesktopMainWindow(mainWindow => mainWindow.Activate());
        Close();
        await FileViewModel.CreateFromNewFileDialog();
    }

    private void RefreshClipboardImg(object? sender, EventArgs e)
    {
        CheckHasClipboardInImage();
    }

    private void CheckHasClipboardInImage()
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            hasImageInClipboard = await ClipboardController.IsImageInClipboard();
        }).ContinueWith(_ =>
        {
            Dispatcher.UIThread.Invoke(NewFromClipboardCommand.NotifyCanExecuteChanged);
        });
    }


    private void NewFromClipboard()
    {
        Activated -= RefreshClipboardImg;
        Application.Current.ForDesktopMainWindow(mainWindow => mainWindow.Activate());
        FileViewModel.OpenFromClipboard();
        Close();
    }

    private bool CanOpenFromClipboard()
    {
        return hasImageInClipboard;
    }

    private void OpenRecent(string parameter)
    {
        Application.Current.ForDesktopMainWindow(mainWindow => mainWindow.Activate());
        Close();
        FileViewModel.OpenRecent(parameter);
    }

    private void OpenInExplorer(string parameter)
    {
        IOperatingSystem.Current.OpenFolder(parameter);
    }

    private bool CanOpenInExplorer(string parameter) => File.Exists(parameter);

    private async void HelloTherePopup_OnLoaded(object sender, RoutedEventArgs e)
    {
        // TODO: Fetching news freezes input when no internet is present
        if (_newsDisabled) return;

        try
        {
            IsFetchingNews = true;
            var news = await NewsProvider.FetchNewsAsync();
            if (news is not null)
            {
                IsFetchingNews = false;
                News.Clear();
                News.AddRange(news);
                if (NewsPanelCollapsed && News.Any(x => x.IsNew))
                {
                    NewsPanelCollapsed = false;
                }
            }
            else
            {
                IsFetchingNews = false;
                FailedFetchingNews = true;
            }
        }
        catch (Exception ex)
        {
            IsFetchingNews = false;
            FailedFetchingNews = true;
            await CrashHelper.SendExceptionInfoAsync(ex);
        }
    }
}
