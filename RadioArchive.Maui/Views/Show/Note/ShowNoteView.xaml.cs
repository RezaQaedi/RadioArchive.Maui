namespace RadioArchive.Maui.Views;

public partial class ShowNoteView : ContentView
{
	public ShowNoteView()
	{
		InitializeComponent();
		// Move this to xaml in .Net7
		workinLayer.ZIndex = 3;
	}
}