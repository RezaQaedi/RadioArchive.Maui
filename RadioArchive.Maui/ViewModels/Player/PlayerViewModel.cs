namespace RadioArchive.Maui.ViewModels
{
    public partial class PlayerViewModel : BaseViewModel
    {
        private readonly PlayerService _playerService;
        private readonly ApplicationStorgeService _storgeService;
        private readonly IShareService _shareService;

        /// <summary>
        /// Subttile of player 
        /// </summary>
        [ObservableProperty]
        string _subtitle;

        /// <summary>
        /// Duration of this media 
        /// </summary>
        [ObservableProperty]
        TimeSpan _duration;

        /// <summary>
        /// Current proggres time of media 
        /// </summary>
        [ObservableProperty]
        TimeSpan _currrentTime;

        /// <summary>
        /// Postion of current media between 0 and 100
        /// </summary>
        [ObservableProperty]
        double _position;

        /// <summary>
        /// Proggress of current media between 0 and 1
        /// </summary>
        [ObservableProperty]
        double _proggress;

        /// <summary>
        /// Inidcates if user is adding new note 
        /// </summary>
        [ObservableProperty]
        bool _isAddingNote;

        /// <summary>
        /// Indicates if media is playing 
        /// </summary>
        [ObservableProperty]
        bool _isPlaying;

        [ObservableProperty]
        ObservableCollection<ShowNoteViewModel> _notes = new();

        [ObservableProperty]
        string _speedRate = "Normal";

        public PlayerViewModel(PlayerService playerService,ApplicationStorgeService storgeService,IShareService shareService)
        {
            _playerService = playerService;
            _storgeService = storgeService;
            _shareService = shareService;

            _playerService.IsPlayingChanged += OnIsPlayingChanged;
            _playerService.MediaPrepard += OnMediaPrepard;
            _playerService.PositionChanged += OnPositionChanged;
            _playerService.NewMediaOpend += OnNewMediaOpend;
            _playerService.MediaStop += OnMediaStop;
            _storgeService.NoteRemoved += OnNoteRemoved;
            _playerService.SpeedRateChanged += (o, s) =>
            {
                var stringSpeed = "Normal";

                if(s != 1)
                    stringSpeed = $"x{s}";                

                SpeedRate = stringSpeed;
            };
            
        }

        private void OnMediaStop(object sender, EventArgs e)
        {
            SetDefaultStats();
        }

        private void SetDefaultStats()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DisplayTitle = string.Empty;
                Subtitle = string.Empty;
                CurrrentTime = TimeSpan.FromSeconds(1);
                Duration = TimeSpan.FromSeconds(1);
                Proggress = 0;
                Position = 0;
            });
        }

        private void OnNoteRemoved(ShowNoteViewModel noteVM)
        {
            MainThread.BeginInvokeOnMainThread(() => { 
            if(Notes.Contains(noteVM))
                Notes.Remove(noteVM);
            });
        }

        private void OnNewMediaOpend(object sender, ShowViewModel e)
        {
            Notes = new();
            var notes = _storgeService.GetUserNotes(e);
            foreach (var note in notes) Notes.Add(note.ToViewModel(_storgeService, _playerService));
        }

        private void OnMediaPrepard(object sender, EventArgs e)
        {
            Initlize();
        }

        public void Initlize()
        {
            if (_playerService.CurrentShow is null)
                return;

            // Update propertys 
            DisplayTitle = _playerService.CurrentShow.DisplayTime;
            DisplayTitle += _playerService.CurrentShow.IsReplay ? " (Best of the week) " : "";
            Subtitle = _playerService.CurrentShow.DisplayDate;
            Duration = TimeSpan.FromMilliseconds(_playerService.Duration);
            IsPlaying = _playerService.IsPlaying;
        }

        private void OnIsPlayingChanged(object sender, bool isPlaying)
        {
            IsPlaying = isPlaying;
        }

        private void OnPositionChanged(object sender, int position)
        {
            if (_playerService.Duration == 0)
                return;

            MainThread.BeginInvokeOnMainThread(() => {
                CurrrentTime = TimeSpan.FromMilliseconds(position);
                // Update player 
                Position = position * 100 / _playerService.Duration;
                Proggress = Position / 100;
            });
        }

        public async void SetPosition(double value)
        {
            await RunCommand(() => IsBusy, 
                async () => await _playerService.SeekTo((int)value * _playerService.Duration / 100));
        }

        /// <summary>
        /// Set state as adding new note 
        /// </summary>
        [RelayCommand]
        private async void AddNewNote() 
        {
            if (_playerService.CurrentShow is null)
                return;

            var showTime = CurrrentTime; 
            var text = await Shell.Current.DisplayPromptAsync("Add new note", "write something", placeholder: "Whats on your mind...", maxLength: 200);
            if (!string.IsNullOrEmpty(text))
            {
                var noteVM = new ShowNoteViewModel(text.Trim(), DateTimeOffset.UtcNow, _storgeService, _playerService, showTime);

                await RunCommand(() => IsBusy, async () => 
                {
                    await Task.Run(() => _storgeService.AddNotes(_playerService.CurrentShow, noteVM));
                });

                Notes.Add(noteVM);
            };
        }

        /// <summary>
        /// Play or pause media 
        /// </summary>
        [RelayCommand]
        private void TogglePlay() => _playerService.PlayAsync(_playerService.CurrentShow);

        [RelayCommand]
        void SeekNext() => SetPosition(Position + 1);

        [RelayCommand]
        void SeekPerv() => SetPosition(Position - 1);

        [RelayCommand]
        async void OpenSetting() 
        {
            string strSpeed = await  Shell.Current.DisplayActionSheet("Set media speed", "Cancel", null, "x0.5", "x0.75", "xNormal", "x1.25", "x1.5", "x1.75", "x2");

            float speed = 1f;
            switch (strSpeed)
            {
                case "x0.5":
                    speed = 0.5f;
                    break;
                case "x0.75":
                    speed = 0.75f;
                    break;
                case "xNormal":
                    speed = 1f;
                    break;
                case "x1.25":
                    speed = 1.25f;
                    break;
                case "x1.5":
                    speed = 1.5f;
                    break;
                case "x1.75":
                    speed = 1.75f;
                    break;
                case "x2":
                    speed = 2f;
                    break;
                default:
                    return;
            }

            _playerService.SetSpeed(speed);
        }

        [RelayCommand]
        async void AddToPlaylist()
        {
            if (_playerService.CurrentShow is null)
                return;

            var userList = _storgeService.GetUserList();

            if(userList is null || !userList.Any())
            {
                await Shell.Current.DisplayAlert("No list found", "try to make a list first ?", "ok");
                return;
            }

            var names = userList.Select(l => l.Title).ToArray();
            var userRespond = await Shell.Current.DisplayActionSheet("Select a list", "Cancel", null, names);

            if (!string.IsNullOrEmpty(userRespond) && !string.Equals(userRespond, "Cancel")) 
            {
                _storgeService.AddShow(_playerService.CurrentShow, userRespond);
            }
        }

        [RelayCommand]
        void Share() 
        {
            if (_playerService.CurrentShow is null)
                return;
            var link = RouteHelper.GetShowPage(_playerService.CurrentShow.Date, _playerService.CurrentShow.Time);
            _shareService.ShareText($"hey check this out! {link}", "Choose");
        }
    }
}
