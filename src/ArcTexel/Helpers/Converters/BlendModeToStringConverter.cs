using System.Globalization;
using ArcTexel.Helpers.Extensions;
using ArcTexel.ChangeableDocument.Enums;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Helpers.Converters;
internal class BlendModeToStringConverter : SingleInstanceConverter<BlendModeToStringConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not BlendMode mode)
            return "<null>";
        return new LocalizedString(mode.LocalizedKeys()).Value;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
