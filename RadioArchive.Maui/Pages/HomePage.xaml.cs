namespace RadioArchive.Maui.Pages;
using Microsoft.Maui.Controls;
using System;

public partial class HomePage : ContentPage
{
    readonly HomeViewModel _viewModel;
    readonly PlayerService _playerService;

	public HomePage(HomeViewModel viewModel, PlayerService playerService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _playerService = playerService;
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Player.OnAppearing();

        // try to reload if we dont have items 
        if (_viewModel.EmptyList)
            _viewModel.Initials();

        // Show mini-player if not visible already and we had something playing or paused 
        if (!Player.IsVisible && _playerService.CurrentShow != null)
            Player.IsVisible = true;

        // Update ShowItem and Shows status
        _viewModel.UpdateItemsState();
        _viewModel.UpdateUserRecentVisitedShows();

        // Hook to player service for updateing shows status 
        _playerService.IsPlayingChanged += OnPlayingChanged;
        _playerService.PositionChanged += OnMediaPositionChanged;
        _playerService.MediaStop += OnMediaStop;

        // Update Play pause as page appear
        UpdatePlayPause(_playerService.IsPlaying);
    }

    private void OnMediaStop(object sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Player.IsVisible = false;
        });
    }

    private void OnPlayingChanged(object sender, bool e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Update Shows Playing or pause status 
            UpdatePlayPause(e);

            // Show mini-player if not visible already
            if (!Player.IsVisible)
                Player.IsVisible = true;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Player.OnDisappearing();

        _playerService.IsPlayingChanged -= OnPlayingChanged;
        _playerService.PositionChanged -= OnMediaPositionChanged;
    }

    private void UpdatePlayPause(bool isPlaying)
    {
        foreach (var show in _viewModel.RecentHistoryShows)
        {
            if (show.Equals(_playerService.CurrentShow))
            {
                show.IsPlaying = isPlaying;
            }
            else
                show.IsPlaying = false;
        }
    }

    private void OnMediaPositionChanged(object sender, int p)
    {
        if (_playerService.Duration == 0)
            return;

        foreach (var show in _viewModel.RecentHistoryShows)
        {
            if (show.Equals(_playerService.CurrentShow))
            {
                var position = p * 100 / _playerService.Duration;
                show.Proggress = (float)position / 100;
            }
        }
    }
}