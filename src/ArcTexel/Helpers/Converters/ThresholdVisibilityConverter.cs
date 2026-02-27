using System.Globalization;
using ArcTexel.UI.Common.Converters;

namespace ArcTexel.Helpers.Converters;

internal class ThresholdVisibilityConverter
    : MarkupConverter
{
    public double Threshold { get; set; } = 100;
    public bool CheckIfLess { get; set; } = false;

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Check((double)value);
    }

    public bool Check(double value)
    {
        return CheckIfLess
            ? value < Threshold
            : value >= Threshold;
    }
}
