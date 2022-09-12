using System.Globalization;

namespace RadioArchive.Maui
{
    public static class TimeHelper
    {
        /// <summary>
        /// Gets name of the month 
        /// </summary>
        /// <param name="month">number value</param>
        /// <returns></returns>
        public static string GetInvariantMonthName(int month)
        {
            return CultureInfo.InvariantCulture.
                DateTimeFormat.GetMonthName
                (month);
        }
    }
}
