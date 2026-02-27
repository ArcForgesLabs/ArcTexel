using Avalonia;
using Avalonia.Input;
using ArcTexel.Models.Commands;
using ArcTexel.Models.Commands.Commands;
using ArcTexel.Models.Input;

namespace ArcTexel.Views.Dialogs;

internal partial class ShortcutsPopup : ArcTexelPopup
{
    public static readonly StyledProperty<CommandController> ControllerProperty = AvaloniaProperty.Register<ShortcutsPopup, CommandController>(
        nameof(Controller));

    public static readonly StyledProperty<bool> IsTopmostProperty = AvaloniaProperty.Register<ShortcutsPopup, bool>(
        "IsTopmost");

    public bool IsTopmost
    {
        get => GetValue(IsTopmostProperty);
        set => SetValue(IsTopmostProperty, value);
    }

    public CommandController Controller
    {
        get => GetValue(ControllerProperty);
        set => SetValue(ControllerProperty, value);
    }

    Command settingsCommand;

    public ShortcutsPopup(CommandController controller)
    {
        DataContext = this;
        InitializeComponent();
        Controller = controller;
        settingsCommand = Controller.Commands["ArcTexel.Window.OpenSettingsWindow"];
    }

    private void ShortcutPopup_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (settingsCommand.Shortcut != new KeyCombination(e.Key, e.KeyModifiers))
        {
            return;
        }

        settingsCommand.Methods.Execute("Keybinds");
    }
}

