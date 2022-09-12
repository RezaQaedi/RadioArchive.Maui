namespace RadioArchive.Maui;
using CommunityToolkit.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("fa-solid-900.ttf", "FontAwsomeSolid");
                fonts.AddFont("fontawesome-webfont.ttf", "FontAwsome");
                fonts.AddFont("PTSansCaption-Bold.ttf", "PTSansCaptionBold");
                fonts.AddFont("PTSansCaption-Regular.ttf", "PTSansCaptionRegular");
            })
            .UseMauiCommunityToolkit()
            .AddClientDataStore()
            .Services
            .AddSingleton<IApplicationApiService, ApplicationApiService>()
            .AddSingleton<ApplicationStorgeService>()
#if ANDROID
            .AddTransient<IShareService, Platforms.Android.ShareService>()
            .AddSingleton<IAudioService, Platforms.Android.AudioService>()
#endif

#if WINDOWS
            .AddTransient<IShareService, Platforms.Windows.ShareService>()
            .AddSingleton<IAudioService, Platforms.Windows.AudioService>()
#endif
            .AddSingleton<PlayerService>()
            .AddSingleton<WifiOptionsService>()
            .AddSingleton<PlayerViewModel>()
            .AddSingleton<PlayerDetailPage>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<HomePage>()
            .AddSingleton<LastShowsPage>()
            .AddSingleton<LastShowViewModel>()
            .AddSingleton<PlayerListPage>()
            .AddSingleton<UserPlayListViewModel>()
            .AddTransient<ShowItemDetailViewModel>()
            .AddTransient<ShowItemDetailPage>()
            .AddTransient<ShowItemListDetailViewModel>()
            .AddTransient<ShowItemListDetailPage>()
            .AddTransient<YearDetailPage>()
            .AddTransient<YearDetailViewModel>();

        return builder.Build();
	}
}
