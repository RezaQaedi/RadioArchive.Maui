using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// converter takes <see cref="IconType"/> and return FontAwesome
    /// </summary>
    public class IconTypetoFontAwsomeConverter : BaseConverter<IconType, string>
    {
        public override IconType ConvertBackTo(string value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override string ConvertFrom(IconType value, CultureInfo culture)
        {
            return value.ToFontAwesome();
        }
    }
}
