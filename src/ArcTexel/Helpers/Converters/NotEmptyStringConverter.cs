using System.Globalization;

namespace ArcTexel.Helpers.Converters;

internal class NotEmptyStringConverter : SingleInstanceConverter<NotEmptyStringConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !string.IsNullOrEmpty(value as string);
    }
}
