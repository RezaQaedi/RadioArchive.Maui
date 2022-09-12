using System.Globalization;

namespace RadioArchive.Maui
{
    public class AllTrueMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || !targetType.IsAssignableFrom(typeof(bool)))
            {
                return false;
                // Alternatively, return BindableProperty.UnsetValue to use the binding FallbackValue
            }

            var match = true;
            // if we had parameter just reverce the boolean 
            if (parameter is not null)
                match = false;

            foreach (var value in values)
            {
                if (value is not bool b)
                {
                    return false;
                    // Alternatively, return BindableProperty.UnsetValue to use the binding FallbackValue
                }
                else if (b != match)
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is not bool b || targetTypes.Any(t => !t.IsAssignableFrom(typeof(bool))))
            {
                // Return null to indicate conversion back is not possible
                return null;
            }

            if (b)
            {
                return targetTypes.Select(t => (object)true).ToArray();
            }
            else
            {
                // Can't convert back from false because of ambiguity
                return null;
            }
        }
    }
}
