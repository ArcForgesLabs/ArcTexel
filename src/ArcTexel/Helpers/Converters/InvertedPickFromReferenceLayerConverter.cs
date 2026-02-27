using System.Globalization;
using ArcTexel.Models.Handlers.Tools;

namespace ArcTexel.Helpers.Converters;

internal class InvertedPickFromReferenceLayerConverter : SingleInstanceConverter<InvertedPickFromReferenceLayerConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IColorPickerHandler handler)
        {
            return !handler.PickFromReferenceLayer;
        }

        return false;
    }
}
