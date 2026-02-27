using System.Collections.Immutable;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using ArcTexel.Extensions.FlyUI.Converters;
using Color = ArcTexel.Extensions.CommonApi.FlyUI.Properties.Color;
using Colors = ArcTexel.Extensions.CommonApi.FlyUI.Properties.Colors;

namespace ArcTexel.Extensions.FlyUI.Elements;

public class Icon : LayoutElement
{
    private double size = 16;
    private string iconName = string.Empty;
    private Color color;

    public double Size { get => size; set => SetField(ref size, value); }
    public string IconName { get => iconName; set => SetField(ref iconName, value); }
    public Color Color { get => color; set => SetField(ref color, value); }

    public Icon(string iconName, double size = 16, Color? color = null)
    {
        IconName = iconName;
        Size = size;
        Color = color ?? Colors.White;
    }

    protected override Control CreateNativeControl()
    {
        TextBlock textBlock = new TextBlock();
        textBlock.Classes.Add("arc-icon");

        Binding iconNameBinding = new Binding()
        {
            Source = this, Path = nameof(IconName),
            Converter = new IconLookupConverter()
        };
        Binding sizeBinding = new Binding() { Source = this, Path = nameof(Size), };
        Binding colorBinding = new Binding()
        {
            Source = this, Path = nameof(Color), Converter = new ColorToAvaloniaBrushConverter()
        };

        textBlock.Bind(TextBlock.TextProperty, iconNameBinding);
        textBlock.Bind(TextBlock.FontSizeProperty, sizeBinding);
        textBlock.Bind(TextBlock.ForegroundProperty, colorBinding);

        return textBlock;
    }

    protected override IEnumerable<object> GetControlProperties()
    {
        yield return IconName;
        yield return Size;
        yield return Color;
    }

    protected override void DeserializeControlProperties(List<object> values)
    {
        IconName = (string)values[0];
        Size = (double)values[1];
        Color = (Color)values[2];
    }
}
