#nullable enable
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ColorPicker.Converters;

public class TextBoxTextStringFormatMultiValueConverter : IMultiValueConverter
{
    private readonly string numericFormat = "N1";

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count != 2)
            throw new ArgumentException("Values array should contain 2 elements", nameof(values));

        if (values[0] is null)
        {
            return AvaloniaProperty.UnsetValue;
        }

        object value0 = values[0]!;
        if (value0 is not double doubleVal)
        {
            string? valueText = value0.ToString();
            if (string.IsNullOrWhiteSpace(valueText) || !double.TryParse(valueText, out doubleVal))
            {
                return AvaloniaProperty.UnsetValue;
            }
        }

        bool showFractionalPart = values[1] is true;

        return doubleVal.ToString(showFractionalPart ? numericFormat : "N0");
    }

    public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        string? text = value?.ToString();
        if (string.IsNullOrWhiteSpace(text))
        {
            return new object?[] { AvaloniaProperty.UnsetValue, AvaloniaProperty.UnsetValue };
        }

        if (!double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture,
                out var result)) return new[] { AvaloniaProperty.UnsetValue, AvaloniaProperty.UnsetValue };

        return new object?[] { result.ToString(numericFormat), AvaloniaProperty.UnsetValue };
    }
}
