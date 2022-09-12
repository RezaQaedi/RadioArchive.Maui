namespace RadioArchive.Maui.ViewModels
{
    public partial class ShowItemViewModel : BaseViewModel
    {
        /// <summary>
        /// List of Shows
        /// </summary>
        [ObservableProperty, NotifyPropertyChangedFor(nameof(Date)), NotifyPropertyChangedFor(nameof(DisplayTitle))]
        private List<ShowViewModel> _shows = new();

        /// <summary>
        /// Indicates if this item is new 
        /// </summary>
        [ObservableProperty]
        private bool _isNew;

        /// <summary>
        /// Inidacates if one of this item shows is playing 
        /// </summary>
        [ObservableProperty]
        private bool _isPlaying;

        public DateTimeOffset Date
        {
            get
            {
                if (Shows.Count > 0)
                    return Shows[0].Date;

                return default;
            }
        }

        public new string DisplayTitle => Date.ToString("dddd");

        /// <summary>
        /// Color of background 
        /// </summary>
        [ObservableProperty]
        private Color _backgroundColor;

        /// <summary>
        ///  50% Lighter version of <see cref="BackgroundColor"/>
        /// </summary>
        [ObservableProperty]
        public Color _backgroundColorLighter;

        public ShowItemViewModel() { }

        public ShowItemViewModel(Color backgroundColor)
        {
            BackgroundColor = backgroundColor;
        }

        [RelayCommand]
        private async void Select()
        {
            var navParam = new Dictionary<string, object>
            {
                {"Items", Shows},
                {"Title", DisplayTitle},
            };

            // Show List 
            await Shell.Current.GoToAsync($"{nameof(ShowItemDetailPage)}", navParam);
        }
    }

    public partial class ShowItemViewModelGroup : List<ShowItemViewModel>
    {
        public string Title { get; private set; }
        public bool HasMoreContent { get; private set; }

        public ShowItemViewModelGroup(string title, bool hasMoreContent, List<ShowItemViewModel> showItemViewsModels) : base(showItemViewsModels)
        {
            Title = title;
            HasMoreContent = hasMoreContent;
        }

        [RelayCommand]
        private async void OpenList()
        {
            if (this.Any())
            {

                var month = this[0].Date.Month;
                var year = this[0].Date.Year;

                // Navigate to this month page  
                var navParam = new Dictionary<string, object>
                {
                    {"Year", year},
                    {"Month", month}
                };

                await Shell.Current.GoToAsync($"{nameof(ShowItemListDetailPage)}", navParam);
            }
        }
    }
}
