using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace RadioArchive.Maui.Platforms.Android.Services
{
    [Service(Exported = false, Enabled = false, Name = "com.RadioArchive.ClosingService")]
    public class ClosingService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            System.Diagnostics.Debug.WriteLine("Close Service Inisailed!");
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnTaskRemoved(Intent rootIntent)
        {
            System.Diagnostics.Debug.WriteLine("OnTask Remove Called. Canceling all notifications!");
            base.OnTaskRemoved(rootIntent);
            NotificationHelper.StopNotification(ApplicationContext);

            StopSelf();
        }
    }
}
