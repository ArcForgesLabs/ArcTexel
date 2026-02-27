using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ArcTexel.Extensions.CommonApi.FlyUI.Properties;

namespace ArcTexel.Extensions.FlyUI.Converters;

public class EdgesToThicknessConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Edges edges)
        {
            return new Thickness(edges.Left, edges.Top, edges.Right, edges.Bottom);
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Thickness thickness)
        {
            return new Edges(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        return null;
    }
}
