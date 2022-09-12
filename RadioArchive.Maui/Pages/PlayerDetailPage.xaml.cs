namespace RadioArchive.Maui.Pages;

using RadioArchive.Maui.Views;

public partial class PlayerDetailPage : ContentPage
{
    readonly PlayerViewModel _viewModel;

	public PlayerDetailPage(PlayerViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        var slider = sender as Slider;

        if(slider is not null)
            _viewModel.SetPosition(slider.Value);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initlize();
    }
}