
namespace RadioArchive.Maui;

public partial class ShowItemDetailPage : ContentPage
{
    readonly ShowItemDetailViewModel _viewModel;
    readonly PlayerService _playerService;
    readonly ApplicationStorgeService _storgeService;

    public ShowItemDetailPage(ShowItemDetailViewModel viewModel, PlayerService playerService, ApplicationStorgeService storgeService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _playerService = playerService;
        _storgeService = storgeService;
    }

    protected override void OnAppearing()
    {
        UpdatePlayPause();
        _playerService.PositionChanged += OnMediaPositionChanged;
        _playerService.IsPlayingChanged += OnMediaIsPlayingChanged;
        _storgeService.ShowRemoved += OnShowRemoved;
    }


    protected override void OnDisappearing()
    {
        _playerService.PositionChanged -= OnMediaPositionChanged;
        _playerService.IsPlayingChanged -= OnMediaIsPlayingChanged;
        _storgeService.ShowRemoved -= OnShowRemoved;
    }
    private void OnShowRemoved(ShowViewModel show)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.Shows.Contains(show))
                _viewModel.Shows.Remove(show);
        });
    }

    private void OnMediaIsPlayingChanged(object sender, bool _)
    {
        UpdatePlayPause();
    }

    private void UpdatePlayPause()
    {
        foreach (var show in _viewModel.Shows)
        {
            if (show.Equals(_playerService.CurrentShow))
            {
                show.IsPlaying = _playerService.IsPlaying;
            }
            else
                show.IsPlaying = false;
        }
    }

    private void OnMediaPositionChanged(object sender, int p)
    {
        if (_playerService.Duration == 0)
            return;

        foreach (var show in _viewModel.Shows)
        {
            if (show.Equals(_playerService.CurrentShow))
            {
                var position = p * 100 / _playerService.Duration;
                show.Proggress = (float)position / 100;
            }
        }
    }
}