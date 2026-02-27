using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ArcTexel.ViewModels;
using ArcTexel.ViewModels.SubViewModels;
using ArcTexel.Views.Dialogs;

namespace ArcTexel.Views.Auth;

public partial class LoginPopup : ArcTexelPopup
{
    public LoginPopup()
    {
        InitializeComponent();
        DataContext = ViewModelMain.Current.UserViewModel;
    }

    protected override async void OnGotFocus(GotFocusEventArgs e)
    {
        if (DataContext is UserViewModel { WaitingForActivation: true } vm)
        {
            await vm.TryValidateSession();
        }
    }
}

