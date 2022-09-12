namespace RadioArchive.Maui.Pages;

public partial class ShowItemListDetailPage : ContentPage
{
    readonly PlayerService _playerService;
    readonly ShowItemListDetailViewModel _viewModel;

	public ShowItemListDetailPage(ShowItemListDetailViewModel viewModel, PlayerService playerService)
    {
        InitializeComponent();
        // TODO : Move this to xaml in .Net7
        workLayer.ZIndex = 2;

        _viewModel = viewModel;
        _playerService = playerService;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_playerService.IsPlaying)
            if (Player.IsVisible == false)
                Player.IsVisible = true;

        Player.OnAppearing();
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Player.OnDisappearing();
    }
}