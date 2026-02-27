using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using Drawie.Numerics;
using ArcDocks.Core.Docking;
using ArcTexel.Models.AnalyticsAPI;
using ArcTexel.Models.Commands;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Document;
using ArcTexel.ViewModels.UserPreferences;
using ArcTexel.Views;
using ArcTexel.Views.Auth;
using ArcTexel.Views.Dialogs;
using ArcTexel.Views.Windows;
using Command = ArcTexel.Models.Commands.Attributes.Commands.Command;
using Commands_Command = ArcTexel.Models.Commands.Attributes.Commands.Command;
using Settings_SettingsWindow = ArcTexel.Views.Windows.Settings.SettingsWindow;
using SettingsWindow = ArcTexel.Views.Windows.Settings.SettingsWindow;

namespace ArcTexel.ViewModels.SubViewModels;

#nullable enable
[Commands_Command.Group("ArcTexel.Window", "WINDOWS")]
internal class WindowViewModel : SubViewModel<ViewModelMain>, IWindowHandler
{
    private CommandController commandController;
    public RelayCommand<string> ShowAvalonDockWindowCommand { get; set; }
    public ObservableCollection<ViewportWindowViewModel> Viewports { get; } = new();
    public ObservableCollection<LazyViewportWindowViewModel> LazyViewports { get; } = new();
    public event EventHandler<ViewportWindowViewModel>? ActiveViewportChanged;
    public event Action<ViewportWindowViewModel> ViewportAdded;
    public event Action<ViewportWindowViewModel> ViewportClosed;

    public event Action<LazyViewportWindowViewModel> LazyViewportAdded;
    public event Action<LazyViewportWindowViewModel> LazyViewportRemoved;

    private object? activeWindow;

    public object? ActiveWindow
    {
        get => activeWindow;
        set
        {
            if (activeWindow == value)
                return;
            activeWindow = value;
            OnPropertyChanged(nameof(ActiveWindow));
            if (activeWindow is ViewportWindowViewModel viewport)
            {
                Owner.LayoutSubViewModel.LayoutManager.ShowViewport(viewport);
                ActiveViewportChanged?.Invoke(this, viewport);
            }
        }
    }

    public WindowViewModel(ViewModelMain owner, CommandController commandController)
        : base(owner)
    {
        ShowAvalonDockWindowCommand = new(ShowDockWindow);
        this.commandController = commandController;
    }

    [Commands_Command.Basic("ArcTexel.Window.CreateNewViewport", "NEW_WINDOW_FOR_IMG", "NEW_WINDOW_FOR_IMG",
        Icon = ArcPerfectIcons.PlusSquare, CanExecute = "ArcTexel.HasDocument",
        MenuItemPath = "VIEW/NEW_WINDOW_FOR_IMG", MenuItemOrder = 0, AnalyticsTrack = true)]
    public void CreateNewViewport()
    {
        var doc = ViewModelMain.Current?.DocumentManagerSubViewModel.ActiveDocument;
        if (doc is null)
            return;
        CreateNewViewport(doc);
    }

    [Commands_Command.Basic("ArcTexel.Window.CenterActiveViewport", "CENTER_ACTIVE_VIEWPORT",
        "CENTER_ACTIVE_VIEWPORT", CanExecute = "ArcTexel.HasDocument",
        Icon = ArcPerfectIcons.Center, AnalyticsTrack = true)]
    public void CenterCurrentViewport()
    {
        if (ActiveWindow is ViewportWindowViewModel viewport)
        {
            viewport.CenterViewportTrigger.Execute(this, viewport.GetRenderOutputSize());
        }
    }


    [Command.Basic("ArcTexel.Viewport.ToggleHud", "TOGGLE_HUD", "TOGGLE_HUD_DESCRIPTION",
        AnalyticsTrack = true, Key = Key.H, Modifiers = KeyModifiers.Shift, MenuItemPath = "VIEW/TOGGLE_HUD")]
    public void ToggleHudOfCurrentViewport()
    {
        if (ActiveWindow is ViewportWindowViewModel viewport)
        {
            viewport.HudVisible = !viewport.HudVisible;
        }
    }


    [Command.Internal("ArcTexel.Viewport.SetRenderOutput")]
    public void SetRenderOutputOfCurrentViewport(string renderOutput)
    {
        if (ActiveWindow is ViewportWindowViewModel viewport)
        {
            viewport.RenderOutputName = renderOutput;
        }
    }

