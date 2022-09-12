using AndroidX.Core.App;

namespace RadioArchive.Maui.Platforms.Android
{
    public class ShareService : IShareService
    {
        public void ShareText(string text, string title)
        {
            var builder = new ShareCompat.IntentBuilder(MainActivity.instance)
                .SetType("text/plain")
                .SetChooserTitle(title)
                .SetText(text);

            builder.StartChooser();
        }
    }
}
