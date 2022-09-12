namespace RadioArchive.Maui.Pages;

public partial class YearDetailPage : ContentPage
{
	public YearDetailPage(YearDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}