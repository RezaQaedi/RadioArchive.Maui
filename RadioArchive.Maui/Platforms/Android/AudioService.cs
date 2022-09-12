using Android.Media;
using RadioArchive.Maui.Platforms.Android.Services;

namespace RadioArchive.Maui.Platforms.Android;

public class AudioService : IAudioService
{
    MainActivity instance;

    public event Action MediaPrepard;
    public event Action MediaStarts;
    public event Action<int> PositionChaned;
    public event Action MediaStop;
    public event Action<bool> PlayingChanged;
    public event Action<float> SpeedRateChanged;

    private MediaPlayer _mediaPlayer => (instance != null &&
        instance.binder.GetMediaPlayerService() != null ) ?
        instance.binder.GetMediaPlayerService().mediaPlayer : null;

    private MediaPlayerService _mediaPlayerService => (instance != null &&
        instance.binder.GetMediaPlayerService() != null) ?
        instance.binder.GetMediaPlayerService() : null;

    public bool IsPlaying => _mediaPlayer?.IsPlaying ?? false;

    public double CurrentPosition => _mediaPlayer?.CurrentPosition ?? 0;

    public int Duration => _mediaPlayer?.Duration ?? 0;

    public float SpeedRate => _mediaPlayerService != null ? _mediaPlayerService.SpeedRate : 1;

    public async Task InitializeAsync(string audioURI, string _, string audioTitle, string audioSubtitle)
    {
        if (this.instance == null)
        {
            this.instance = MainActivity.instance;
            this.instance.MediaPrepared += (_, _) => MediaPrepard?.Invoke();
            this.instance.PositionChanged += (_, p) => PositionChaned?.Invoke(p);
            this.instance.MediaStarts += (_, _) => MediaStarts?.Invoke();
            this.instance.MediaStop += (_, _) => MediaStop?.Invoke();
            this.instance.PlayingChaned += (_, p) => PlayingChanged?.Invoke(p);
            this.instance.SpeedChanged += (_, s) => SpeedRateChanged?.Invoke(s);
        }
        else
        {
            await this.instance.binder.GetMediaPlayerService().Stop();
        }

        this.instance.binder.GetMediaPlayerService().AudioUrl = audioURI;
        this.instance.binder.GetMediaPlayerService().AudioTitle = audioTitle;
        this.instance.binder.GetMediaPlayerService().AudioSubTitle = audioSubtitle;

    }

    public Task PauseAsync()
    {
        if (IsPlaying)
        {
            return this.instance.binder.GetMediaPlayerService().Pause();
        }

        return Task.CompletedTask;
    }

    public async Task PlayAsync(double position = 0)
    {
        await this.instance.binder.GetMediaPlayerService().Play();
        await this.instance.binder.GetMediaPlayerService().Seek((int)position);
    }

    public async Task<bool> PlayAsync() => await this.instance.binder.GetMediaPlayerService().Play();

    public async Task Seek(int position)
    {
        if (_mediaPlayer != null)
            await instance.binder.GetMediaPlayerService().Seek(position);
    }

    public void SetSpeedRate(float speed)
    {
        if(_mediaPlayer != null)
            this.instance.binder.GetMediaPlayerService().SetPlaybackSpeed(speed);
    }

    public Task SetPosition(float position) => this.instance.binder.GetMediaPlayerService().SetPostion(position);
}
