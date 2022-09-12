namespace RadioArchive.Maui;

public partial class PlayerView : ContentView
{
    PlayerViewModel _viewModel;

    public PlayerView()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (_viewModel == null)
        {
            this._viewModel = this.Handler.MauiContext.Services.GetService<PlayerViewModel>();
            this.BindingContext = _viewModel;
            _viewModel.Initlize();
        }
    }

    internal void OnAppearing()
    {
        if (this._viewModel != null)
        {
            if(this.BindingContext == null)
                this.BindingContext = this._viewModel;

            _viewModel.Initlize();
        }
    }

    internal void OnDisappearing()
    {
        this.BindingContext = null;
    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        var slider = sender as Slider;

        if (slider is not null)
        {
            _viewModel.SetPosition(slider.Value);
        }
    }
}