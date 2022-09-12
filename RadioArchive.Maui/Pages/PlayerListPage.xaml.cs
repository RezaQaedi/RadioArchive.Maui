namespace RadioArchive.Maui.Pages;

public partial class PlayerListPage : ContentPage
{
	public PlayerListPage(UserPlayListViewModel viewModel)
	{
        this.BindingContext = viewModel;
		InitializeComponent();
	}
}