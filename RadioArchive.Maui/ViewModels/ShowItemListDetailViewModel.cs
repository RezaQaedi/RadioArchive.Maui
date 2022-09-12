namespace RadioArchive.Maui.ViewModels
{
    public partial class ShowItemListDetailViewModel : BaseViewModel, IQueryAttributable
    {
        readonly IApplicationApiService _apiService;
        readonly ApplicationStorgeService _storgeService;
        readonly PlayerService _mediaService;
        int _year;
        int _month;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(EmptyList))]
        ShowItemListViewModel _shows;

        [ObservableProperty]
        bool _failedToLoad;

        public ShowItemListDetailViewModel(IApplicationApiService applicationApiService, PlayerService playerService,ApplicationStorgeService storgeService)
        {
            _storgeService = storgeService;
            _apiService = applicationApiService;
            _mediaService = playerService;
        }

        public bool EmptyList => Shows is null;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (!query.Any())
                return;

            _year = Convert.ToInt32(query["Year"]);
            _month = Convert.ToInt32(query["Month"]);
            DisplayTitle = $"{TimeHelper.GetInvariantMonthName(_month)} ({_year})";
            Initials();
        }

        [RelayCommand]
        public async void GoBack() => await Shell.Current.GoToAsync("..");

        [RelayCommand]
        public async void Initials()
        {
            if(Shows is not null)
                if (Shows.Items.Any())
                    return;

            await RunCommand(() => IsBusy, async () => 
            {
                FailedToLoad = false;

                var showModels = 
                    await _apiService.GetShowsWithSpecificDateAsync(_year, _month);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (showModels is null)
                    {
                        FailedToLoad = true;
                        return;
                    }

                    Shows = new ShowItemListViewModel() { Items = showModels.GetViewModels(_mediaService, _storgeService) };
                });
            });
        }
    }
}
