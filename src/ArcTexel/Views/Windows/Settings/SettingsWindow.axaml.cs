using ArcTexel.ViewModels;
using ArcTexel.Views.Dialogs;

namespace ArcTexel.Views.Windows.Settings;

public partial class SettingsWindow : ArcTexelPopup
{
    public SettingsWindow() : this(0)
    {
    }

    public SettingsWindow(int page = 0)
    {
        InitializeComponent();
        var viewModel = DataContext as SettingsWindowViewModel;
        viewModel!.CurrentPage = page;
    }
}

