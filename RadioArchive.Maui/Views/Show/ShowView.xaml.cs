namespace RadioArchive.Maui.Views;

public partial class ShowView : ContentView
{
	public ShowView()
	{
		InitializeComponent();

		// Todo : Move this to xaml in .Net7
		detailContainer.ZIndex = 1;
		workingLayer.ZIndex = 4;
		playButtonContainer.ZIndex = 2;
		likeButton.ZIndex = 2;
		showDetail.ZIndex = 3;
	}
}