    [Commands_Command.Basic("ArcTexel.Window.FlipHorizontally", "FLIP_VIEWPORT_HORIZONTALLY",
        "FLIP_VIEWPORT_HORIZONTALLY", CanExecute = "ArcTexel.HasDocument",
        Icon = ArcPerfectIcons.Image180, AnalyticsTrack = true)]
    public void FlipViewportHorizontally()
    {
        if (ActiveWindow is ViewportWindowViewModel viewport)
        {
            viewport.FlipX = !viewport.FlipX;
        }
    }

    [Commands_Command.Basic("ArcTexel.Window.FlipVertically", "FLIP_VIEWPORT_VERTICALLY", "FLIP_VIEWPORT_VERTICALLY",
        CanExecute = "ArcTexel.HasDocument",
        Icon = ArcPerfectIcons.XFlip, AnalyticsTrack = true)]
    public void FlipViewportVertically()
    {
        if (ActiveWindow is ViewportWindowViewModel viewport)
        {
            viewport.FlipY = !viewport.FlipY;
        }
    }

    public void CreateNewViewport(DocumentViewModel doc)
    {
        ViewportWindowViewModel newViewport = new ViewportWindowViewModel(this, doc);
        Viewports.Add(newViewport);
        foreach (var viewport in Viewports.Where(vp => vp.Document == doc))
        {
            viewport.IndexChanged();
        }

        ViewportAdded?.Invoke(newViewport);
    }

    public void CreateNewViewport(LazyDocumentViewModel lazyDoc)
    {
        LazyViewportWindowViewModel newViewport = new LazyViewportWindowViewModel(this, lazyDoc);
        LazyViewports.Add(newViewport);

        LazyViewportAdded?.Invoke(newViewport);
    }

    public void MakeDocumentViewportActive(DocumentViewModel? doc)
    {
        if (doc is null)
        {
            ActiveWindow = null;
            Owner.DocumentManagerSubViewModel.MakeActiveDocumentNull();
            return;
        }

        ActiveWindow = Viewports.FirstOrDefault(viewport => viewport.Document == doc);
    }

    public void MakeDocumentViewportActive(LazyDocumentViewModel? doc)
    {
        if (doc is null)
        {
            ActiveWindow = null;
            return;
        }

        ActiveWindow = LazyViewports.FirstOrDefault(viewport => viewport.LazyDocument == doc);
    }

    public string CalculateViewportIndex(ViewportWindowViewModel viewport)
    {
        ViewportWindowViewModel[] viewports = Viewports.Where(a => a.Document == viewport.Document).ToArray();
        if (viewports.Length < 2)
            return "";
        return $"[{Array.IndexOf(viewports, viewport) + 1}]";
    }

    public async Task<bool> OnViewportWindowCloseButtonPressed(ViewportWindowViewModel viewport)
    {
        var viewports = Viewports.Where(vp => vp.Document == viewport.Document).ToArray();
        if (viewports.Length == 1)
        {
            Analytics.SendCloseDocument();
            return await Owner.DisposeDocumentWithSaveConfirmation(viewport.Document);
        }

        Viewports.Remove(viewport);

        foreach (var sibling in viewports)
        {
            sibling.IndexChanged();
        }

        ViewportClosed?.Invoke(viewport);

        return true;
    }

    public void OnLazyViewportWindowCloseButtonPressed(LazyViewportWindowViewModel viewport)
    {
        LazyViewports.Remove(viewport);
        LazyViewportRemoved?.Invoke(viewport);
        Owner.CloseLazyDocument(viewport.LazyDocument);
    }

    public void CloseViewportsForDocument(DocumentViewModel document)
    {
        var viewports = Viewports.Where(vp => vp.Document == document).ToArray();
        foreach (ViewportWindowViewModel viewport in viewports)
        {
            Viewports.Remove(viewport);
            ViewportClosed?.Invoke(viewport);
        }
    }

    public void CloseViewportForLazyDocument(LazyDocumentViewModel lazyDoc)
    {
        if (lazyDoc is null)
            return;

        var viewport = LazyViewports.FirstOrDefault(vp => vp.LazyDocument == lazyDoc);
        if (viewport is not null)
        {
            LazyViewports.Remove(viewport);
            LazyViewportRemoved?.Invoke(viewport);
        }
    }

