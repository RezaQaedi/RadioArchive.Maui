namespace RadioArchive.Maui.Services;

public interface IAudioService
{
    Task InitializeAsync(string audioURI, string referer, string audioTitle, string audioSubtitle);
    Task PlayAsync(double position = 0);
    Task<bool> PlayAsync();
    Task PauseAsync();
    Task Seek(int position);
    Task SetPosition(float position);

    void SetSpeedRate(float speed);

    event Action MediaPrepard;
    event Action MediaStarts;
    event Action MediaStop;
    event Action<int> PositionChaned;
    event Action<bool> PlayingChanged;
    event Action<float> SpeedRateChanged;

    bool IsPlaying { get; }
    double CurrentPosition { get; }
    int Duration { get; }
    float SpeedRate { get; }
}
