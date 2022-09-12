using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using RadioArchive.Maui.Platforms.Android.Services;

namespace RadioArchive.Maui;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static MainActivity instance;
    public MediaPlayerServiceBinder binder;
    MediaPlayerServiceConnection mediaPlayerServiceConnection;

    public event StatusChangedEventHandler StatusChanged;

    public event CoverReloadedEventHandler CoverReloaded;

    public event PlayingEventHandler Playing;
    public event PlayingChangedEventHandler PlayingChaned;

    public event BufferingEventHandler Buffering;

    public event EventHandler MediaPrepared;
    public event EventHandler<int> PositionChanged;
    public event EventHandler MediaStarts;
    public event EventHandler MediaStop;
    public event EventHandler<float> SpeedChanged;


    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        instance = this;
        NotificationHelper.CreateNotificationChannel(ApplicationContext);
        if (mediaPlayerServiceConnection == null)
            InitializeMedia();
    }

    private void InitializeMedia()
    {
        mediaPlayerServiceConnection = new MediaPlayerServiceConnection(this);
        // Bind Media service 
        var mediaPlayerServiceIntent = new Intent(ApplicationContext, typeof(MediaPlayerService));
        BindService(mediaPlayerServiceIntent, mediaPlayerServiceConnection, Bind.AutoCreate);

        // starting a closing service 
        //var closingServiceIntent = new Intent(ApplicationContext, typeof(ClosingService));
        //StartService(closingServiceIntent);
    }

    public override void OnBackPressed()
    {
        var navStack = Shell.Current.Navigation.NavigationStack;
        var modalStack = Shell.Current.Navigation.ModalStack;

        if (navStack[navStack.Count - 1] != null || modalStack.Any())
        {
            base.OnBackPressed();
        }
        else
        {
            MoveTaskToBack(true);
        }
    }

    class MediaPlayerServiceConnection : Java.Lang.Object, IServiceConnection
    {
        readonly MainActivity instance;

        public MediaPlayerServiceConnection(MainActivity mediaPlayer)
        {
            this.instance = mediaPlayer;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            if (service is MediaPlayerServiceBinder binder)
            {
                instance.binder = binder;

                var mediaPlayerService = binder.GetMediaPlayerService();
                mediaPlayerService.CoverReloaded += (object sender, EventArgs e) => { instance.CoverReloaded?.Invoke(sender, e); };
                mediaPlayerService.StatusChanged += (object sender, EventArgs e) => { instance.StatusChanged?.Invoke(sender, e); };
                mediaPlayerService.Playing += (object sender, EventArgs e) => { instance.Playing?.Invoke(sender, e); };
                mediaPlayerService.Buffering += (object sender, EventArgs e) => { instance.Buffering?.Invoke(sender, e); };
                mediaPlayerService.MediaPrepared += (object sender, EventArgs e) => { instance.MediaPrepared?.Invoke(sender, e); };
                mediaPlayerService.PositionChanged += (object sender, int e) => { instance.PositionChanged?.Invoke(sender, e); };
                mediaPlayerService.MediaStarts += (object sender, EventArgs e) => { instance.MediaStarts?.Invoke(sender, e); };
                mediaPlayerService.MediaStop += (object sender, EventArgs e) => { instance.MediaStop?.Invoke(sender, e); };
                mediaPlayerService.PlayingChanged += (object sender, bool e) => { instance.PlayingChaned?.Invoke(sender, e); };
                mediaPlayerService.SpeedRateChanged += (object sender, float s) => { instance.SpeedChanged?.Invoke(sender, s); };
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
        }
    }
}