    [Commands_Command.Basic("ArcTexel.Window.OpenSettingsWindow", "OPEN_SETTINGS", "OPEN_SETTINGS_DESCRIPTIVE",
        Key = Key.OemComma, Modifiers = KeyModifiers.Control,
        MenuItemPath = "EDIT/SETTINGS", MenuItemOrder = 16, Icon = ArcPerfectIcons.Settings, AnalyticsTrack = true)]
    public static void OpenSettingsWindow(int page)
    {
        if (page < 0)
        {
            page = 0;
        }

        var settings = new Settings_SettingsWindow(page);
        settings.Show();
    }

    [Commands_Command.Basic("ArcTexel.Window.OpenStartupWindow", "OPEN_STARTUP_WINDOW", "OPEN_STARTUP_WINDOW",
        Icon = ArcPerfectIcons.Home, MenuItemPath = "VIEW/OPEN_STARTUP_WINDOW", MenuItemOrder = 1,
        AnalyticsTrack = true)]
    public void OpenHelloThereWindow()
    {
        new HelloTherePopup(Owner.FileSubViewModel).Show(MainWindow.Current);
    }

    [Command.Basic("ArcTexel.Window.OpenOnboardingWindow", "OPEN_ONBOARDING_WINDOW", "OPEN_ONBOARDING_WINDOW",
        Icon = ArcPerfectIcons.Compass, MenuItemPath = "VIEW/OPEN_ONBOARDING_WINDOW", MenuItemOrder = 2,
        AnalyticsTrack = true)]
    public OnboardingDialog OpenOnboardingWindow()
    {
        var dialog = new OnboardingDialog { DataContext = new OnboardingViewModel() };
        dialog.ShowDialog(MainWindow.Current);
        return dialog;
    }

    [Commands_Command.Basic("ArcTexel.Window.OpenShortcutWindow", "OPEN_SHORTCUT_WINDOW", "OPEN_SHORTCUT_WINDOW",
        Key = Key.F1,
        Icon = ArcPerfectIcons.Book, MenuItemPath = "VIEW/OPEN_SHORTCUT_WINDOW", MenuItemOrder = 2,
        AnalyticsTrack = true)]
    public void ShowShortcutWindow()
    {
        var popup = new ShortcutsPopup(commandController);
        popup.Show();
        popup.Activate();
    }

    [Commands_Command.Basic("ArcTexel.Window.OpenAboutWindow", "OPEN_ABOUT_WINDOW", "OPEN_ABOUT_WINDOW",
        Icon = ArcPerfectIcons.Info, MenuItemPath = "HELP/ABOUT", MenuItemOrder = 5, AnalyticsTrack = true)]
    public void OpenAboutWindow()
    {
        new AboutPopup().Show();
    }

    [Commands_Command.Internal("ArcTexel.Window.ShowDockWindow")]
    [Commands_Command.Basic("ArcTexel.Window.OpenPreviewWindow", "DocumentPreview", "OPEN_PREVIEW_WINDOW",
        "OPEN_PREVIEW_WINDOW")]
    public void ShowDockWindow(string id)
    {
        Owner.LayoutSubViewModel.LayoutManager.ShowDockable(id);
    }

    [Commands_Command.Basic("ArcTexel.Window.OpenAccountWindow", "OPEN_ACCOUNT_WINDOW", "OPEN_ACCOUNT_WINDOW",
        MenuItemOrder = 6, AnalyticsTrack = true)]
    public LoginPopup OpenAccountWindow(bool dialog = false)
    {
        LoginPopup popup = new LoginPopup();
        if (dialog)
        {
            popup.ShowDialog(MainWindow.Current);
            return popup;
        }

        popup.Show();
        return popup;
    }

    /// <summary>
    /// Used to save the WindowState before toggling to FullScreen-Mode.
    /// </summary>
    private WindowState LastWindowState { get; set; }

    /// <summary>
    /// Method used to toggle to FullScreen-Mode.
    /// </summary>
    [Commands_Command.Basic("ArcTexel.Window.ToggleFullscreen", "TOGGLE_FULLSCREEN", "TOGGLE_FULLSCREEN_DESCRIPTIVE",
        Key = Key.F11,
        Icon = ArcPerfectIcons.Fullscreen,
        AnalyticsTrack = true)]
    public void ToggleFullscreen()
    {
        var window = Owner.AttachedWindow;

        if (window is null) return;

        if (window.WindowState != WindowState.FullScreen) LastWindowState = window.WindowState;

        window.WindowState = window.WindowState switch
        {
            WindowState.FullScreen => LastWindowState,
            _ => WindowState.FullScreen
        };
    }
}
