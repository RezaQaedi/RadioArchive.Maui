using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// converter takes in date and converts to user friendly time 
    /// </summary>
    public class TimeToDisplayTimeConverter : BaseConverter<DateTimeOffset, string>
    {
        public override string ConvertFrom(DateTimeOffset value, CultureInfo culture)
        {
            if (value == default)
                return "none";

            //get the time
            var time = value;
            var Diffrence = DateTimeOffset.UtcNow - time;

            if (Diffrence.Days <= 7)
            {
                if (Diffrence.Days == 0)
                    return "Today";

                return $"{Diffrence.Days} Day ago";
            }

            //otherwise, return a full date
            return time.ToLocalTime().ToString("yyy/MM/dd");
        }

        public override DateTimeOffset ConvertBackTo(string value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
