namespace RadioArchive.Maui.ViewModels
{
    public partial class ShowNoteViewModel : BaseViewModel
    {
        private readonly ApplicationStorgeService _storgeService;
        private readonly PlayerService _playerService;

        /// <summary>
        /// Text of this note 
        /// </summary>
        [ObservableProperty]
        string _textNote;

        /// <summary>
        /// non-comited text 
        /// </summary>
        [ObservableProperty]
        string _editedText;

        /// <summary>
        /// Indicates if the current text is in edit mode 
        /// </summary>
        [ObservableProperty]
        bool _editing;

        /// <summary>
        /// Time of this note has been written
        /// </summary>
        [ObservableProperty]
        DateTimeOffset _date;

        /// <summary>
        /// Time span of media time when this note has been written 
        /// </summary>
        [ObservableProperty]
        TimeSpan _podcastTime;

        public ShowNoteViewModel(string textNote, DateTimeOffset date, ApplicationStorgeService storgeService, PlayerService playerService, TimeSpan podcastTime = default)
        {
            _storgeService = storgeService;
            _playerService = playerService;
            TextNote = textNote;
            Date = date;
            PodcastTime = podcastTime;
        }

        /// <summary>
        /// Cancel edit 
        /// </summary>
        [RelayCommand]
        private void Cancel() => Editing = false;

        /// <summary>
        /// Commits edits and save the value 
        /// </summary>
        [RelayCommand]
        private async void Save()
        {
            Editing = false;
            TextNote = EditedText.Trim();

            await RunCommand(() => IsBusy, async () =>
            {
                await Task.Run(() => _storgeService.UpdateNote(this));
            });
        }

        /// <summary>
        /// Puts the current control in edit mod 
        /// </summary>
        [RelayCommand]
        private async void Edit()
        {
            var editedText = await Shell.Current.DisplayPromptAsync("Edit", "", maxLength: 200, initialValue: TextNote);

            if (editedText is null || string.Equals(editedText.Trim(), TextNote))
                return;

            TextNote = editedText;

            await RunCommand(() => IsBusy, async () =>
            {
                await Task.Run(() => _storgeService.UpdateNote(this));
            });
        }

        [RelayCommand]
        private async void SeekToTime()
        {
            await _playerService.SeekTo((int)PodcastTime.TotalMilliseconds);
        }

        /// <summary>
        /// will remove this item from Db 
        /// </summary>
        [RelayCommand]
        private async void Remove()
        {
            var respond = await Shell.Current.DisplayAlert("Remove note", "this can't be undone, are you sure?", "yes", "cancel");

            if (respond)
                await RunCommand(() => IsBusy, async () =>
                {
                    await Task.Run(() => _storgeService.RemoveNote(this));
                });
        }
    }
}
