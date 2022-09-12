using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// Converter takes <see cref="TimeSpan"/> and give user friendly time span 
    /// </summary>
    public class TimeSpanToDisplayTimeConverter : BaseConverter<TimeSpan, string>
    {
        public override TimeSpan ConvertBackTo(string value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override string ConvertFrom(TimeSpan value, CultureInfo culture)
        {

            if (value.Hours > 0)
                return value.ToString(@"h\:m\:s");
            else
                return value.ToString(@"m\:s");
        }
    }
}
