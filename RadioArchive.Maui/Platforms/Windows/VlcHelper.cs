using LibVLCSharp.Shared;

namespace RadioArchive.Maui
{
    public static class VlcHelper
    {
        public static Media GetNewMedia(this LibVLC libVLC,DateTimeOffset dateTime, ShowTime showTime,params VLCMediaOptions[] vLCMediaOptions)
        {
            RouteHelper.GetUrlForSpeceficDate(dateTime, showTime, out var url, out var urlReferre);

            var media = new Media(libVLC, new Uri(url));
            //media.AddOption(":http-user-agent='Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:99.0) Gecko/20100101 Firefox/99.0'");

            foreach (var options in vLCMediaOptions)
            {
                switch (options)
                {
                    case VLCMediaOptions.Keep:
                        media.AddOption(":sout-keep");
                        break;
                    case VLCMediaOptions.File:
                        media.AddOption(":sout=#file{dst=" + GetDestenation(dateTime, showTime) + "}");
                        break;
                    case VLCMediaOptions.Referrer:
                        media.AddOption($":http-referrer={urlReferre}");
                        break;
                }
            }

            return media;
        }

        private static string GetDestenation(DateTimeOffset dateTime, ShowTime showTime)
        {
            var currentDirectory = Path.GetDirectoryName(AppContext.BaseDirectory);

            // TODO : catch exeption when failing to create directory
            try
            {
                if (!Directory.Exists(Path.Combine(currentDirectory, "Records")))
                    Directory.CreateDirectory(Path.Combine(currentDirectory, "Records"));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debugger.Log(0, "Error", $"Faild to make records directory with error {e.Message}");
            }

            return Path.Combine($"{currentDirectory}\\Records", $"{dateTime:yyy_MM_dd}_{showTime}_{DateTime.UtcNow.Ticks}.mp3");
        }
    }
}
