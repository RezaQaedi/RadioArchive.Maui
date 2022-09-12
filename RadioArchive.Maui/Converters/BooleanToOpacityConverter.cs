using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// Converter takes boolean and inverse it 
    /// </summary>
    public class BooleanToOpacityConverter : BaseConverter<bool, float>
    {
        public override float ConvertFrom(bool value, CultureInfo culture)
        {
            return value ? 1f : 0.2f;
        }

        public override bool ConvertBackTo(float value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    };
}
