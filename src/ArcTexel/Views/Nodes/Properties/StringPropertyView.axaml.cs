using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ArcTexel.Models.Dialogs;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using ArcTexel.OperatingSystem;
using ArcTexel.ViewModels.Nodes.Properties;

namespace ArcTexel.Views.Nodes.Properties;

public partial class StringPropertyView : NodePropertyView
{
    public static readonly StyledProperty<ICommand> OpenInDefaultAppCommandProperty =
        AvaloniaProperty.Register<StringPropertyView, ICommand>(
            nameof(OpenInDefaultAppCommand));

    public ICommand OpenInDefaultAppCommand
    {
        get => GetValue(OpenInDefaultAppCommandProperty);
        set => SetValue(OpenInDefaultAppCommandProperty, value);
    }

    public StringPropertyView()
    {
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
    }
}
