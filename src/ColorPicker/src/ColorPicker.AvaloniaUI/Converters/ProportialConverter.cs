#nullable enable
using System.Globalization;
using Avalonia.Data.Converters;

namespace ColorPicker.Converters;

internal class ProportialConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count >= 3 &&
            values[0] is double v0 &&
            values[1] is double v1 &&
            values[2] is double v2 &&
            v2 != 0)
        {
            return v0 * (v1 / v2);
        }

        return 0d;
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
