namespace RadioArchive.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            MainPage = new DesktopShell();
        else
            MainPage = new MobileShell();

        Routing.RegisterRoute(nameof(ShowItemDetailPage), typeof(ShowItemDetailPage));
        Routing.RegisterRoute(nameof(ShowItemListDetailPage), typeof(ShowItemListDetailPage));
        Routing.RegisterRoute(nameof(YearDetailPage), typeof(YearDetailPage));
    }
}
