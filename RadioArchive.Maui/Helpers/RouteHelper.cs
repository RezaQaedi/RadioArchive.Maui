using System.Globalization;

namespace RadioArchive.Maui
{
    public static class RouteHelper
    {
        private const string HOST = "https://www.holakoueearchive.co";
        private const string GETFILE = "mp3/slave/w_";
        private const string GETFILEREFERRER = "listen-to-show.php?&str=";
        private const string FILEPAGE = "?item=";

        public static void GetUrlForSpeceficDate(DateTimeOffset date, ShowTime time, out string fileUrl, out string fileUrlReferrer)
        {
            var strDate = date.ToString("yyyyddMM", CultureInfo.InvariantCulture);
            var strDateMonth = date.ToString("MMMM", CultureInfo.InvariantCulture);

            var timeStr = "";

            switch (time)
            {
                case ShowTime.Evening:
                    timeStr = "e";
                    break;
                case ShowTime.Morning:
                    timeStr = "m";
                    break;
                case ShowTime.Afternoon:
                    timeStr = "a";
                    break;
            }

            fileUrl = $"{GetAbsoluteRoute(GETFILE)}{strDate}{timeStr}.mp3";
            fileUrlReferrer = $"{GetAbsoluteRoute(GETFILEREFERRER)}{strDate}{timeStr}&f={strDateMonth}";
        }

        /// <summary>
        /// Get speceifc show page url 
        /// </summary>
        /// <param name="date">Date of show</param>
        /// <param name="time">Time of day</param>
        /// <returns></returns>
        public static string GetShowPage(DateTimeOffset date, ShowTime time)
        {
            var urlDate = date.ToString("MMMM-d-yyyy", CultureInfo.InvariantCulture);

            var timeStr = "";

            if(time != ShowTime.None)
                timeStr = $"-{time}";

            return $"{GetAbsoluteRoute(FILEPAGE)}{urlDate}{timeStr}";
        }

        /// <summary>
        /// Converts a relative URL into an absolute URL
        /// </summary>
        /// <param name="relativeUrl">The relative URL</param>
        /// <returns>Returns the absolute URL including the Host URL</returns>
        public static string GetAbsoluteRoute(string relativeUrl)
        {
            // Get the host
            var host = HOST;

            // If they are not passing any URL...
            if (string.IsNullOrEmpty(relativeUrl))
                // Return the host
                return host;

            // If the relative URL does not start with /...
            if (!relativeUrl.StartsWith("/"))
                // Add the /
                relativeUrl = $"/{relativeUrl}";

            // Return combined URL
            return host + relativeUrl;
        }
    }
}
