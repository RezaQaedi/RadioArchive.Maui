using LibVLCSharp.Shared;

namespace RadioArchive.Maui.Platforms.Windows
{
    public class AudioService : IAudioService
    {
        /// <summary>
        /// Media object to control a media 
        /// </summary>
        private readonly MediaPlayer _mediaPlayer;
        private readonly LibVLC _vlc;
        private Timer _timer;

        private bool _mediaPrepard;

        public bool IsPlaying => _mediaPlayer.IsPlaying;

        public double CurrentPosition => _mediaPlayer.Time;

        public int Duration => (int)_mediaPlayer.Length;

        public float SpeedRate { get; private set; } = 1;

        public event Action MediaPrepard;
        public event Action<int> PositionChaned;
        public event Action MediaStarts;
        public event Action MediaStop;
        public event Action<bool> PlayingChanged;
        public event Action<float> SpeedRateChanged;

        public AudioService()
        {
            // Initialize vlclbr
            LibVLCSharp.Shared.Core.Initialize();
            _vlc = new LibVLC("--verbose=2");
            _mediaPlayer = new(_vlc);
            // Vlc events 
            //_mediaPlayer.TimeChanged += OnMediaTimeChanged;
            _mediaPlayer.LengthChanged += OnMediaLengthChanged;
            _mediaPlayer.Playing += OnMediaPlaying;
            _mediaPlayer.Paused += OnMediaPaused;
            _mediaPlayer.Stopped += OnMediaStopped;
            _vlc.Log += _vlc_Log;
        }

        private void _vlc_Log(object sender, LogEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[vlc][{e.Module}][{e.Level}]:{e.Message}");
        }

        private void OnMediaStopped(object sender, EventArgs e)
        {
            _timer?.Dispose();
        }

        private void OnMediaPaused(object sender, EventArgs e)
        {
            _timer?.Dispose();
            PlayingChanged?.Invoke(false);
        }

        private void OnTimerSignal(object state)
        {
            PositionChaned?.Invoke((int)_mediaPlayer.Time);
        }

        private void OnMediaPlaying(object sender, EventArgs e)
        {
            MediaStarts?.Invoke();
            PlayingChanged?.Invoke(true);
            _timer = new Timer(OnTimerSignal, new AutoResetEvent(false), 0, 1000);
        }

        private void OnMediaLengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            if (_mediaPrepard)
            {
                MediaPrepard?.Invoke();
                _mediaPrepard = false;
            }
        }

        public Task InitializeAsync(string audioURI, string referer, string title, string sub)
        {
            // Create media 
            _timer?.Dispose();
            using var media = new Media(_vlc, new Uri(audioURI));
            media.AddOption($":http-referrer={referer}");
            _mediaPlayer.Media = media;
            _mediaPrepard = true;

            return Task.CompletedTask;
        }

        public Task PauseAsync()
        {
            _mediaPlayer.Pause();
            return Task.CompletedTask;
        }

        public async Task PlayAsync(double position = 0)
        {
            await Seek((int)position);
            await PlayAsync();
        }

        public Task<bool> PlayAsync()
        {
            var succes = _mediaPlayer.Play();

            var tsc = new TaskCompletionSource<bool>();
            tsc.SetResult(succes);
            return tsc.Task;
        }

        public Task Seek(int position)
        {
            _mediaPlayer.SeekTo(TimeSpan.FromMilliseconds(position));
            return Task.CompletedTask;
        }

        public void SetSpeedRate(float speed)
        {
            _mediaPlayer.SetRate(speed);
            SpeedRate = speed;
            SpeedRateChanged?.Invoke(speed);
        }

        public Task SetPosition(float position)
        {
            _mediaPlayer.Position = position;
            return Task.CompletedTask;
        }
    }
}
