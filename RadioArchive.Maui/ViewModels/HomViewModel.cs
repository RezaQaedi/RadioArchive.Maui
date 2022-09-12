namespace RadioArchive.Maui.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        readonly ApplicationStorgeService _storgeService;
        readonly IApplicationApiService _applicationApiService;
        private readonly PlayerService _mediaService;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(EmptyList))]
        ObservableCollection<ShowItemViewModelGroup> _shows = new();

        [ObservableProperty, NotifyPropertyChangedFor(nameof(RecentHistoryAny))]
        ObservableCollection<ShowViewModel> _recentHistoryShows = new();

        [ObservableProperty]
        bool _failedToload;

        public bool EmptyList => Shows.Any() is false;
        public bool RecentHistoryAny => RecentHistoryShows.Any();

        public HomeViewModel(IApplicationApiService applicationApiService, PlayerService mediaService, ApplicationStorgeService storgeService)
        {
            _mediaService = mediaService;

            // Get services 
            _applicationApiService = applicationApiService;
            _storgeService = storgeService;
            _mediaService.IsPlayingChanged += OnPlayingChanged;
        }

        private void OnPlayingChanged(object sender, bool isPlaying)
        {
            // Update isPlayig 
            foreach (var showGroup in Shows)
                foreach (var ShowItem in showGroup)
                    ShowItem.IsPlaying = ShowItem.Shows.Any(s => s.Equals(_mediaService.CurrentShow));
        }

        public void UpdateItemsState()
        {
            var newstWatchedShow = _storgeService.GetVisitedShows().OrderBy(s => s.Date).LastOrDefault();
            // Update isNew 
            foreach (var showGroup in Shows)
                foreach (var ShowItem in showGroup)
                    ShowItem.IsNew = ShowItem.Shows.Any(s => newstWatchedShow?.Date < s.Date);
        }

        public void UpdateUserRecentVisitedShows()
        {
            var history = _storgeService.GetVisitedShows()?.TakeLast(2).OrderByDescending(s => s.UserProggresion);

            if (history.Any())
            {
                var shows = new ObservableCollection<ShowViewModel>();

                foreach (var showData in history)
                    shows.Add(showData.ToViewModel(_storgeService, _mediaService));

                RecentHistoryShows = shows;
            }
        }

        [RelayCommand]
        public async void Initials()
        {
            FailedToload = false;
            List<ShowApiModel> topRatedModels = null;
            List<ShowApiModel> mostRecentModels = null;

            await RunCommand(() => IsBusy, async () =>
            {
                topRatedModels =
                await _applicationApiService.GetTopRatedShowsAsync();

                mostRecentModels =
                await _applicationApiService.GetLastShowsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (topRatedModels is null || mostRecentModels is null)
                    {
                        FailedToload = true;
                        return;
                    }

                    Shows = new()
                    {
                        new ShowItemViewModelGroup("Most recent shows", true, mostRecentModels.GetViewModels(_mediaService, _storgeService)),
                        new ShowItemViewModelGroup("Top rated shows", false, topRatedModels.GetViewModels(_mediaService, _storgeService))                        
                    };

                    UpdateItemsState();
                });
            });
        }

    }
}
