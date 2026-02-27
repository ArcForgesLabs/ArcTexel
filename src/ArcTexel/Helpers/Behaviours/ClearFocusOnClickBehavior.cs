using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using ArcTexel.Models.Controllers;

namespace ArcTexel.Helpers.Behaviours;

internal class ClearFocusOnClickBehavior : Behavior<Control>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.AddHandler(InputElement.PointerPressedEvent, AssociatedObject_MouseDown);
        AssociatedObject.AddHandler(InputElement.LostFocusEvent, AssociatedObject_LostFocus);
    }

    private void AssociatedObject_LostFocus(object? sender, RoutedEventArgs? e)
    {
    }

    protected override void OnDetaching()
    {
        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, AssociatedObject_MouseDown);
    }

    private void AssociatedObject_MouseDown(object? sender, PointerPressedEventArgs? e)
    {
        AssociatedObject?.Focus();
        ShortcutController.UnblockShortcutExecutionAll();
    }
}
