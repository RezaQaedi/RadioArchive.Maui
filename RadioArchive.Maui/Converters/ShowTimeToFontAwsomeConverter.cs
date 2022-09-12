using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// Converter takes <see cref="ShowTime"/> and give fontawsome string
    /// </summary>
    public class ShowTimeToFontAwsomeConverter : BaseConverter<ShowTime, string>
    {
        public override ShowTime ConvertBackTo(string value, CultureInfo culture)
        {    
            throw new NotImplementedException();
        }

        public override string ConvertFrom(ShowTime value, CultureInfo culture)
        {
            var fontAwsome = IconType.Podcast.ToFontAwesome();

            switch (value)
            {
                case ShowTime.Evening:
                    fontAwsome = IconType.Evening.ToFontAwesome();
                    break;
                case ShowTime.Morning:
                    fontAwsome = IconType.Morning.ToFontAwesome();
                    break;
                case ShowTime.Afternoon:
                    fontAwsome = IconType.Afternoon.ToFontAwesome();
                    break;
            }

            return fontAwsome;
        }
    }}
