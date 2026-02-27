using Avalonia;

namespace ArcTexel.UI.Common.Localization;

public interface ICustomTranslatorElement
{
    public void SetTranslationBinding(AvaloniaProperty dependencyProperty, IObservable<string> binding);
    public AvaloniaProperty GetDependencyProperty();
}
