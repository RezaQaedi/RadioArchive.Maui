namespace RadioArchive.Maui.Pages;

public partial class LastShowsPage : ContentPage
{
    public LastShowsPage(LastShowViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}

