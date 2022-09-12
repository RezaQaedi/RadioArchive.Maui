namespace RadioArchive.Maui.ViewModels
{
    public partial class ShowViewModel : BaseViewModel
    {
        #region Private methods
        readonly PlayerService _mediaService;
        readonly ApplicationStorgeService _storgeService;

        /// <summary>
        /// Get or set podcast time 
        /// </summary>
        [ObservableProperty]
        ShowTime _time;

        /// <summary>
        /// List of note this item have 
        /// </summary>
        [ObservableProperty]
        ObservableCollection<ShowNoteViewModel> _notes;

        /// <summary>
        /// Indicate's this podcast is replay show 
        /// </summary>
        [ObservableProperty]
        bool _isReplay;

        /// <summary>
        /// Inidcates if this podcast is liked before 
        /// </summary>
        [ObservableProperty]
        bool _isLiked;

        /// <summary>
        /// Indicates if this podcast is playing now 
        /// </summary>
        [ObservableProperty]
        bool _isPlaying;

        /// <summary>
        /// Indicated if this item is removble 
        /// </summary>
        [ObservableProperty]
        bool _isRemoveble;

        /// <summary>
        /// Indicates if note shoud be visible 
        /// </summary>
        [ObservableProperty]
        bool _noteVisible;

        /// <summary>
        /// Proggress of watching this item between  0 and 1 
        /// </summary>
        [ObservableProperty]
        float _proggress;

        /// <summary>
        /// User written input 
        /// </summary>
        [ObservableProperty]
        string _notesInput;

        /// <summary>
        /// Color of background 
        /// </summary>
        [ObservableProperty, NotifyPropertyChangedFor(nameof(BackgroundColorLighter))]
        Color _backgroundColor;

        /// <summary>
        /// The date of relase 
        /// </summary>
        [ObservableProperty, NotifyPropertyChangedFor(nameof(NoSpecificDate)), NotifyPropertyChangedFor(nameof(DisplayTitle))]
        DateTimeOffset _date;

        #endregion

        #region Public properties

        /// <summary>
        /// Get's if this podcast dosnt have any specific date 
        /// </summary>
        public bool NoSpecificDate => _time == ShowTime.None;

        /// <summary>
        /// Used to show as title in main UI
        /// </summary>
        public new string DisplayTitle => _date.ToString("dddd");

        /// <summary>
        /// User friendly time 
        /// </summary>
        public string DisplayTime => Time == ShowTime.None ? "" : Time.ToString();

        /// <summary>
        /// User friendly date 
        /// </summary>
        public string DisplayDate => Date.ToString("ddd  yyy/MM/dd");

        /// <summary>
        ///  50% Lighter version of <see cref="_backgroundColor"/>
        /// </summary>
        public Color BackgroundColorLighter => BackgroundColor.Lerp(Colors.White, 0.5f);
        #endregion

        #region Counstractor

        public ShowViewModel() { }

        public ShowViewModel(DateTimeOffset date, ShowTime podcastTime, Color color, PlayerService mediaService, ApplicationStorgeService storgeService, bool isReplay = false, bool isRemovble = false)
        {
            Time = podcastTime;
            Date = date;
            IsReplay = isReplay;
            BackgroundColor = color;
            IsRemoveble = isRemovble;
            _mediaService = mediaService;
            _storgeService = storgeService;
        }

        #endregion

        #region Public Mthods 

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ShowViewModel vm)
                return Date == vm.Date && Time == vm.Time;
            if (obj is ShowDataModel dataModel)
                return Date == dataModel.Date && Time == dataModel.Time;

            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Time);
        }
        #endregion

        #region Private methods 

        [RelayCommand]
        async void Remove()
        {
            var title = (Shell.Current.CurrentPage.BindingContext as BaseViewModel)?.DisplayTitle;

            var respond = await Shell.Current.DisplayAlert($"Remove from {title}", "are you sure?", "yes", "no");

            if (respond)
                _storgeService.RemoveShowFromPlayList(this, title);
        }

        [RelayCommand]
        private async void Play()
        {
            await _mediaService.PlayAsync(this);
        }

        [RelayCommand]
        private void ShowNote() => NoteVisible = !NoteVisible;

        /// <summary>
        /// Updates this podcast like status 
        /// </summary>
        [RelayCommand]
        private async void ChangeLike()
        {

            IsLiked = !IsLiked;

            await RunCommand(() => IsBusy, async () =>
            {
                await Task.Run(() => _storgeService.UpdateLike(this));
            });
        }

        /// <summary>
        /// Add this podcast to specefic user play list 
        /// </summary>
        /// <param name="listKey">key to specify user play list</param>
        private async void AddThisToUsePlayList(string listKey)
        {
            await RunCommand(() => IsBusy, async () =>
            {
                await Task.Run(() => _storgeService.AddShow(this, listKey));
            });
        }

        [RelayCommand]
        private async void GoToPlayerDetailPage()
        {
            if (!Equals(_mediaService.CurrentShow))
                await _mediaService.PlayAsync(this);

            await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync($"//{nameof(PlayerDetailPage)}");
        }
        #endregion
    }
}
