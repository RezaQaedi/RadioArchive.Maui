using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace RadioArchive.Maui
{
    /// <summary>
    /// Converter takes boolean and inverse it 
    /// </summary>
    public class InverseBooleanConverter : BaseConverter<bool, bool>
    {
        public override bool ConvertFrom(bool value, CultureInfo culture) => !value;

        public override bool ConvertBackTo(bool value, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
