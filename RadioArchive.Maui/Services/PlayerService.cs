namespace RadioArchive.Maui.Services;

using CommunityToolkit.Maui.Alerts;
using RadioArchive.Maui.Views;

public class PlayerService
{
    private readonly IAudioService _audioService;
    private readonly WifiOptionsService _wifiOptionsService;
    private readonly ApplicationStorgeService _storgeService;
    private float _startPostion = 0;
    /// <summary>
    /// Inidcates if we are trying to play already 
    /// </summary>
    private bool _playigPending; 

    public ShowViewModel CurrentShow { get; set; }
    public bool IsPlaying { get; set; }
    public double CurrentPosition => _audioService.CurrentPosition;
    public int Duration => _audioService.Duration;
    public float SpeedRate => _audioService.SpeedRate;

    public event EventHandler<bool> IsPlayingChanged;
    public event EventHandler<int> PositionChanged;
    public event EventHandler MediaPrepard;
    public event EventHandler<ShowViewModel> NewMediaOpend;
    public event EventHandler MediaStop;
    public event EventHandler<float> SpeedRateChanged;

    public PlayerService(IAudioService audioService, WifiOptionsService wifiOptionsService, ApplicationStorgeService storgeService)
    {
        _audioService = audioService;
        _wifiOptionsService = wifiOptionsService;
        _storgeService = storgeService;

        audioService.PositionChaned += p => PositionChanged?.Invoke(this, p);
        audioService.MediaPrepard += () => MediaPrepard?.Invoke(this, EventArgs.Empty);
        audioService.MediaStarts += OnMediaStarts;
        audioService.MediaStop += OnMediaStop;
        audioService.PlayingChanged += p =>
        {
            IsPlayingChanged?.Invoke(this, p);
            IsPlaying = p;

            // Update proggres when media puased 
            if (p == false)
                UpdateProggress();
        };
        audioService.SpeedRateChanged += s => SpeedRateChanged?.Invoke(this, s);
    }

    private void OnMediaStop()
    {
        UpdateProggress();
        CurrentShow = null;
        MediaStop?.Invoke(this, EventArgs.Empty);
    }

    private void OnMediaStarts()
    {
        if (_startPostion > 0.1f)
        {
            MainThread.BeginInvokeOnMainThread(async () => 
            {
                var respond = await Shell.Current.DisplayAlert("Resume", "Do you want to resume from last visit?", "Yes", "No");

                if (respond)
                    await _audioService.SetPosition(_startPostion);

                _startPostion = 0;
            });
        }
    }

    private async Task PlayAsync(ShowViewModel show, bool isPlaying)
    {
        if (show is null)
            return;

        if (show.Equals(CurrentShow))
        {
            if (isPlaying)
            {
                await InternalPauseAsync();
            }
            else
                await InternalPlayAsync(show);
        }
        else
        {
            // Update proggress on new show playing
            UpdateProggress();
            await InternalPlayAsync(show, true);
        }
    }

    public Task PlayAsync(ShowViewModel show)
    {
        var isPlaying = _audioService.IsPlaying;

        return PlayAsync(show, isPlaying);
    }

    private void UpdateProggress()
    {
        if (CurrentShow != null && Duration != 0)
        {
            var position = CurrentPosition * 100 / Duration;
            CurrentShow.Proggress = (float)position / 100;
            _storgeService.UpdateProggress(CurrentShow);
        }
        else if(Duration == 0)
            System.Diagnostics.Debug.WriteLine($"Coulden't get Duration ?");
    }

    private void SetCurrentShow(ShowViewModel showView)
    {
        CurrentShow = showView;
        _startPostion = CurrentShow.Proggress;
        _storgeService.UpdateVisitDate(CurrentShow);
    }

    public async Task SeekTo(int position) => await _audioService.Seek(position);

    public void SetSpeed(float playbackSpeed) => _audioService.SetSpeedRate(playbackSpeed);

    private async Task InternalPauseAsync()
    {
        await _audioService.PauseAsync();
    }

    private async Task InternalPlayAsync(ShowViewModel show, bool initializePlayer = false)
    {
        if(_playigPending)
        {
            var alert = Toast.Make("Please wait , media is preparing");
            await alert.Show();
            return;
        }

        _playigPending = true;
        var hasWifi = await _wifiOptionsService.HasWifiOrCanPlayWithOutWifiAsync();

        if (!hasWifi)
        {
            return;
        }

        if (initializePlayer)
        {
            RouteHelper.GetUrlForSpeceficDate(show.Date, show.Time, out var url, out var referer);
            await _audioService.InitializeAsync(url, referer, show.DisplayDate, show.DisplayTime);
            SetCurrentShow(show);
        }

        var result = await _audioService.PlayAsync();

        if(!result)
        {
            await Shell.Current.DisplayAlert("Failed to play the meida", "Make sure you have internet conncetion", "ok");
            _playigPending = false;
            return;
        }

        if(initializePlayer)
            NewMediaOpend?.Invoke(this, CurrentShow);

        _playigPending = false;
    }
}
